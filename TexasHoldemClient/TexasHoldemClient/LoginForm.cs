using System;
using System.Drawing;
using System.Windows.Forms;

namespace TexasHoldemClient.view
{
    public partial class LoginForm : Form
    {
        private readonly TextBox host = new TextBox();
        public LoginForm()
        {
            InitializeComponent();
            Text = "Poker - Join table";
            ClientSize = new Size(320, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            label1.Text = "Player name";
            login.Text = "Vlad";
            login.MaxLength = 20;
            label2.Location = new Point(16, 104);
            label2.Text = "Server IP or hostname";
            host.SetBounds(16, 125, 270, 24);
            host.Text = "127.0.0.1";
            Controls.Add(host);
            Port.Location = new Point(16, 160);
            textBox1.Location = new Point(16, 188);
            textBox1.Text = "8805";
            button1.SetBounds(16, 235, 150, 32);
            button1.Text = "Join table";
            AcceptButton = button1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int port;
            if (string.IsNullOrWhiteSpace(login.Text) || string.IsNullOrWhiteSpace(host.Text) ||
                !int.TryParse(textBox1.Text, out port) || port < 1 || port > 65535)
            {
                MessageBox.Show(this, "Enter a player name, server address and a port from 1 to 65535.", "Check connection settings");
                return;
            }
            var table = new PokerWindow(host.Text.Trim(), port, login.Text.Trim());
            table.FormClosed += delegate { Show(); };
            Hide();
            table.Show();
        }
        public void global_FormClosed(object sender, EventArgs e) { Application.Exit(); }
    }
}
