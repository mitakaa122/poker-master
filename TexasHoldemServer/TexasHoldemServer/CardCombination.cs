using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.main.model
{
    public class CardCombination {
        List<Card> cards;
        int[] quantityOfType;

        private int[] getIndexesForType(int type)
        {
            string indexes = "";
            for(int i = 0; i < cards.Count; i++)
            {
                if(cards[i].type == type)
                {
                    indexes += (i + " ");
                }
            }
            indexes = indexes.Substring(0, indexes.Length - 1);
            return stringToIndexes(indexes);
        }
        
        private int[] stringToIndexes(string indexes)
        {
            string[] arrOfIndexes = indexes.Split(' ');
            int[] integerArr = new int[arrOfIndexes.Length];
            for (int i = 0; i < arrOfIndexes.Length; i++)
            {
                integerArr[i] = Int32.Parse(arrOfIndexes[i]);
            }
            return integerArr;
        }

        private bool onTable(int[] indexes)
        {
            for(int i = 0; i < indexes.Length; i++)
            {
                if (!cards[indexes[i]].onHand) return false;
            }
            return true;
        }

        private int getMaxCard(params int[] values)
        {
            Array.Sort(values);
            return values[values.Length - 1];
        }

        private int[] quantity()
        {
            int[] array = new int[13];
            for (int i = 0; i < cards.Count; i++)
            {
                array[cards[i].type]++;
            }
            return array;
        }

        private int ofKind(int value)
        {

            for (int i = 0; i < quantityOfType.Length; i++)
            {
                if (quantityOfType[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        private int getAdditionallyValue(int[] indexes)
        {
            Array.Sort(indexes);
            Card[] CardOnTable = new Card[5 - indexes.Length];
            Card[] CardOnHand = new Card[2];
            int j = 0;
            int k = 0;
            for(int i = 0; i < cards.Count; i++)
            {
                if (indexes[i] != i)
                {
                    if (cards[i].onHand)
                    {
                        CardOnHand[j] = cards[i];
                        j++;
                    }
                    else
                    {
                        CardOnTable[k] = cards[i];
                        k++;
                    }
                }
            }
            return Math.Max(getMaxValue(CardOnHand),getMaxValue(CardOnTable));
        }

        private int getMaxValue(Card[] arrOfCards)
        {
            int max = 0;
            for(int i = 0; i < arrOfCards.Length; i++)
            {
                if(arrOfCards[i].type > max)
                {
                    max = arrOfCards[i].type;
                }
            }
            return max;
        }

        public double determineHandRank(List<Card> cards)
        {
            this.cards = cards;
            this.quantityOfType = quantity();
            double value = 0;
            try
            {
                if ((value = isFlushStraight(cards)) >= 5000) return value;
                if ((value = isCare(cards)) >= 1000) return value;
                if ((value = isFullHouse(cards)) >= 500) return value;
                if ((value = isFlush(cards)) >= 250) return value;
                if ((value = isStraight(cards)) >= 200) return value;
                if ((value = isSet(cards)) >= 150) return value;
                if ((value = isTwoPair(cards)) >= 100) return value;
                if ((value = isPair(cards)) >= 50) return value;
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].onHand)
                        value += cards[i].type;
                }
            }catch (Exception e)
            {
                Console.WriteLine("Card combination exception");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return value;


        }

        double isPair(List<Card> cards)
        {
            int pair = 0;
            int highCard = 0;
            for(int i = 1; i < cards.Count; i++)
            {
                if (cards[i - 1].type == cards[i].type)
                {

                    highCard = cards[i].type;
                    ++pair;
                }
            }
            return pair == 1 ? (50 + highCard) : 0;
        }

        double isTwoPair(List<Card> cards)
        {
            int pair = 0;
            int highCard = 0;
            string indexesString = "";
            int additionalValue = 0;
            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i - 1].type == cards[i].type)
                {
                    ++pair;
                    highCard = cards[i].type;
                    indexesString += (i-1 + " ");
                    indexesString += (i + " ");
                }
            }
            if (pair == 2)
            {
                indexesString = indexesString.Substring(0, indexesString.Length - 1);
                int[] indexes = stringToIndexes(indexesString);
                if (onTable(indexes))
                {
                    additionalValue = getAdditionallyValue(indexes);
                }
            }
            return pair == 2 ? (100 + highCard + additionalValue) : 0;
        }

        double isSet(List<Card> cards)
        {
            int set = 0;
            int highCard = 0;
            int res = ofKind(3);
            int additionalValue = 0;
            if(res != -1)
            {
                int[] indexes = getIndexesForType(res);
                if (onTable(indexes))
                {
                    additionalValue = getAdditionallyValue(indexes);
                }
                highCard = res;
                set = 3;

            }
            return set == 3 ? (150 + highCard) : 0;
        }

        double isStraight(List<Card> cards)
        {
            if(cards[cards.Count - 1].type == 12 && quantityOfType[0] >= 1 
                   && quantityOfType[1] >= 1 && quantityOfType[2] >= 1
                        && quantityOfType[3] >= 1) {
                return 203;
            }
            int straight = 1;
            int highCard = 0;
            for(int i = 1; i < cards.Count; i++)
            {
                if (cards[i - 1].type == cards[i].type - 1)
                {
                    ++straight;
                    highCard = cards[i].type;
                } else {
                    if(straight != 5)
                    {
                        straight = 1;
                    }
                }
            }
            return straight == 5 ? (200 + highCard) : 0;
        }

        double isFlush(List<Card> cards)
        {
            int highCardH = 0, highCardP = 0, highCardT = 0, highCardC = 0;
            int tail = 0, pike = 0, heart = 0, clever = 0;
            for (int i = 1; i < cards.Count; i++)
            {
                switch (cards[i].suit)
                {
                    case 0:
                        ++heart;
                        if(highCardH < cards[i].type)
                        {
                            highCardH = cards[i].type;
                        }
                        break;
                    case 1:
                        ++tail;
                        if (highCardT < cards[i].type)
                        {
                            highCardT = cards[i].type;
                        }
                        break;
                    case 2:
                        ++clever;
                        if (highCardC < cards[i].type)
                        {
                            highCardC = cards[i].type;
                        }
                        break;
                    case 3:
                        ++pike;
                        if (highCardP < cards[i].type)
                        {
                            highCardP = cards[i].type;
                        }
                        break;
                }   
            }
            int highCard = getMaxCard(highCardC, highCardH, highCardP, highCardT);
            return (heart == 5 || tail == 5 || pike == 5 || clever == 5) ? (250 + highCard) : 0;
        }

        double isFullHouse(List<Card> cards)
        {
            int full = 0;
            int count = 1;
            int highCard = 0;
            bool hasSet = false;
            for(int i = 1; i < cards.Count; i++)
            {
                if(cards[i-1].type == cards[i].type)
                {
                    ++count;
                }
                else
                {
                    if(count == 3)
                    {
                        hasSet = true;
                        if(highCard < cards[i - 1].type)
                        {
                            highCard = cards[i - 1].type;
                        }
                        full += count;
                        count = 1;
                    }
                    else if(count == 2)
                    {
                        full += count;
                        count = 1;
                    }
                    else
                    {
                        count = 1;
                    }
                }
            }
            if (count == 3)
            {
                hasSet = true;
                if (highCard < cards[6].type)
                {
                    highCard = cards[6].type;
                }
                full += count;
                count = 1;
            }
            else if (count == 2)
            {
                full += count;
                count = 1;
            }
            return (full >= 5 && hasSet) ? (500 + highCard) : 0;
        }

        double isCare(List<Card> cards)
        {
            int res = ofKind(4);
            int q = 0;
            int highCard = 0;
            int additionalValue = 0;
            if(res != -1)
            {
                int[] indexes = getIndexesForType(res);
                if (onTable(indexes))
                {
                    additionalValue = getAdditionallyValue(indexes);
                }
                highCard = res;
                q = 4;
            }
            return q == 4 ? (1000 + highCard + additionalValue) : 0;
        }

        double isFlushStraight(List<Card> cards)
        {
            double royal = isFlush(cards);
            royal += isStraight(cards);
            return royal >= 450 ? 5000 : 0;
        }

        //@Test
        public static void Main(string[] args)
        {
            List<Card> cc = new List<Card>();
            cc.Add(new Card(0, 0, "Two"));
            cc.Add(new Card(1, 0, "Two"));
            cc.Add(new Card(2, 3, "Five"));
            cc.Add(new Card(3, 1, "Three"));
            cc.Add(new Card(1, 8, "Jack"));
            cc.Add(new Card(2, 5, "Seven"));
            cc.Add(new Card(3, 5, "Seven"));
            cc.Sort();
            CardCombination c = new CardCombination();
            Console.Write(c.isTwoPair(cc));
            Console.ReadKey();
        }

    }
}
