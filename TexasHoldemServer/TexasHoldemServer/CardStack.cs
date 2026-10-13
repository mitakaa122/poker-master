using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.main.model
{
    public class CardStack
    {
        private Card[] stack = new Card[52];

        private readonly int DeckSize = 52;

        readonly string[] names = {"Two","Three","Four","Five","Six","Seven",
                                "Eight","Nine","Ten","Jack","Queen","King","Ace"};
        private int position = -1;
        public CardStack()
        {
            int k = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 13; j++)
                {
                    stack[k] = new Card(i, j, names[j]);
                    k++;
                }
            }
        }

        public void printCardStack()
        {
            foreach(Card card in stack){
                Console.WriteLine(card);
            }
        }
        
        public void shuffle()
        {
            Random random = new Random();
            Card tmp;
            int rand = 0;
            for (int i = 0; i < DeckSize; i++)
            {
                rand = random.Next(52);
                tmp = stack[i];
                stack[i] = stack[rand];
                stack[rand] = tmp;
            }
        }

        public Card pop()
        {
            return stack[++position];
        }

        public Card popToHand()
        {
            stack[++position].onHand = true;
            return stack[position];
        }

        public void reset()
        {
            position = -1;
        }

    }
}
