namespace FlappyBird
{
    partial class OyunFormu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            mnuOyun = new ToolStripMenuItem();
            mnuYeniOyun = new ToolStripMenuItem();
            mnuDuraklat = new ToolStripMenuItem();
            mnuDemo = new ToolStripMenuItem();
            mnuAyrac = new ToolStripSeparator();
            mnuCikis = new ToolStripMenuItem();
            mnuYardim = new ToolStripMenuItem();
            mnuKontroller = new ToolStripMenuItem();
            pnlBilgi = new TableLayoutPanel();
            lblSkor = new Label();
            lblEnYuksek = new Label();
            lblSeviye = new Label();
            pbOyun = new PictureBox();
            oyunZamanlayici = new System.Windows.Forms.Timer(components);
            menuStrip.SuspendLayout();
            pnlBilgi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbOyun).BeginInit();
            SuspendLayout();
            //
            // menuStrip
            //
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { mnuOyun, mnuYardim });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(480, 28);
            menuStrip.TabIndex = 0;
            //
            // mnuOyun
            //
            mnuOyun.DropDownItems.AddRange(new ToolStripItem[] { mnuYeniOyun, mnuDuraklat, mnuDemo, mnuAyrac, mnuCikis });
            mnuOyun.Name = "mnuOyun";
            mnuOyun.Size = new Size(57, 24);
            mnuOyun.Text = "&Oyun";
            //
            // mnuYeniOyun
            //
            mnuYeniOyun.Name = "mnuYeniOyun";
            mnuYeniOyun.ShortcutKeys = Keys.F2;
            mnuYeniOyun.Size = new Size(224, 26);
            mnuYeniOyun.Text = "&Yeni Oyun";
            mnuYeniOyun.Click += mnuYeniOyun_Click;
            //
            // mnuDuraklat
            //
            mnuDuraklat.Name = "mnuDuraklat";
            mnuDuraklat.ShortcutKeyDisplayString = "P";
            mnuDuraklat.Size = new Size(224, 26);
            mnuDuraklat.Text = "&Duraklat / Devam";
            mnuDuraklat.Click += mnuDuraklat_Click;
            //
            // mnuDemo
            //
            mnuDemo.CheckOnClick = true;
            mnuDemo.Name = "mnuDemo";
            mnuDemo.ShortcutKeys = Keys.F3;
            mnuDemo.Size = new Size(224, 26);
            mnuDemo.Text = "D&emo (otomatik pilot)";
            mnuDemo.CheckedChanged += mnuDemo_CheckedChanged;
            //
            // mnuAyrac
            //
            mnuAyrac.Name = "mnuAyrac";
            mnuAyrac.Size = new Size(221, 6);
            //
            // mnuCikis
            //
            mnuCikis.Name = "mnuCikis";
            mnuCikis.Size = new Size(224, 26);
            mnuCikis.Text = "Çı&kış";
            mnuCikis.Click += mnuCikis_Click;
            //
            // mnuYardim
            //
            mnuYardim.DropDownItems.AddRange(new ToolStripItem[] { mnuKontroller });
            mnuYardim.Name = "mnuYardim";
            mnuYardim.Size = new Size(65, 24);
            mnuYardim.Text = "&Yardım";
            //
            // mnuKontroller
            //
            mnuKontroller.Name = "mnuKontroller";
            mnuKontroller.Size = new Size(224, 26);
            mnuKontroller.Text = "&Kontroller";
            mnuKontroller.Click += mnuKontroller_Click;
            //
            // pnlBilgi
            //
            pnlBilgi.BackColor = Color.FromArgb(40, 44, 52);
            pnlBilgi.ColumnCount = 3;
            pnlBilgi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            pnlBilgi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            pnlBilgi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            pnlBilgi.Controls.Add(lblSkor, 0, 0);
            pnlBilgi.Controls.Add(lblEnYuksek, 1, 0);
            pnlBilgi.Controls.Add(lblSeviye, 2, 0);
            pnlBilgi.Dock = DockStyle.Top;
            pnlBilgi.Location = new Point(0, 28);
            pnlBilgi.Name = "pnlBilgi";
            pnlBilgi.RowCount = 1;
            pnlBilgi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBilgi.Size = new Size(480, 40);
            pnlBilgi.TabIndex = 1;
            //
            // lblSkor
            //
            lblSkor.Dock = DockStyle.Fill;
            lblSkor.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSkor.ForeColor = Color.White;
            lblSkor.Name = "lblSkor";
            lblSkor.TabIndex = 0;
            lblSkor.Text = "Skor: 0";
            lblSkor.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblEnYuksek
            //
            lblEnYuksek.Dock = DockStyle.Fill;
            lblEnYuksek.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEnYuksek.ForeColor = Color.Gold;
            lblEnYuksek.Name = "lblEnYuksek";
            lblEnYuksek.TabIndex = 1;
            lblEnYuksek.Text = "En Yüksek: 0";
            lblEnYuksek.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSeviye
            //
            lblSeviye.Dock = DockStyle.Fill;
            lblSeviye.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSeviye.ForeColor = Color.LightSkyBlue;
            lblSeviye.Name = "lblSeviye";
            lblSeviye.TabIndex = 2;
            lblSeviye.Text = "Seviye: 1";
            lblSeviye.TextAlign = ContentAlignment.MiddleCenter;
            //
            // pbOyun
            //
            pbOyun.BackColor = Color.SkyBlue;
            pbOyun.Dock = DockStyle.Fill;
            pbOyun.Location = new Point(0, 68);
            pbOyun.Name = "pbOyun";
            pbOyun.Size = new Size(480, 640);
            pbOyun.TabIndex = 2;
            pbOyun.TabStop = false;
            pbOyun.Paint += pbOyun_Paint;
            pbOyun.MouseDown += pbOyun_MouseDown;
            //
            // oyunZamanlayici
            //
            oyunZamanlayici.Interval = 16;
            oyunZamanlayici.Tick += oyunZamanlayici_Tick;
            //
            // OyunFormu
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 708);
            Controls.Add(pbOyun);
            Controls.Add(pnlBilgi);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "OyunFormu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Flappy Bird — YMH219";
            Load += OyunFormu_Load;
            KeyDown += OyunFormu_KeyDown;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            pnlBilgi.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbOyun).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem mnuOyun;
        private ToolStripMenuItem mnuYeniOyun;
        private ToolStripMenuItem mnuDuraklat;
        private ToolStripMenuItem mnuDemo;
        private ToolStripSeparator mnuAyrac;
        private ToolStripMenuItem mnuCikis;
        private ToolStripMenuItem mnuYardim;
        private ToolStripMenuItem mnuKontroller;
        private TableLayoutPanel pnlBilgi;
        private Label lblSkor;
        private Label lblEnYuksek;
        private Label lblSeviye;
        private PictureBox pbOyun;
        private System.Windows.Forms.Timer oyunZamanlayici;
    }
}
