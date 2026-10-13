namespace TexasHoldemClient
{
    partial class PokerWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.player1 = new System.Windows.Forms.Label();
            this.player2 = new System.Windows.Forms.Label();
            this.tableCard = new System.Windows.Forms.Label();
            this.player1cards = new System.Windows.Forms.Label();
            this.player2cards = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.winner = new System.Windows.Forms.Label();
            this.youLogin = new System.Windows.Forms.Label();
            this.p1chips = new System.Windows.Forms.Label();
            this.p2chips = new System.Windows.Forms.Label();
            this.bank = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.p2act = new System.Windows.Forms.Label();
            this.p1act = new System.Windows.Forms.Label();
            this.p1c2 = new System.Windows.Forms.PictureBox();
            this.p1c1 = new System.Windows.Forms.PictureBox();
            this.p2c1 = new System.Windows.Forms.PictureBox();
            this.p2c2 = new System.Windows.Forms.PictureBox();
            this.tc1 = new System.Windows.Forms.PictureBox();
            this.tc2 = new System.Windows.Forms.PictureBox();
            this.tc3 = new System.Windows.Forms.PictureBox();
            this.tc4 = new System.Windows.Forms.PictureBox();
            this.tc5 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.p1c2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1c1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2c1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2c2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc5)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(541, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(541, 206);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 41);
            this.button2.TabIndex = 4;
            this.button2.Text = "Call";
            this.button2.UseCompatibleTextRendering = true;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(541, 275);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 39);
            this.button3.TabIndex = 5;
            this.button3.Text = "Fold";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(541, 180);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(96, 20);
            this.textBox1.TabIndex = 6;
            // 
            // player1
            // 
            this.player1.AutoSize = true;
            this.player1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player1.Location = new System.Drawing.Point(27, 381);
            this.player1.Name = "player1";
            this.player1.Size = new System.Drawing.Size(25, 25);
            this.player1.TabIndex = 7;
            this.player1.Text = "?";
            // 
            // player2
            // 
            this.player2.AutoSize = true;
            this.player2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player2.Location = new System.Drawing.Point(27, 9);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(25, 25);
            this.player2.TabIndex = 8;
            this.player2.Text = "?";
            // 
            // tableCard
            // 
            this.tableCard.AutoSize = true;
            this.tableCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tableCard.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableCard.Location = new System.Drawing.Point(12, 184);
            this.tableCard.Name = "tableCard";
            this.tableCard.Size = new System.Drawing.Size(80, 15);
            this.tableCard.TabIndex = 11;
            this.tableCard.Text = "Card on table";
            // 
            // player1cards
            // 
            this.player1cards.AutoSize = true;
            this.player1cards.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player1cards.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player1cards.Location = new System.Drawing.Point(361, 391);
            this.player1cards.Name = "player1cards";
            this.player1cards.Size = new System.Drawing.Size(0, 15);
            this.player1cards.TabIndex = 12;
            // 
            // player2cards
            // 
            this.player2cards.AutoSize = true;
            this.player2cards.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.player2cards.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.player2cards.Location = new System.Drawing.Point(361, 9);
            this.player2cards.Name = "player2cards";
            this.player2cards.Size = new System.Drawing.Size(0, 15);
            this.player2cards.TabIndex = 13;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.status.Location = new System.Drawing.Point(538, 39);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(43, 13);
            this.status.TabIndex = 14;
            this.status.Text = "Status: ";
            // 
            // winner
            // 
            this.winner.AutoSize = true;
            this.winner.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.winner.Location = new System.Drawing.Point(538, 335);
            this.winner.Name = "winner";
            this.winner.Size = new System.Drawing.Size(49, 13);
            this.winner.TabIndex = 15;
            this.winner.Text = "Winners ";
            // 
            // youLogin
            // 
            this.youLogin.AutoSize = true;
            this.youLogin.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.youLogin.Location = new System.Drawing.Point(538, 72);
            this.youLogin.Name = "youLogin";
            this.youLogin.Size = new System.Drawing.Size(57, 13);
            this.youLogin.TabIndex = 16;
            this.youLogin.Text = "Your login:";
            // 
            // p1chips
            // 
            this.p1chips.AutoSize = true;
            this.p1chips.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.p1chips.Location = new System.Drawing.Point(361, 369);
            this.p1chips.Name = "p1chips";
            this.p1chips.Size = new System.Drawing.Size(13, 13);
            this.p1chips.TabIndex = 17;
            this.p1chips.Text = "?";
            // 
            // p2chips
            // 
            this.p2chips.AutoSize = true;
            this.p2chips.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.p2chips.Location = new System.Drawing.Point(361, 39);
            this.p2chips.Name = "p2chips";
            this.p2chips.Size = new System.Drawing.Size(13, 13);
            this.p2chips.TabIndex = 18;
            this.p2chips.Text = "?";
            this.p2chips.Click += new System.EventHandler(this.p2chips_Click);
            // 
            // bank
            // 
            this.bank.AutoSize = true;
            this.bank.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bank.Location = new System.Drawing.Point(57, 220);
            this.bank.Name = "bank";
            this.bank.Size = new System.Drawing.Size(13, 13);
            this.bank.TabIndex = 19;
            this.bank.Text = "?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(12, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Bank:";
            // 
            // p2act
            // 
            this.p2act.AutoSize = true;
            this.p2act.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.p2act.Location = new System.Drawing.Point(29, 39);
            this.p2act.Name = "p2act";
            this.p2act.Size = new System.Drawing.Size(25, 13);
            this.p2act.TabIndex = 23;
            this.p2act.Text = "???";
            // 
            // p1act
            // 
            this.p1act.AutoSize = true;
            this.p1act.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.p1act.Location = new System.Drawing.Point(29, 359);
            this.p1act.Name = "p1act";
            this.p1act.Size = new System.Drawing.Size(25, 13);
            this.p1act.TabIndex = 24;
            this.p1act.Text = "???";
            // 
            // p1c2
            // 
            this.p1c2.Location = new System.Drawing.Point(246, 330);
            this.p1c2.Name = "p1c2";
            this.p1c2.Size = new System.Drawing.Size(50, 73);
            this.p1c2.TabIndex = 25;
            this.p1c2.TabStop = false;
            // 
            // p1c1
            // 
            this.p1c1.Location = new System.Drawing.Point(179, 330);
            this.p1c1.Name = "p1c1";
            this.p1c1.Size = new System.Drawing.Size(50, 73);
            this.p1c1.TabIndex = 26;
            this.p1c1.TabStop = false;
            // 
            // p2c1
            // 
            this.p2c1.Location = new System.Drawing.Point(179, 12);
            this.p2c1.Name = "p2c1";
            this.p2c1.Size = new System.Drawing.Size(50, 73);
            this.p2c1.TabIndex = 28;
            this.p2c1.TabStop = false;
            // 
            // p2c2
            // 
            this.p2c2.Location = new System.Drawing.Point(246, 12);
            this.p2c2.Name = "p2c2";
            this.p2c2.Size = new System.Drawing.Size(50, 73);
            this.p2c2.TabIndex = 27;
            this.p2c2.TabStop = false;
            // 
            // tc1
            // 
            this.tc1.Location = new System.Drawing.Point(115, 174);
            this.tc1.Name = "tc1";
            this.tc1.Size = new System.Drawing.Size(50, 73);
            this.tc1.TabIndex = 29;
            this.tc1.TabStop = false;
            // 
            // tc2
            // 
            this.tc2.Location = new System.Drawing.Point(180, 174);
            this.tc2.Name = "tc2";
            this.tc2.Size = new System.Drawing.Size(50, 73);
            this.tc2.TabIndex = 30;
            this.tc2.TabStop = false;
            // 
            // tc3
            // 
            this.tc3.Location = new System.Drawing.Point(246, 174);
            this.tc3.Name = "tc3";
            this.tc3.Size = new System.Drawing.Size(50, 73);
            this.tc3.TabIndex = 31;
            this.tc3.TabStop = false;
            // 
            // tc4
            // 
            this.tc4.Location = new System.Drawing.Point(311, 174);
            this.tc4.Name = "tc4";
            this.tc4.Size = new System.Drawing.Size(50, 73);
            this.tc4.TabIndex = 32;
            this.tc4.TabStop = false;
            // 
            // tc5
            // 
            this.tc5.Location = new System.Drawing.Point(377, 174);
            this.tc5.Name = "tc5";
            this.tc5.Size = new System.Drawing.Size(50, 73);
            this.tc5.TabIndex = 33;
            this.tc5.TabStop = false;
            // 
            // PokerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(694, 415);
            this.Controls.Add(this.tc5);
            this.Controls.Add(this.tc4);
            this.Controls.Add(this.tc3);
            this.Controls.Add(this.tc2);
            this.Controls.Add(this.tc1);
            this.Controls.Add(this.p2c1);
            this.Controls.Add(this.p2c2);
            this.Controls.Add(this.p1c1);
            this.Controls.Add(this.p1c2);
            this.Controls.Add(this.p1act);
            this.Controls.Add(this.p2act);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bank);
            this.Controls.Add(this.p2chips);
            this.Controls.Add(this.p1chips);
            this.Controls.Add(this.youLogin);
            this.Controls.Add(this.winner);
            this.Controls.Add(this.status);
            this.Controls.Add(this.player2cards);
            this.Controls.Add(this.player1cards);
            this.Controls.Add(this.tableCard);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.player1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "PokerWindow";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.global_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.p1c2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1c1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2c1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2c2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label player1;
        private System.Windows.Forms.Label player2;
        private System.Windows.Forms.Label tableCard;
        private System.Windows.Forms.Label player1cards;
        private System.Windows.Forms.Label player2cards;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label winner;
        private System.Windows.Forms.Label youLogin;
        private System.Windows.Forms.Label p1chips;
        private System.Windows.Forms.Label p2chips;
        private System.Windows.Forms.Label bank;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label p2act;
        private System.Windows.Forms.Label p1act;
        private System.Windows.Forms.PictureBox p1c2;
        private System.Windows.Forms.PictureBox p1c1;
        private System.Windows.Forms.PictureBox p2c1;
        private System.Windows.Forms.PictureBox p2c2;
        private System.Windows.Forms.PictureBox tc1;
        private System.Windows.Forms.PictureBox tc2;
        private System.Windows.Forms.PictureBox tc3;
        private System.Windows.Forms.PictureBox tc4;
        private System.Windows.Forms.PictureBox tc5;
    }
}

