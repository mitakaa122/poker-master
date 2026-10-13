using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.main.model;

namespace TexasHoldemServer.model
{
    public class Poker
    {
        private CardStack deck;
        private Blind blind;
        private CardCombination combo;

        private List<Player> players;
        private List<Player> winners;

        private List<Card> tableCards;
        private int smallBlind;
        private int bigBlind;
        private int turn = 0;
        public enum Stage { PREFLOP, FLOP, TURN, RIVER, FINISH}

        public Poker()
        {
            combo = new CardCombination();
            players = new List<Player>();
            blind = new Blind();
            deck = new CardStack();
            bigBlind = 1;
            smallBlind = 0;
            blind.upBlinds(50, 50);
        }

        public Blind getBank()
        {
            return blind;
        }

        public void setPlayer(Player player)
        {
            players.Add(player);
        }

        public int getTurn()
        {
            return turn;
        }

        public void resetQueue()
        {
            turn = 0;
        }


        public void next()
        {
            if (turn >= players.Count - 1)
            {
                turn = -1;
                return;
            }
            ++turn;
        }

        public void playPoker(Stage stage)
        {
            try
            {
                switch (stage)
                {
                    case Stage.PREFLOP:
                        {
                            // инициализируем карты на столе
                            tableCards = new List<Card>();
                            // берем новую колоду
                            deck.reset();
                            // тусуем 
                            deck.shuffle();
                            // ставим блайнды
                            smallAndBig();
                            // раздаем игрокам
                            for (int i = 0; i < players.Count; i++)
                            {
                                players[i].setCards(deck.popToHand(), deck.popToHand());
                            }
                            break;
                        }
                    case Stage.FLOP:
                        tableCards.Add(deck.pop());
                        tableCards.Add(deck.pop());
                        tableCards.Add(deck.pop());
                        break;
                    case Stage.TURN:
                        tableCards.Add(deck.pop());
                        break;
                    case Stage.RIVER:
                        tableCards.Add(deck.pop());
                        break;
                    case Stage.FINISH:
                        determineWinners();
                        break;
                }
            } catch (Exception e)
            {
                Console.WriteLine("Poker, play poker");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        private void smallAndBig()
        {
            try
            {
                Player small = players[smallBlind];
                long total1 = small.getChips();
                long bet1 = blind.getLower();
                small.setChips(total1 - bet1);
                blind.bet(bet1);

                Player big = players[bigBlind];
                long total2 = big.getChips();
                long bet2 = blind.getUpper();
                big.setChips(total2 - bet2);
                blind.bet(bet2);
            }
            catch (Exception e)
            {
                Console.WriteLine("Poker, smallAndBiG exception");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }

        

        public void determineWinners()
        {
            winners = new List<Player>();
            double scalar = 0;
            // определяем победителя 
            try
            {
                for (int i = 0; i < players.Count; i++)
                {
                    scalar = combo.determineHandRank(
                        Card.joinCards(players[i].getCards(), tableCards));
                    players[i].pokerCombo = scalar;
                    Console.WriteLine(players[i].getNickname() + " has: " + players[i].pokerCombo);
                }
                players = players.OrderBy(i => i.pokerCombo).ToList();
                winners.Add(players[players.Count - 1]);
                for (int i = players.Count - 2; i >= 0; i--)
                {
                    if (players[i].pokerCombo == players[i + 1].pokerCombo)
                    {
                        winners.Add(players[i]);
                    }
                }
                long bank = blind.getBank() / winners.Count;
                for (int i = 0; i < winners.Count; i++)
                {
                    long chips = winners[i].getChips();
                    winners[i].setChips(chips + bank);
                }

            } catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Poker, determine winners. IndexOutOfRangeException");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            } catch (Exception e)
            {
                Console.WriteLine("Poker, determine winners");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                players = players.OrderBy(i => i.number).ToList();
                Console.WriteLine("Winner has selected");
            }

        }   

        public string getStringWinners()
        {
            return extractWinners(winners);
        }

        private string extractWinners(List<Player> winners)
        {
            string resp = "||winners|";
            for(int i = 0; i < winners.Count; i++)
            {
                resp += winners[i].getNickname() + ",";
            }
            return resp.Substring(0, resp.Length - 1);
        }

        public Player determineWinners(int id)
        {
            long chips = players[id].getChips();
            players[id].setChips(chips + blind.getBank());
            return players[id];
        }

        public string getCards()
        {
            string request = "";
            for(int i = 0; i < tableCards.Count; i++)
            {
                request += tableCards[i].ToCode() + ",";
            }
            if(request.Length != 0)
            {
                request = request.Substring(0, request.Length - 1);
            }
            return request;
        }

        public List<Player> getPlayers()
        {
            return this.players;
        }

    }
}
