using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;

namespace PokerNet
{
    public class Request
    {
        public string Command { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Action { get; set; }
        public int Amount { get; set; }
        public int Version { get; set; }
    }
    public class SeatView
    {
        public string Name { get; set; }
        public int Chips { get; set; }
        public int Bet { get; set; }
        public string Action { get; set; }
        public string[] Cards { get; set; }
        public bool Ready { get; set; }
    }
    public class Reply
    {
        public string Error { get; set; }
        public string Token { get; set; }
        public int You { get; set; }
        public int Version { get; set; }
        public int Hand { get; set; }
        public int Dealer { get; set; }
        public string Stage { get; set; }
        public int Turn { get; set; }
        public int Pot { get; set; }
        public int ToCall { get; set; }
        public int MinRaiseTo { get; set; }
        public int MaxRaiseTo { get; set; }
        public string Result { get; set; }
        public string[] Board { get; set; }
        public SeatView[] Players { get; set; }
    }
    public static class Wire
    {
        public static string ReadLine(Stream stream)
        {
            using (var buffer = new MemoryStream())
            {
                int b;
                while ((b = stream.ReadByte()) != -1)
                {
                    if (b == 10) return Encoding.UTF8.GetString(buffer.ToArray());
                    if (buffer.Length >= 16384) throw new IOException("Message too large.");
                    buffer.WriteByte((byte)b);
                }
                throw new IOException("Connection closed before a complete message arrived.");
            }
        }
        public static void Write(Stream stream, object value)
        {
            byte[] data = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(value) + "\n");
            stream.Write(data, 0, data.Length);
        }
        public static Reply Send(string host, int port, Request request)
        {
            using (var client = new TcpClient())
            {
                var connect = client.BeginConnect(host, port, null, null);
                using (connect.AsyncWaitHandle)
                {
                    if (!connect.AsyncWaitHandle.WaitOne(3000)) throw new IOException("Connection timed out.");
                    client.EndConnect(connect);
                }
                client.ReceiveTimeout = client.SendTimeout = 3000;
                var stream = client.GetStream();
                Write(stream, request);
                return new JavaScriptSerializer().Deserialize<Reply>(ReadLine(stream));
            }
        }
    }
}
