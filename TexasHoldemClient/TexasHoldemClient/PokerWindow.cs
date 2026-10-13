using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokerNet;

namespace TexasHoldemClient
{
    public partial class PokerWindow : Form
    {
        private readonly string host, playerName, token;
        private readonly int port;
        private readonly Timer timer = new Timer();
        private readonly Button raise = new Button(), ready = new Button();
        private readonly Label limits = new Label();
        private Reply state;
        private bool busy, joined;
        public PokerWindow(string host, int port, string name, bool autoConnect = true)
        {
            this.host = host; this.port = port; playerName = name;
            string sessionDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions");
            Directory.CreateDirectory(sessionDirectory);
            string sessionKey;
            using (var hash = SHA256.Create()) sessionKey = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(host + ":" + port + ":" + name))).Replace("-", "");
            string sessionFile = Path.Combine(sessionDirectory, sessionKey + ".session.json");
            token = File.Exists(sessionFile) ? File.ReadAllText(sessionFile) : Guid.NewGuid().ToString("N");
            File.WriteAllText(sessionFile, token);
            InitializeComponent();
            Text = "Poker - " + name + " @ " + host + ":" + port;
            ClientSize = new Size(800, 500);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ForeColor = Color.White;
            foreach (Control c in Controls) if (c is Button || c is TextBox) c.ForeColor = Color.Black;
            player1cards.Text = player2cards.Text = "";
            tableCard.Text = "Community cards";
            youLogin.Text = "You: " + name;
            youLogin.Location = new Point(545, 20);
            status.SetBounds(16, 420, 760, 25);
            status.AutoSize = false;
            winner.SetBounds(16, 451, 760, 25);
            winner.AutoSize = false;
            button1.SetBounds(545, 70, 110, 34);
            button2.SetBounds(545, 112, 110, 34);
            button3.SetBounds(545, 154, 110, 34);
            textBox1.SetBounds(545, 207, 110, 24);
            textBox1.Text = "40";
            raise.SetBounds(545, 240, 110, 34);
            raise.Text = "Raise to";
            raise.ForeColor = Color.Black;
            raise.Click += async delegate { int value; if (int.TryParse(textBox1.Text, out value)) await Act("raise", value); else MessageBox.Show(this, "Enter a whole number."); };
            ready.SetBounds(545, 335, 140, 34);
            ready.Text = "Next hand";
            ready.ForeColor = Color.Black;
            ready.Click += async delegate { await Exchange(new Request { Command = "ready", Version = state.Version }); };
            limits.SetBounds(545, 278, 240, 52);
            Controls.AddRange(new Control[] { raise, ready, limits });
            foreach (Control control in Controls)
            {
                var button = control as Button;
                if (button != null) { button.BackColor = SystemColors.Control; button.UseVisualStyleBackColor = true; }
            }
            EnableActions(false);
            ready.Enabled = false;
            timer.Interval = 700;
            timer.Tick += async delegate { await Poll(); };
            if (autoConnect) Shown += async delegate { timer.Start(); await Poll(); };
        }
        private async Task Poll()
        {
            await Exchange(new Request { Command = joined ? "state" : "join", Name = playerName });
        }
        private async Task Exchange(Request request)
        {
            if (busy || IsDisposed) return;
            busy = true;
            if (request.Command != "state") { EnableActions(false); ready.Enabled = false; }
            request.Token = token;
            try
            {
                Reply reply = await Task.Run(() => Wire.Send(host, port, request));
                if (IsDisposed) return;
                if (reply.Players != null)
                {
                    joined = true;
                    state = reply;
                    Render();
                }
                if (!string.IsNullOrEmpty(reply.Error))
                {
                    if (reply.Players == null) { joined = false; EnableActions(false); ready.Enabled = false; }
                    status.Text = reply.Error;
                    if (request.Command != "state" && request.Command != "join") MessageBox.Show(this, reply.Error, "Action not accepted");
                }
            }
            catch (Exception e)
            {
                if (!IsDisposed) { EnableActions(false); ready.Enabled = false; status.Text = "Connection lost. Retrying... " + e.Message; }
            }
            finally { busy = false; }
        }
        private Task Act(string action, int amount = 0)
        {
            return Exchange(new Request { Command = "action", Action = action, Amount = amount, Version = state.Version });
        }
        private void Render()
        {
            var p = state.Players;
            player1.Text = p[0].Name + (state.Dealer == 0 ? " (D)" : "");
            player2.Text = p.Length > 1 ? p[1].Name + (state.Dealer == 1 ? " (D)" : "") : "Waiting...";
            p1chips.Text = "Chips: " + p[0].Chips;
            p2chips.Text = p.Length > 1 ? "Chips: " + p[1].Chips : "";
            p1act.Text = p[0].Action + " / Bet: " + p[0].Bet;
            p2act.Text = p.Length > 1 ? p[1].Action + " / Bet: " + p[1].Bet : "";
            bank.Text = state.Pot.ToString();
            winner.Text = state.Result;
            status.Text = "Hand " + state.Hand + " - " + state.Stage + (state.Turn == state.You ? " - Your turn" : state.Turn >= 0 ? " - Waiting for " + p[state.Turn].Name : "");
            var boxes = new[] { p1c1, p1c2, p2c1, p2c2, tc1, tc2, tc3, tc4, tc5 };
            for (int i = 0; i < 4; i++)
            {
                string card = i / 2 < p.Length && p[i / 2].Cards.Length > i % 2 ? p[i / 2].Cards[i % 2] : null;
                DrawCard(boxes[i], card);
            }
            for (int i = 0; i < 5; i++) DrawCard(boxes[i + 4], i < state.Board.Length ? state.Board[i] : null);
            bool mine = state.Turn == state.You;
            EnableActions(mine);
            button1.Enabled = mine && state.ToCall == 0;
            button2.Enabled = mine && state.ToCall > 0;
            button2.Text = "Call " + state.ToCall;
            int existing = p[state.You].Bet + state.ToCall;
            raise.Enabled = textBox1.Enabled = mine && state.MaxRaiseTo > existing;
            limits.Text = "Total street bet\nMin: " + Math.Min(state.MinRaiseTo, state.MaxRaiseTo) + "   Max: " + state.MaxRaiseTo;
            ready.Enabled = state.Stage == "Finished" && !p[state.You].Ready && Array.TrueForAll(p, s => s.Chips > 0);
            if (state.Stage == "Finished" && Array.Exists(p, s => s.Chips == 0)) status.Text = "Match over. Restart the server to play again.";
        }
        private static void DrawCard(PictureBox box, string card)
        {
            if (object.Equals(box.Tag, card)) return;
            box.Tag = card;
            var old = box.Image;
            box.Image = null;
            if (old != null) old.Dispose();
            if (card == null) return;
            var bitmap = new Bitmap(100, 146);
            using (var g = Graphics.FromImage(bitmap))
            using (var font = new Font("Segoe UI", 26, FontStyle.Bold))
            using (var small = new Font("Segoe UI Symbol", 30))
            {
                g.Clear(card == "??" ? Color.SteelBlue : Color.Ivory);
                g.DrawRectangle(Pens.Black, 0, 0, 99, 145);
                if (card == "??") g.DrawString("?", font, Brushes.White, 29, 43);
                else
                {
                    bool red = card[1] == 'h' || card[1] == 'd';
                    Brush ink = red ? Brushes.Firebrick : Brushes.Black;
                    g.DrawString(card[0] == 'T' ? "10" : card[0].ToString(), font, ink, 5, 3);
                    string suit = card[1] == 'h' ? "♥" : card[1] == 'd' ? "♦" : card[1] == 'c' ? "♣" : "♠";
                    g.DrawString(suit, small, ink, 22, 60);
                }
            }
            box.Image = bitmap;
            box.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void EnableActions(bool enabled)
        {
            button1.Enabled = button2.Enabled = button3.Enabled = raise.Enabled = textBox1.Enabled = enabled;
        }
        private async void button1_Click(object sender, EventArgs e) { await Act("check"); }
        private async void button2_Click(object sender, EventArgs e) { await Act("call"); }
        private async void button3_Click(object sender, EventArgs e) { await Act("fold"); }
        private void label2_Click(object sender, EventArgs e) { }
        private void p2chips_Click(object sender, EventArgs e) { }
        public void global_FormClosed(object sender, EventArgs e)
        {
            timer.Stop(); timer.Dispose();
            foreach (Control control in Controls)
            {
                var box = control as PictureBox;
                if (box != null && box.Image != null) box.Image.Dispose();
            }
        }
    }
}
