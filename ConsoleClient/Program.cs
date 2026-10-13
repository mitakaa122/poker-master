using System;
using System.IO;
using System.Web.Script.Serialization;
using PokerNet;

public class Session
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}
public static class ConsoleClient
{
    public static int Main(string[] args)
    {
        try
        {
            if (args.Length < 2) throw new Exception("Usage: PokerCli session.json join HOST PORT NAME | state | ready VERSION | action VERSION check/call/fold/raise [TOTAL]");
            var json = new JavaScriptSerializer();
            Session session;
            string command = args[1];
            if (command == "join" && !File.Exists(args[0]))
            {
                if (args.Length != 5) throw new Exception("Join requires HOST PORT NAME.");
                session = new Session { Host = args[2], Port = int.Parse(args[3]), Name = args[4], Token = Guid.NewGuid().ToString("N") };
                File.WriteAllText(args[0], json.Serialize(session));
            }
            else session = json.Deserialize<Session>(File.ReadAllText(args[0]));
            var request = new Request { Command = command, Name = session.Name, Token = session.Token };
            if (command == "action" || command == "ready") request.Version = int.Parse(args[2]);
            if (command == "action") { request.Action = args[3]; if (args.Length > 4) request.Amount = int.Parse(args[4]); }
            Reply reply = Wire.Send(session.Host, session.Port, request);
            reply.Token = null;
            Console.WriteLine(json.Serialize(reply));
            return string.IsNullOrEmpty(reply.Error) ? 0 : 1;
        }
        catch (Exception e) { Console.Error.WriteLine(e.Message); return 1; }
    }
}
