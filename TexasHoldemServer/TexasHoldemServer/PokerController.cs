using System;
using TexasHoldemServer.view;
using TexasHoldemServer.model;
using System.Text;
using System.Collections.Generic;
using TexasHoldem.main.model;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TexasHoldem.main.controller
{
    class PokerController
    {
        private Poker model;
        private ServerView view;
        private IPEndPoint ipPoint;
        private Socket listenSocket;

        private string[] playersState;
        // количество игроков
        private static int players = -1;

        private static bool isStartGame;
        private static bool isFinishRound;
        private static bool isFinishGame;
        private static bool allPlayersFold;

        // игровые моменты
        private int lastPlayer;
        private int playersFinish;

        // size of lobby
        private int PLAYERS = 2;

        public PokerController(Poker model, ServerView view)
        {
            this.model = model;
            this.view = view;
            playersState = new string[PLAYERS];
            ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8805);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void startServer()
        {
            model = new Poker();
            // connection & waiting players
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                Console.WriteLine("server has started");
                while(!isStartGame)
                {
                    Socket handler = listenSocket.Accept();
                    string msg;
                    int bytes = 0;
                    byte[] data = new byte[1024];
                    do
                    {
                        bytes = handler.Receive(data);
                        msg = Encoding.Unicode.GetString(data, 0, bytes);
                    } while (handler.Available > 0);
                    string resp = msgHandler(msg);
                    data = Encoding.Unicode.GetBytes(resp);
                    handler.Send(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } catch(Exception e)
            {
                Console.WriteLine("Connection exception");
                Console.WriteLine(e.Message);
            }
            // начинаем игру
            playPoker();

        }

        private void playPoker()
        {
            int gameCounter = 0;
            try
            {
                while (true)
                {
                    playersFinish = 0;
                    lastPlayer = -1;
                    isFinishGame = false;
                    allPlayersFold = false;
                    initState();
                    Console.WriteLine("game # " + ++gameCounter);
                    foreach (Poker.Stage stage in Enum.GetValues(typeof(Poker.Stage)))
                    {
                        model.playPoker(stage);
                        initStateWithoutFold();
                        // стадия игры
                        Console.WriteLine("game " + stage.ToString());
                        isFinishRound = false;
                        // получение запроса
                        while (!isFinishRound)
                        {
                            string resp;
                            Socket handler = listenSocket.Accept();
                            string msg;
                            int bytes = 0;
                            byte[] data = new byte[1024];
                            do
                            {
                                bytes = handler.Receive(data);
                                msg = Encoding.Unicode.GetString(data, 0, bytes);
                            } while (handler.Available > 0);

                            // если последний этап
                            if (stage == Poker.Stage.FINISH)
                            {
                                resp = msgHandler("finish");
                                // все игроки должны получить сообщение об окончании игры!
                                playersFinish++;
                                if (playersFinish == PLAYERS)
                                {
                                    isFinishRound = true;
                                }
                            } else if (allPlayersFold) {
                                resp = msgHandler("finish");
                                playersFinish++;
                                if (playersFinish == PLAYERS)
                                {
                                    isFinishRound = true;
                                    isFinishGame = true;
                                }

                            }
                            // обработка запроса
                            else
                            {
                                resp = msgHandler(msg);
                            }

                            // отправка ответа
                            data = Encoding.Unicode.GetBytes(resp);
                            handler.Send(data);
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        if (isFinishGame)
                        {
                            model.resetQueue();
                            break;
                        }
                    }
                    // Делаем паузу.
                    Thread.Sleep(3000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Poker Controller, stage handler");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("game over");
                Console.ReadKey();
            }
        }

        private void actionHandler(string action, int player)
        {
            string[] args = action.Split(':');
            Console.WriteLine("player: " + player + " action: " + action);
            try
            {
                switch (args[0])
                {
                    case "call":
                        playersState[player] = action;
                        long bet = long.Parse(args[1]);
                        model.getBank().bet(bet);
                        Player p = model.getPlayers()[player];
                        long chips = p.getChips();
                        p.setChips(chips - bet);
                        model.next();
                        break;
                    case "check":
                        playersState[player] = "check";
                        model.next();
                        break;
                    case "fold":
                        playersState[player] = "fold";
                        model.next();
                        break;
                }
            } catch (Exception e)
            {
                Console.WriteLine("Poker Controller, action msg handler");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }

        private bool stageCall()
        {
            int count = 0;
            for (int i = 0; i < PLAYERS; i++)
            {
                // fix me
                string[] param = playersState[i].Split(':');
                if (param[0].Equals("call") || param[0].Equals("fold"))
                {
                    count++;
                }
            }
            return count == PLAYERS || count == 0;
        }

        private bool stageFinish(int id)
        {
            for(int i = 0; i < PLAYERS; i++)
            {
                if (i != id && !playersState[i].Equals("fold"))
                {
                    return false;
                }
            }
            return true;
        }

        private string msgHandler(string msg)
        {
            string game_inf = "deny";
            string[] args = msg.Split('|');
            try {
                switch (args[0])
                {
                    case "connect":
                        model.setPlayer(new Player(++players, args[1], long.Parse(args[2])));
                        game_inf = players.ToString();
                        break;
                    case "game":
                        if (players < 1)
                        {
                            return "no_players";
                        } else
                        {
                            isStartGame = true;
                            return logins();
                        }
                    case "stage":
                        {
                            // id игрока
                            int playerID = int.Parse((args[1].Split(':')[1]));
                            // если игрок скинул карты, он пропускает свои ходы.
                            if(playersState[playerID].Equals("fold"))
                            {
                                model.next();
                                return getInf("not_u_turn");
                            }
                            // если все скинули карты
                            if (stageFinish(playerID))
                            {
                                lastPlayer = playerID;
                                allPlayersFold = true;
                                playersFinish++;
                                return msgHandler("finish");
                            }
                            if (checkTurn(args[1]))
                            {
                                return getInf("u_turn");
                            }
                            else
                            {
                                return getInf("not_u_turn");
                            }
                        }
                    case "action":
                        {
                            string[] param = args[2].Split(':');
                            int playerID = int.Parse(param[1]);
                            string act = args[1];
                            actionHandler(act, playerID);
                            if (model.getTurn() == -1)
                            {
                                if(stageCall())
                                {
                                    isFinishRound = true;
                                }
                                model.next();

                            }
                            if (act.Equals("not_action"))
                            {
                                return getInf("u_turn");
                            }
                            else
                            {
                                return getInf("not_turn");
                            }
                        }
                    case "finish":
                        {
                            string resp;
                            if (allPlayersFold)
                            {
                                // последний оставшийся игрок
                                Player p = model.determineWinners(lastPlayer);
                                resp = getInf("win");
                                resp += "||winers|" + p.getNickname();
                            } else
                            {
                                resp = getInf("win");
                                resp += model.getStringWinners();
                            }
                            return resp;
                        }
                    default:
                        return "deny";

                }
            } catch (Exception e)
            {
                Console.WriteLine("Poker Controller, msg handler");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return game_inf;
        }

        private string getInf(string status)
        {
            string resp = status;   
            List<Player> players = model.getPlayers();
            try
            {
                for (int i = 0; i < players.Count; i++)
                {
                    resp += "||n:" + i;
                    resp += "|card:";
                    resp += players[i].getStringCards();
                    resp += "|chips:";
                    resp += players[i].getChips();
                    // action
                    resp += "|action:" + playersState[i];
                }
                resp += "||table|card:" + model.getCards() + "|chips:" + model.getBank().Total();
            } catch (Exception e)
            {
                throw new Exception("get information exception!" + e.Message);
            }
            return resp;
        }

        private bool checkTurn(string param)
        {
            string[] args = param.Split(':');
            return int.Parse(args[1]) == model.getTurn();
        }

        private string logins()
        {
            string logins = "";
            foreach(Player p in model.getPlayers()) {
                logins += p.getNickname() + "|";
            }
            return logins;
        }

        private void initStateWithoutFold()
        {
            for (int i = 0; i < PLAYERS; i++)
            {
                if (!playersState[i].Equals("fold"))
                {
                    playersState[i] = "not_action";
                }
            }
        }

        private void initState()
        {
            for (int i = 0; i < PLAYERS; i++)
            {
                playersState[i] = "not_action";
            }
        }
    }
}
