using System.Collections.Generic;

namespace TexasHoldem.main.model
{
    public class Player
    {
        public Player(int number, string nickname, long chips)
        {
            this.number = number;
            this.nickname = nickname;
            this.chips = chips;
        }
        // public fields for sort
        public double pokerCombo;
        public int number;

        private int port;
        private string nickname;
        private long chips;
        private List<Card> playerCards;

        public void setPort(int port)
        {
            this.port = port;
        }

        public int getPort()
        {
            return port;
        }

        public long getChips()
        {
            return this.chips;
        }

        public string getNickname()
        {
            return this.nickname;
        }

        public void setChips(long chips)
        {
            this.chips = chips;
        }

        public void setNumber(int number)
        {
            this.number = number;
        }

        public int getNumber()
        {
            return number;
        }

        /* public void setPokerCombo(double pokerCombo)
        {
            this.pokerCombo = pokerCombo;
        }

        public double getPokerCombo()
        {
            return pokerCombo;
        } */

        public void setCards(Card one, Card two)
        {
            playerCards = new List<Card>();
            playerCards.Add(one);
            playerCards.Add(two);
        }

        public List<Card> getCards()
        {
            return this.playerCards;
        }

        public string getStringCards()
        {
            string cards = "";
            if(playerCards != null)
            {
                cards += playerCards[0].ToCode() + "," +
                            playerCards[1].ToCode();
                return cards;
            }
            else
            {
                return "";
            }
        }

    }
}
