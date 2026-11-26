namespace FlappyBird.Oyun
{
    /// <summary>
    /// Üstte ve altta birer boru, aralarında kuşun geçeceği bir boşluk.
    /// X konumu borunun sol kenarı, BoslukY boşluğun üst kenarıdır.
    /// </summary>
    public class BoruCifti : OyunNesnesi
    {
        public const float Genislik = 70f;
        public const float BoslukYuksekligi = 165f;

        private readonly float _zeminY;   // alt borunun bittiği yer (zeminin üstü)

        /// <summary>Kuş bu borunun sağ kenarını geçince true olur; skor bir kez sayılsın diye.</summary>
        public bool Gecildi { get; set; }

        public float BoslukY => Y;

        public BoruCifti(float x, float boslukY, float zeminY) : base(x, boslukY)
        {
            _zeminY = zeminY;
        }

        public RectangleF UstBoru => new RectangleF(X, 0, Genislik, BoslukY);
        public RectangleF AltBoru => new RectangleF(X, BoslukY + BoslukYuksekligi, Genislik,
                                                    _zeminY - (BoslukY + BoslukYuksekligi));

        public bool EkranDisinda => X + Genislik < 0;

        /// <summary>Boruyu sola kaydırır.</summary>
        public void Hareket(float hiz)
        {
            X -= hiz;
        }

        /// <summary>Ekrandan çıkan boruyu sağa geri alır ve yeni rastgele bir boşluk verir.</summary>
        public void YenidenKonumlandir(float yeniX, float yeniBoslukY)
        {
            X = yeniX;
            Y = yeniBoslukY;
            Gecildi = false;
        }

        public bool Carpisiyor(RectangleF alan)
        {
            return alan.IntersectsWith(UstBoru) || alan.IntersectsWith(AltBoru);
        }

        public override void Ciz(Graphics g)
        {
            using var dolgu = new SolidBrush(Color.FromArgb(95, 180, 60));
            using var kenar = new Pen(Color.FromArgb(40, 90, 25), 3f);

            BoruCiz(g, UstBoru, dolgu, kenar, agizAltta: true);
            BoruCiz(g, AltBoru, dolgu, kenar, agizAltta: false);
        }

        private static void BoruCiz(Graphics g, RectangleF r, Brush dolgu, Pen kenar, bool agizAltta)
        {
            if (r.Height <= 0) return;
            g.FillRectangle(dolgu, r);
            g.DrawRectangle(kenar, r.X, r.Y, r.Width, r.Height);

            // Borunun ağız kısmı: biraz daha geniş bir kapak.
            const float agizYuksekligi = 24f;
            float agizY = agizAltta ? r.Bottom - agizYuksekligi : r.Y;
            var agiz = new RectangleF(r.X - 5, agizY, r.Width + 10, agizYuksekligi);
            g.FillRectangle(dolgu, agiz);
            g.DrawRectangle(kenar, agiz.X, agiz.Y, agiz.Width, agiz.Height);
        }
    }
}
