namespace FlappyBird.Oyun
{
    public enum OyunDurumu
    {
        Hazir,       // oyun başlamadı, ilk tuşa basılmasını bekliyor
        Oynaniyor,
        Duraklatildi,
        Bitti
    }

    /// <summary>
    /// Oyunun tüm kuralları ve durumu burada: kuş, borular, skor, hız, çarpışma.
    /// Ekranla ilgisi yoktur; form her Timer tikinde Guncelle() çağırır,
    /// sonra Ciz() ile her şeyi ekrana çizdirir.
    /// </summary>
    public class OyunMotoru
    {
        public const int BoruSayisi = 3;
        public const float BorularArasiMesafe = 230f;
        public const float ZeminYuksekligi = 70f;

        private const float BaslangicHizi = 3f;
        private const float HizArtisi = 0.5f;
        private const float MaksHiz = 7f;
        private const int SeviyeBasinaSkor = 5;

        private readonly Random _rastgele = new();
        private readonly List<BoruCifti> _borular = new();

        public float Genislik { get; }
        public float Yukseklik { get; }
        public float ZeminY => Yukseklik - ZeminYuksekligi;

        public Kus Kus { get; }
        public IReadOnlyList<BoruCifti> Borular => _borular;

        public OyunDurumu Durum { get; private set; } = OyunDurumu.Hazir;
        public int Skor { get; private set; }
        public int EnYuksekSkor { get; private set; }

        /// <summary>Boruların kare başına sola kayma miktarı; seviye arttıkça büyür.</summary>
        public float Hiz { get; private set; } = BaslangicHizi;

        public int Seviye => Skor / SeviyeBasinaSkor + 1;

        /// <summary>Skor değişince formun etiketleri güncellemesi için.</summary>
        public event EventHandler? SkorDegisti;

        /// <summary>Oyun bittiğinde (çarpışma) bir kez tetiklenir.</summary>
        public event EventHandler? OyunBitti;

        public OyunMotoru(float genislik, float yukseklik, int enYuksekSkor = 0)
        {
            Genislik = genislik;
            Yukseklik = yukseklik;
            EnYuksekSkor = enYuksekSkor;
            Kus = new Kus(genislik * 0.25f, yukseklik * 0.4f);
            YeniOyun();
        }

        /// <summary>Her şeyi başlangıç durumuna getirir.</summary>
        public void YeniOyun()
        {
            Skor = 0;
            Hiz = BaslangicHizi;
            Kus.Sifirla(Yukseklik * 0.4f);

            _borular.Clear();
            for (int i = 0; i < BoruSayisi; i++)
            {
                float x = Genislik + 120 + i * BorularArasiMesafe;
                _borular.Add(new BoruCifti(x, RastgeleBoslukY(), ZeminY));
            }

            Durum = OyunDurumu.Hazir;
            SkorDegisti?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Oyuncu zıplama tuşuna bastı.</summary>
        public void Ziplat()
        {
            if (Durum == OyunDurumu.Hazir)
            {
                Durum = OyunDurumu.Oynaniyor;
            }
            if (Durum == OyunDurumu.Oynaniyor)
            {
                Kus.Ziplat();
            }
        }

        public void DuraklatDevamEt()
        {
            if (Durum == OyunDurumu.Oynaniyor) Durum = OyunDurumu.Duraklatildi;
            else if (Durum == OyunDurumu.Duraklatildi) Durum = OyunDurumu.Oynaniyor;
        }

        /// <summary>Oyun döngüsünün bir adımı: hareket, skor, çarpışma.</summary>
        public void Guncelle()
        {
            if (Durum != OyunDurumu.Oynaniyor)
            {
                return;
            }

            Kus.Guncelle();

            foreach (BoruCifti boru in _borular)
            {
                boru.Hareket(Hiz);

                // Kuş borunun sağ kenarını geçtiyse skor +1 (her boru için bir kez).
                if (!boru.Gecildi && boru.X + BoruCifti.Genislik < Kus.X)
                {
                    boru.Gecildi = true;
                    SkorArtir();
                }

                if (boru.EkranDisinda)
                {
                    // En sağdaki borunun arkasına taşı: borular sonsuz döngüde akar.
                    float enSagX = _borular.Max(b => b.X);
                    boru.YenidenKonumlandir(enSagX + BorularArasiMesafe, RastgeleBoslukY());
                }
            }

            if (CarpismaVar())
            {
                OyunuBitir();
            }
        }

        private bool CarpismaVar()
        {
            RectangleF kusAlani = Kus.Sinir;
            if (kusAlani.Bottom >= ZeminY)
            {
                return true;
            }
            return _borular.Any(b => b.Carpisiyor(kusAlani));
        }

        private void SkorArtir()
        {
            Skor++;
            if (Skor % SeviyeBasinaSkor == 0)
            {
                Hiz = Math.Min(Hiz + HizArtisi, MaksHiz);   // her 5 puanda biraz hızlan
            }
            SkorDegisti?.Invoke(this, EventArgs.Empty);
        }

        private void OyunuBitir()
        {
            Durum = OyunDurumu.Bitti;
            if (Skor > EnYuksekSkor)
            {
                EnYuksekSkor = Skor;
            }
            OyunBitti?.Invoke(this, EventArgs.Empty);
        }

        private float RastgeleBoslukY()
        {
            const float kenarPayi = 60f;
            float enAlt = ZeminY - BoruCifti.BoslukYuksekligi - kenarPayi;
            return kenarPayi + (float)_rastgele.NextDouble() * (enAlt - kenarPayi);
        }
    }
}
