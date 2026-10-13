using System;
using System.Collections.Generic;
using System.Linq;
using PokerNet;

public static class GameTests
{
    private static int checks;
    private static void Assert(bool condition, string message)
    {
        checks++;
        if (!condition) throw new Exception(message);
    }
    private static int[] Cards(string text)
    {
        return text.Split(' ').Select(s => "hdcs".IndexOf(s[1]) * 13 + "23456789TJQKA".IndexOf(s[0])).ToArray();
    }
    private static long Rank(string text) { return HandRank.Best(Cards(text)); }
    private static Reply State(Game g, string token) { return g.Handle(new Request { Command = "state", Token = token }); }
    private static Reply Act(Game g, string token, string action, int amount = 0)
    {
        var s = State(g, token);
        return g.Handle(new Request { Command = "action", Token = token, Version = s.Version, Action = action, Amount = amount });
    }
    private static string[] Join(Game game)
    {
        var tokens = new[] { Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N") };
        for (int i = 0; i < 2; i++) Assert(game.Handle(new Request { Command = "join", Name = "Player" + i, Token = tokens[i] }).Error == null, "Join failed");
        return tokens;
    }
    private static void Conservation(Reply state)
    {
        Assert(state.Players.Sum(p => p.Chips) + state.Pot == 4000, "Chips were created or lost");
        Assert(state.Players.All(p => p.Chips >= 0), "Negative stack");
    }
    public static int Main()
    {
        try
        {
            Assert(HandRank.Describe(Rank("Ah Kh Qh Jh Th 2c 3s")) == "Straight flush", "Royal flush");
            Assert(HandRank.Describe(Rank("Ah Kh Qh 2h 3h Jc Ts")) == "Flush", "Separate flush and straight must not become straight flush");
            Assert(Rank("Ah 2c 3d 4s 5h 9c Td") < Rank("2h 3c 4d 5s 6h 9c Td"), "Wheel rank");
            Assert(HandRank.Describe(Rank("2h 3c 4d 5s 6h 7c 7d")) == "Straight", "Long straight with duplicate");
            Assert(HandRank.Describe(Rank("Ah Ac Ad Kh Kc Kd 2s")) == "Full house", "Two trips");
            Assert(Rank("Ah Ac Ks Qd Js 9h 2c") > Rank("Ad As Kc Qh Ts 9d 2s"), "Pair kicker");
            Assert(Rank("Ah Ac Kh Kc Qs 9h 2c") > Rank("Ad As Kd Ks Js 9d 2s"), "Two pair kicker");
            Assert(Rank("Ah Ac Ad As Kh 3c 2c") > Rank("Ah Ac Ad As Qh 3c 2c"), "Quads kicker");
            Assert(Rank("Ah Qh 9h 8h 7h 2c 3c") > Rank("Ah Qh 9h 8h 6h 2c 3c"), "Flush kicker");
            Assert(Rank("Ah Kh Qh Jh Th 2c 3s") == Rank("Ah Kh Qh Jh Th 4c 5s"), "Play the board tie");
            var g = new Game(); var t = Join(g); var s = State(g, t[0]);
            Assert(s.Turn == 0 && s.ToCall == 10 && s.Pot == 30, "Heads-up blinds");
            Assert(s.Players[1].Cards.All(c => c == "??"), "Opponent cards leaked");
            Assert(g.Handle(new Request { Command = "join", Name = "Player0", Token = t[0] }).You == 0, "Join retry");
            Assert(g.Handle(new Request { Command = "join", Name = "Third", Token = Guid.NewGuid().ToString() }).Error != null, "Third seat accepted");
            Assert(g.Handle(new Request { Command = "state", Token = "bad" }).Players == null, "Unauthenticated state");
            Assert(Act(g, t[1], "check").Error != null, "Out-of-turn action");
            Assert(Act(g, t[0], "check").Error != null, "Check facing bet");
            Assert(Act(g, t[0], "raise", -1).Error != null, "Negative raise");
            Assert(Act(g, t[0], "raise", 30).Error != null, "Under-minimum raise");
            Assert(Act(g, t[0], "raise", 999999).Error != null, "Over-stack raise");
            s = Act(g, t[0], "call");
            Assert(s.Stage == "Preflop" && s.Turn == 1, "Big blind must retain option");
            int stale = s.Version;
            s = Act(g, t[1], "check");
            Assert(s.Stage == "Flop" && s.Turn == 1, "Postflop order");
            Assert(g.Handle(new Request { Command = "action", Token = t[1], Action = "check", Version = stale }).Error != null, "Stale action accepted");
            s = Act(g, t[1], "raise", 20);
            Assert(s.Error == null && s.Turn == 0, "Opening bet");
            s = Act(g, t[0], "raise", 60);
            Assert(s.Error == null && s.MinRaiseTo == 100, "Minimum reraise");
            s = Act(g, t[1], "fold");
            Assert(s.Stage == "Finished" && s.Players[0].Cards.All(c => c == "??"), "Fold should not reveal");
            Conservation(s);
            var random = new Random(234);
            int hands = 0;
            while (hands < 500)
            {
                s = State(g, t[0]); Conservation(s);
                if (s.Stage == "Finished")
                {
                    hands++;
                    if (s.Players.Any(p => p.Chips == 0)) { g = new Game(); t = Join(g); continue; }
                    int dealer = s.Dealer;
                    for (int i = 0; i < 2; i++)
                    {
                        s = State(g, t[i]);
                        s = g.Handle(new Request { Command = "ready", Token = t[i], Version = s.Version });
                        Assert(s.Error == null, "Ready failed");
                    }
                    Assert(s.Dealer != dealer, "Dealer did not rotate");
                    continue;
                }
                int who = s.Turn;
                s = State(g, t[who]);
                int choice = random.Next(10);
                string action = s.ToCall > 0 ? "call" : "check";
                int amount = 0;
                if (choice == 0) action = "fold";
                else if (choice <= 3 && s.MaxRaiseTo > s.Players[who].Bet + s.ToCall)
                { action = "raise"; amount = choice == 1 ? s.MaxRaiseTo : Math.Min(s.MinRaiseTo, s.MaxRaiseTo); }
                s = Act(g, t[who], action, amount);
                Assert(s.Error == null, "Legal simulated action failed: " + s.Error);
                Conservation(s);
                if (s.Stage == "Finished" && s.Board.Length == 5 && s.Players.All(p => p.Cards.All(c => c != "??")))
                    Assert(s.Board.Concat(s.Players.SelectMany(p => p.Cards)).Distinct().Count() == 9, "Duplicate card");
            }
            Console.WriteLine("PASS: " + checks + " checks, 500 simulated hands, hand rankings, privacy, legality, chip conservation.");
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
}
