using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PokerNet
{
    public class Game
    {
        private class Seat
        {
            public string Name, Token, Action;
            public int Chips = 2000, Bet;
            public int[] Cards = new int[0];
            public bool Ready, Acted;
        }
        private readonly List<Seat> seats = new List<Seat>();
        private readonly List<int> board = new List<int>();
        private Queue<int> deck;
        private int version, hand, dealer = 1, turn = -1, pot, currentBet, lastRaise = 20;
        private string stage = "Lobby", result = "Waiting for a second player.";
        private bool reveal;

        public Reply Handle(Request request)
        {
            lock (seats)
            {
                if (request == null) return new Reply { Error = "Empty request." };
                int id = seats.FindIndex(s => s.Token == request.Token);
                if (request.Command == "join")
                {
                    // Reusing a client-generated token makes a retried join idempotent.
                    Guid parsed;
                    if (!Guid.TryParse(request.Token, out parsed)) return new Reply { Error = "A session token is required." };
                    if (id >= 0) return View(id);
                    string name = (request.Name ?? "").Trim();
                    if (name.Length < 1 || name.Length > 20 || name.Any(char.IsControl)) return new Reply { Error = "Use a name of 1-20 printable characters." };
                    if (seats.Count == 2) return new Reply { Error = "The table is full." };
                    if (seats.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase))) return new Reply { Error = "This name is already in use." };
                    seats.Add(new Seat { Name = name, Token = request.Token, Action = "Joined" });
                    id = seats.Count - 1;
                    version++;
                    if (seats.Count == 2) StartHand();
                    return View(id);
                }
                if (id < 0) return new Reply { Error = "Unknown session. Rejoin after a server restart." };
                if (request.Command == "state") return View(id);
                if (request.Version != version) return View(id, "The table changed. Review the current state and try again.");
                if (request.Command == "ready")
                {
                    if (stage != "Finished") return View(id, "The hand is not finished.");
                    if (seats.Any(s => s.Chips == 0)) return View(id, "Match over. Restart the server for a new match.");
                    seats[id].Ready = true;
                    version++;
                    if (seats.All(s => s.Ready)) StartHand();
                    return View(id);
                }
                if (request.Command != "action") return View(id, "Unknown command.");
                if (turn != id || stage == "Finished") return View(id, "It is not your turn.");
                Seat player = seats[id], other = seats[1 - id];
                int call = Math.Min(currentBet - player.Bet, player.Chips);
                switch (request.Action)
                {
                    case "fold":
                        player.Action = "Fold";
                        Finish(1 - id, false);
                        version++;
                        return View(id);
                    case "check":
                        if (call != 0) return View(id, "Call or fold when facing a bet.");
                        player.Action = "Check";
                        break;
                    case "call":
                        if (call == 0) return View(id, "There is no bet to call. Check instead.");
                        Pay(player, call);
                        player.Action = "Call " + call;
                        break;
                    case "raise":
                        int max = Math.Min(player.Bet + player.Chips, other.Bet + other.Chips);
                        int target = request.Amount;
                        if (other.Chips == 0 || target <= currentBet || target > max || (target < currentBet + lastRaise && target != max))
                            return View(id, "Invalid raise. Enter a total street bet within the displayed limits.");
                        lastRaise = Math.Max(lastRaise, target - currentBet);
                        Pay(player, target - player.Bet);
                        currentBet = target;
                        other.Acted = false;
                        player.Action = "Raise to " + target;
                        break;
                    default: return View(id, "Unknown action.");
                }
                player.Acted = true;
                version++;
                if (seats.All(s => s.Acted) && player.Bet == other.Bet) Advance();
                else if (seats.Any(s => s.Chips == 0) && player.Bet == other.Bet) RunOut();
                else turn = 1 - id;
                return View(id);
            }
        }
        private void Pay(Seat seat, int amount)
        {
            seat.Chips -= amount;
            seat.Bet += amount;
            pot += amount;
        }
        private void StartHand()
        {
            hand++;
            dealer = 1 - dealer;
            stage = "Preflop";
            result = "";
            reveal = false;
            pot = 0;
            board.Clear();
            int[] cards = Enumerable.Range(0, 52).ToArray();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                for (int i = cards.Length - 1; i > 0; i--)
                {
                    uint value, bound = uint.MaxValue - uint.MaxValue % (uint)(i + 1);
                    do { rng.GetBytes(bytes); value = BitConverter.ToUInt32(bytes, 0); } while (value >= bound);
                    int j = (int)(value % (uint)(i + 1));
                    int temp = cards[i]; cards[i] = cards[j]; cards[j] = temp;
                }
            }
            deck = new Queue<int>(cards);
            foreach (Seat s in seats)
            {
                s.Bet = 0; s.Ready = s.Acted = false; s.Action = "";
                s.Cards = new[] { deck.Dequeue(), deck.Dequeue() };
            }
            int effective = seats.Min(s => s.Chips);
            Pay(seats[dealer], Math.Min(10, effective));
            Pay(seats[1 - dealer], Math.Min(20, effective));
            seats[dealer].Action = "Small blind";
            seats[1 - dealer].Action = "Big blind";
            currentBet = seats.Max(s => s.Bet);
            lastRaise = 20;
            turn = dealer;
            if (seats.Any(s => s.Chips == 0) && seats[0].Bet == seats[1].Bet) RunOut();
            version++;
        }
        private void Advance()
        {
            if (board.Count == 5) { Showdown(); return; }
            if (seats.Any(s => s.Chips == 0)) { RunOut(); return; }
            DealStreet();
            foreach (Seat s in seats) { s.Bet = 0; s.Acted = false; s.Action = ""; }
            currentBet = 0;
            lastRaise = 20;
            turn = 1 - dealer;
        }
        private void DealStreet()
        {
            deck.Dequeue(); // Burn one card before each street.
            int count = board.Count == 0 ? 3 : 1;
            for (int i = 0; i < count; i++) board.Add(deck.Dequeue());
            stage = board.Count == 3 ? "Flop" : board.Count == 4 ? "Turn" : "River";
        }
        private void RunOut()
        {
            while (board.Count < 5) DealStreet();
            Showdown();
        }
        private void Showdown()
        {
            long a = HandRank.Best(seats[0].Cards.Concat(board));
            long b = HandRank.Best(seats[1].Cards.Concat(board));
            Finish(a == b ? -1 : a > b ? 0 : 1, true);
            result += " (" + HandRank.Describe(Math.Max(a, b)) + ")";
        }
        private void Finish(int winner, bool showdown)
        {
            reveal = showdown;
            if (winner == -1)
            {
                seats[0].Chips += pot / 2;
                seats[1].Chips += pot / 2;
                seats[1 - dealer].Chips += pot % 2;
                result = "Split pot: " + pot;
            }
            else
            {
                seats[winner].Chips += pot;
                result = seats[winner].Name + " wins " + pot + (showdown ? "" : " by fold");
            }
            pot = 0;
            stage = "Finished";
            turn = -1;
        }
        private Reply View(int id, string error = null)
        {
            Seat you = seats[id];
            return new Reply {
                Error = error, Token = you.Token, You = id, Version = version, Hand = hand,
                Dealer = dealer, Stage = stage, Turn = turn, Pot = pot, Result = result,
                ToCall = Math.Min(Math.Max(0, currentBet - you.Bet), you.Chips),
                MinRaiseTo = currentBet + lastRaise,
                MaxRaiseTo = seats.Min(s => s.Bet + s.Chips),
                Board = board.Select(HandRank.Card).ToArray(),
                Players = seats.Select((s, i) => new SeatView {
                    Name = s.Name, Chips = s.Chips, Bet = s.Bet, Action = s.Action, Ready = s.Ready,
                    Cards = i == id || reveal ? s.Cards.Select(HandRank.Card).ToArray() : s.Cards.Select(c => "??").ToArray()
                }).ToArray()
            };
        }
    }
}
