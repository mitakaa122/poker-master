using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.main.model
{
    public class Card : IComparable<Card>
    {

        public Card(int suit, int type, string name) {
            this.suit = suit;
            this.type = type;
            this.name = name;
        }
        // heart - 0, tile - 1, clover - 2, pike - 3
        public bool onHand { get; set; }
        public int suit { get; }
        public int type { get; }  // A,  K,  Q,  J, 10, 9, 8, 7, 6, 5, 4, 3, 2;
                                        //12, 11, 10,  9,  8, 7, 6, 5, 4, 3, 2, 1, 0;

        public string name { get; set; }


        public List<Card> sortCards(List<Card> cards)
        {
            cards.Sort();
            return cards;
        }

        public int CompareTo(Card other)
        {
            if (this.type == other.type) return 0;
            int dif = other.type - this.type;
            return -dif / Math.Abs(dif);
        }

        public static List<Card> joinCards(List<Card> player, List<Card> table)
        {
            List<Card> join = new List<Card>();
            for(int i = 0; i < player.Count; i++)
            {
                join.Add(player[i]);
            }
            for (int i = 0; i < table.Count; i++)
            {
                join.Add(table[i]);
            }
            join.Sort();
            return join;
        }

        public string suitName(int suit)
        {
            switch(suit)
            {
                case 0:
                    return "HEART";
                case 1:
                    return "TILE";
                case 2:
                    return "CLEVER";
                case 3:
                    return "PIKE";
            }
            throw new Exception("Such suit doesn't exist");
        }

        public string ToCode()
        {
            return type + "&" + suit;
        }

        public override string ToString()
        {
            return name + "&" + suitName(this.suit);
        }

        /*// @Test
        static void Main(string[] args)
        {
            HandRankDetermine();
        }

        // @Test
        public static void HandRankDetermine()
        {

            CardCombination combination = new CardCombination();
            CardStack deck;
            for(int i = 0; i < 10; i++)
            {
                List<Card> cardsPlayerOne = new List<Card>();
                List<Card> cardsPlayerTwo = new List<Card>();
                List<Card> cardsTable = new List<Card>();
                deck = new CardStack();
                deck.shuffle();
                for(int j = 0; j < 2; j++)
                {
                    cardsPlayerOne.Add(deck.popToHand());
                    cardsPlayerTwo.Add(deck.popToHand());
                }
                //flop
                for(int j = 0; j < 3; j++)
                {
                    cardsTable.Add(deck.pop());
                }
                //turn
                cardsTable.Add(deck.pop());
                //river
                cardsTable.Add(deck.pop());

                Console.WriteLine("Player 1");
                foreach (Card card in cardsPlayerOne)
                {
                    Console.Write(card + " ");
                }
                Console.WriteLine("");
                foreach (Card card in cardsTable)
                {
                    Console.Write(card + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("Player 2");
                foreach (Card card in cardsPlayerTwo)
                {
                    Console.Write(card + " ");
                }
                Console.WriteLine();
                double p1 = combination.determineHandRank(joinCards(cardsPlayerOne, cardsTable));
                double p2 = combination.determineHandRank(joinCards(cardsPlayerTwo, cardsTable));
                Console.WriteLine(p1 + " | " + p2);

                Console.WriteLine("---------------------");
            }
            Console.Read();

        } */

    }

}
