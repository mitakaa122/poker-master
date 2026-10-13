using TexasHoldemServer.model;
using TexasHoldemServer.view;
using TexasHoldem.main.controller;

namespace TexasHoldemServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Poker poker = new Poker();
            ServerView view = new ServerView();
            PokerController controller = new PokerController(poker, view);

            controller.startServer();
        }
    }
}
