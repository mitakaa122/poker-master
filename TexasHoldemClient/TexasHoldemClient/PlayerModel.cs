using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemClient
{
    public class PlayerModel
    {
        private int number;
        private string login;
        private long chips;
        private int port;

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
        public void setChips(long chips)
        {
            this.chips = chips;
        }
        public int getNumber()
        {
            return number;
        }
        public string getLogin()
        {
            return login;
        }
        public void setNumber(int number)
        {
            this.number = number;
        }
        public void setLogin(string login)
        {
            this.login = login;
        }
    }
}
