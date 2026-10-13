using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using PokerNet;
using TexasHoldemClient;
using TexasHoldemClient.view;

public static class UiPreview
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        using (var login = new LoginForm()) Save(login, "bin/login-preview.png");
        using (var table = new PokerWindow("preview.invalid", 8805, "Preview", false))
        {
            var reply = new Reply {
                You = 0, Hand = 1, Stage = "Flop", Turn = 0, Pot = 40, Dealer = 0,
                MinRaiseTo = 20, MaxRaiseTo = 1980, Board = new[] { "Th", "Jc", "2d" }, Result = "",
                Players = new[] {
                    new SeatView { Name = "Vlad", Chips = 1980, Bet = 0, Action = "", Cards = new[] { "Ah", "Ks" } },
                    new SeatView { Name = "Codex", Chips = 1980, Bet = 0, Action = "Check", Cards = new[] { "??", "??" } }
                }
            };
            typeof(PokerWindow).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(table, reply);
            typeof(PokerWindow).GetMethod("Render", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(table, null);
            Save(table, "bin/table-preview.png");
        }
        Console.WriteLine("Rendered login and table previews.");
    }
    private static void Save(Form form, string path)
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-32000, -32000);
        form.ShowInTaskbar = false;
        form.Show();
        Application.DoEvents();
        using (var image = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(image, new Rectangle(0, 0, image.Width, image.Height));
            image.Save(path);
        }
    }
}
