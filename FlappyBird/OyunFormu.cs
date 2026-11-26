using FlappyBird.Oyun;

namespace FlappyBird
{
    /// <summary>
    /// Oyun penceresi. Üç işi var:
    ///  1) Timer her tik attığında motoru bir adım ilerletmek (oyun döngüsü),
    ///  2) Klavye/fare olaylarını motora iletmek,
    ///  3) Motorun durumunu PictureBox üzerine çizmek.
    /// Oyunun kuralları formda değil, OyunMotoru sınıfındadır.
    /// </summary>
    public partial class OyunFormu : Form
    {
        private OyunMotoru _motor = null!;   // Load olayında oluşturulur
        private readonly YuksekSkorDeposu _rekorDeposu = new();

        private readonly Font _buyukYazi = new("Segoe UI", 28F, FontStyle.Bold);
        private readonly Font _kucukYazi = new("Segoe UI", 12F, FontStyle.Bold);
        private readonly Font _skorYazisi = new("Segoe UI", 36F, FontStyle.Bold);

        public OyunFormu()
        {
            InitializeComponent();
        }

        private void OyunFormu_Load(object sender, EventArgs e)
        {
            // Oyun alanının gerçek boyutu (ekran ölçeklemesi dahil) PictureBox'tan alınır.
            _motor = new OyunMotoru(pbOyun.ClientSize.Width, pbOyun.ClientSize.Height,
                                    _rekorDeposu.Oku());
            _motor.SkorDegisti += Motor_SkorDegisti;
            _motor.OyunBitti += Motor_OyunBitti;

            BilgileriGuncelle();
            oyunZamanlayici.Start();
        }

        // ---------------------------------------------------------------
        // Oyun döngüsü
        // ---------------------------------------------------------------

        /// <summary>Saniyede ~60 kez çalışır: motoru ilerlet, ekranı yeniden çizdir.</summary>
        private void oyunZamanlayici_Tick(object sender, EventArgs e)
        {
            _motor.Guncelle();
            pbOyun.Invalidate();     // Paint olayını tetikler
        }

        // ---------------------------------------------------------------
        // Motor olayları (event)
        // ---------------------------------------------------------------

        private void Motor_SkorDegisti(object? sender, EventArgs e)
        {
            BilgileriGuncelle();
        }

        private void Motor_OyunBitti(object? sender, EventArgs e)
        {
            // Yeni rekor kırıldıysa dosyaya yaz. Yazılamazsa oyun durmaz, sadece başlıkta belirtilir.
            if (_motor.Skor > 0 && _motor.Skor == _motor.EnYuksekSkor)
            {
                bool yazildi = _rekorDeposu.Kaydet(_motor.EnYuksekSkor);
                Text = yazildi ? "Flappy Bird — YMH219 (yeni rekor kaydedildi!)"
                               : "Flappy Bird — YMH219 (rekor dosyaya yazılamadı)";
            }
            BilgileriGuncelle();
        }

        private void BilgileriGuncelle()
        {
            lblSkor.Text = $"Skor: {_motor.Skor}";
            lblEnYuksek.Text = $"En Yüksek: {_motor.EnYuksekSkor}";
            lblSeviye.Text = $"Seviye: {_motor.Seviye}";
        }

        // ---------------------------------------------------------------
        // Klavye, fare ve menü olayları
        // ---------------------------------------------------------------

        private void OyunFormu_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                case Keys.Up:
                case Keys.W:
                    if (_motor.Durum == OyunDurumu.Bitti)
                    {
                        break;   // bittikten sonra yanlışlıkla yeniden başlamasın; R / Enter kullanılır
                    }
                    _motor.Ziplat();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.P:
                    _motor.DuraklatDevamEt();
                    pbOyun.Invalidate();
                    break;

                case Keys.R:
                case Keys.Enter:
                    if (_motor.Durum == OyunDurumu.Bitti)
                    {
                        YeniOyunBaslat();
                    }
                    break;
            }
        }

        private void pbOyun_MouseDown(object sender, MouseEventArgs e)
        {
            if (_motor.Durum != OyunDurumu.Bitti)
            {
                _motor.Ziplat();
            }
        }

        private void mnuYeniOyun_Click(object sender, EventArgs e)
        {
            YeniOyunBaslat();
        }

        private void mnuDuraklat_Click(object sender, EventArgs e)
        {
            _motor.DuraklatDevamEt();
            pbOyun.Invalidate();
        }

        private void mnuDemo_CheckedChanged(object sender, EventArgs e)
        {
            _motor.OtomatikPilot = mnuDemo.Checked;
        }

        private void mnuCikis_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuKontroller_Click(object sender, EventArgs e)
        {
            bool oynuyordu = _motor.Durum == OyunDurumu.Oynaniyor;
            if (oynuyordu) _motor.DuraklatDevamEt();

            MessageBox.Show(
                "Boşluk / Yukarı ok / W / fare tıklaması : zıpla\n" +
                "P : duraklat / devam et\n" +
                "R veya Enter : oyun bitince yeniden başla\n" +
                "F2 : yeni oyun\n\n" +
                "Her 5 puanda bir seviye atlanır ve borular hızlanır.",
                "Kontroller", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (oynuyordu) _motor.DuraklatDevamEt();
        }

        private void YeniOyunBaslat()
        {
            Text = "Flappy Bird — YMH219";
            _motor.YeniOyun();
            pbOyun.Invalidate();
        }

        // ---------------------------------------------------------------
        // Çizim
        // ---------------------------------------------------------------

        private void pbOyun_Paint(object sender, PaintEventArgs e)
        {
            if (_motor == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            ArkaPlaniCiz(g);

            // Çok biçimlilik: her nesne kendi Ciz() metodunu kullanır.
            foreach (OyunNesnesi boru in _motor.Borular)
            {
                boru.Ciz(g);
            }
            ZeminiCiz(g);
            _motor.Kus.Ciz(g);

            DurumYazisiniCiz(g);
        }

        private void ArkaPlaniCiz(Graphics g)
        {
            var alan = new Rectangle(0, 0, pbOyun.Width, pbOyun.Height);
            using var gok = new System.Drawing.Drawing2D.LinearGradientBrush(
                alan, Color.FromArgb(120, 200, 245), Color.FromArgb(200, 238, 255), 90f);
            g.FillRectangle(gok, alan);

            using var bulut = new SolidBrush(Color.FromArgb(235, 255, 255, 255));
            BulutCiz(g, bulut, 50, 90);
            BulutCiz(g, bulut, 290, 170);
            BulutCiz(g, bulut, 150, 300);
        }

        private static void BulutCiz(Graphics g, Brush firca, float x, float y)
        {
            g.FillEllipse(firca, x, y, 70, 34);
            g.FillEllipse(firca, x + 30, y - 16, 64, 44);
            g.FillEllipse(firca, x + 70, y, 60, 32);
        }

        private void ZeminiCiz(Graphics g)
        {
            float zeminY = _motor.ZeminY;
            using var toprak = new SolidBrush(Color.FromArgb(222, 200, 140));
            using var cim = new SolidBrush(Color.FromArgb(110, 190, 70));
            g.FillRectangle(toprak, 0, zeminY, pbOyun.Width, OyunMotoru.ZeminYuksekligi);
            g.FillRectangle(cim, 0, zeminY, pbOyun.Width, 14);
        }

        private void DurumYazisiniCiz(Graphics g)
        {
            switch (_motor.Durum)
            {
                case OyunDurumu.Oynaniyor:
                    OrtaliYaz(g, _motor.Skor.ToString(), _skorYazisi, 40);
                    break;

                case OyunDurumu.Hazir:
                    OrtaliYaz(g, "Hazır mısın?", _buyukYazi, 150);
                    OrtaliYaz(g, "Başlamak için BOŞLUK tuşuna bas", _kucukYazi, 210);
                    break;

                case OyunDurumu.Duraklatildi:
                    YariSaydamPerde(g);
                    OrtaliYaz(g, "DURAKLATILDI", _buyukYazi, 220);
                    OrtaliYaz(g, "Devam etmek için P", _kucukYazi, 280);
                    break;

                case OyunDurumu.Bitti:
                    YariSaydamPerde(g);
                    OrtaliYaz(g, "OYUN BİTTİ", _buyukYazi, 180);
                    OrtaliYaz(g, $"Skor: {_motor.Skor}    En yüksek: {_motor.EnYuksekSkor}", _kucukYazi, 245);
                    OrtaliYaz(g, "Yeniden başlamak için R / Enter", _kucukYazi, 285);
                    break;
            }
        }

        private void YariSaydamPerde(Graphics g)
        {
            using var perde = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
            g.FillRectangle(perde, 0, 0, pbOyun.Width, pbOyun.Height);
        }

        /// <summary>Metni yatayda ortalayıp gölgeli yazar.</summary>
        private void OrtaliYaz(Graphics g, string metin, Font yazi, float y)
        {
            SizeF boyut = g.MeasureString(metin, yazi);
            float x = (pbOyun.Width - boyut.Width) / 2;
            using var golge = new SolidBrush(Color.FromArgb(160, 0, 0, 0));
            g.DrawString(metin, yazi, golge, x + 2, y + 2);
            g.DrawString(metin, yazi, Brushes.White, x, y);
        }
    }
}
