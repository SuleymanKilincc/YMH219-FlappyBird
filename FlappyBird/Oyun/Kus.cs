using System.Drawing.Drawing2D;

namespace FlappyBird.Oyun
{
    /// <summary>
    /// Oyuncunun kontrol ettiği kuş. Dikey hızı (Hiz) her karede yer çekimi
    /// kadar artar; zıplayınca hız yukarı yönde (negatif) ayarlanır.
    /// </summary>
    public class Kus : OyunNesnesi
    {
        public const float Boyut = 34f;

        private const float YerCekimi = 0.5f;     // her karede hıza eklenen değer
        private const float ZiplamaHizi = -8f;    // zıplayınca verilen yukarı hız
        private const float MaksDusmeHizi = 10f;

        /// <summary>Dikey hız (piksel/kare). Negatif = yukarı, pozitif = aşağı.</summary>
        public float Hiz { get; private set; }

        public Kus(float x, float y) : base(x, y)
        {
        }

        /// <summary>Çarpışma kontrolü için kuşun kapladığı alan (kenarlardan biraz içeride, oyuncuya tolerans).</summary>
        public RectangleF Sinir => new RectangleF(X + 4, Y + 4, Boyut - 8, Boyut - 8);

        public void Ziplat()
        {
            Hiz = ZiplamaHizi;
        }

        /// <summary>Bir oyun karesi ilerlet: yer çekimini uygula ve konumu güncelle.</summary>
        public void Guncelle()
        {
            Hiz = Math.Min(Hiz + YerCekimi, MaksDusmeHizi);
            Y += Hiz;

            if (Y < 0)          // tavana çarpınca ölmez, sadece durur
            {
                Y = 0;
                Hiz = 0;
            }
        }

        public void Sifirla(float y)
        {
            Y = y;
            Hiz = 0;
        }

        public override void Ciz(Graphics g)
        {
            // Hıza göre kuşu hafifçe yukarı/aşağı eğ.
            float aci = Math.Clamp(Hiz * 4f, -25f, 70f);
            GraphicsState durum = g.Save();
            g.TranslateTransform(X + Boyut / 2, Y + Boyut / 2);
            g.RotateTransform(aci);

            float r = Boyut / 2;
            using var govde = new SolidBrush(Color.FromArgb(255, 205, 40));
            using var kanat = new SolidBrush(Color.FromArgb(240, 160, 20));
            using var gaga = new SolidBrush(Color.FromArgb(235, 90, 40));
            using var kenar = new Pen(Color.FromArgb(120, 70, 10), 2f);

            g.FillEllipse(govde, -r, -r, Boyut, Boyut);
            g.DrawEllipse(kenar, -r, -r, Boyut, Boyut);
            g.FillEllipse(kanat, -r + 3, -2, r, r * 0.7f);                    // kanat
            g.FillEllipse(Brushes.White, r * 0.15f, -r * 0.65f, r * 0.7f, r * 0.7f); // göz
            g.FillEllipse(Brushes.Black, r * 0.45f, -r * 0.45f, r * 0.3f, r * 0.3f);
            g.FillPolygon(gaga, new[]                                         // gaga
            {
                new PointF(r * 0.7f, -2), new PointF(r + 9, 3), new PointF(r * 0.7f, 8)
            });

            g.Restore(durum);
        }
    }
}
