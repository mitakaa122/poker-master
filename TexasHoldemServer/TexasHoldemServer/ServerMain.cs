using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web.Script.Serialization;

namespace PokerNet
{
    public static class ServerMain
    {
        public static int Main(string[] args)
        {
            int port = 8805;
            if (args.Length > 0 && (!int.TryParse(args[0], out port) || port < 1 || port > 65535))
            { Console.Error.WriteLine("Usage: PokerServer.exe [port]"); return 1; }
            var game = new Game();
            var listener = new TcpListener(IPAddress.Any, port);
            try
            {
                listener.Start();
                Console.WriteLine("Poker server listening on port " + port + ". Blinds 10/20, 2000 chips, two seats.");
                Console.WriteLine("This PC: 127.0.0.1");
                foreach (var address in Dns.GetHostAddresses(Dns.GetHostName()))
                    if (address.AddressFamily == AddressFamily.InterNetwork) Console.WriteLine("LAN address: " + address);
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(delegate {
                        using (client)
                        {
                            try
                            {
                                client.ReceiveTimeout = client.SendTimeout = 3000;
                                var stream = client.GetStream();
                                var request = new JavaScriptSerializer().Deserialize<Request>(Wire.ReadLine(stream));
                                Wire.Write(stream, game.Handle(request));
                            }
                            catch (Exception e) { Console.Error.WriteLine("Client request failed: " + e.Message); }
                        }
                    });
                }
            }
            catch (Exception e) { Console.Error.WriteLine(e.Message); return 1; }
            finally { listener.Stop(); }
        }
    }
}
