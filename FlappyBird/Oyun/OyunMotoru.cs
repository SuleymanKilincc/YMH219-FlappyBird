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
            float? oncekiBoslukY = null;
            for (int i = 0; i < BoruSayisi; i++)
            {
                float x = Genislik + 120 + i * BorularArasiMesafe;
                float boslukY = RastgeleBoslukY(oncekiBoslukY);
                _borular.Add(new BoruCifti(x, boslukY, ZeminY));
                oncekiBoslukY = boslukY;
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

        /// <summary>Açıkken kuş kendi kendine uçar (demo / test için).</summary>
        public bool OtomatikPilot { get; set; }

        /// <summary>Oyun döngüsünün bir adımı: hareket, skor, çarpışma.</summary>
        public void Guncelle()
        {
            if (OtomatikPilot && Durum == OyunDurumu.Hazir)
            {
                Durum = OyunDurumu.Oynaniyor;
            }
            if (Durum != OyunDurumu.Oynaniyor)
            {
                return;
            }

            if (OtomatikPilot)
            {
                OtomatikPilotKarari();
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
                    BoruCifti enSagdaki = _borular.MaxBy(b => b.X)!;
                    boru.YenidenKonumlandir(enSagdaki.X + BorularArasiMesafe,
                                            RastgeleBoslukY(enSagdaki.BoslukY));
                }
            }

            if (CarpismaVar())
            {
                OyunuBitir();
            }
        }

        /// <summary>
        /// Basit otomatik pilot: önündeki ilk borunun boşluğunun alt kısmına göre karar verir.
        /// Kuş hedefin altına düşmeye başlamışsa zıplatır.
        /// </summary>
        private void OtomatikPilotKarari()
        {
            BoruCifti? siradaki = _borular
                .Where(b => b.X + BoruCifti.Genislik >= Kus.X)
                .OrderBy(b => b.X)
                .FirstOrDefault();

            float hedefY = siradaki != null
                ? siradaki.BoslukY + BoruCifti.BoslukYuksekligi - Kus.Boyut - 30
                : Yukseklik * 0.4f;

            if (Kus.Y > hedefY && Kus.Hiz >= -2)
            {
                Kus.Ziplat();
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

        /// <summary>
        /// Yeni boşluk için rastgele bir yükseklik seçer. Bir önceki boşluk verilirse yeni boşluk
        /// ondan en fazla MaksBoslukFarki kadar uzak olur; böylece yetişilemeyecek bir boru çıkmaz.
        /// </summary>
        private float RastgeleBoslukY(float? oncekiBoslukY = null)
        {
            const float kenarPayi = 60f;
            const float MaksBoslukFarki = 140f;

            float enUst = kenarPayi;
            float enAlt = ZeminY - BoruCifti.BoslukYuksekligi - kenarPayi;
            if (oncekiBoslukY.HasValue)
            {
                enUst = Math.Max(enUst, oncekiBoslukY.Value - MaksBoslukFarki);
                enAlt = Math.Min(enAlt, oncekiBoslukY.Value + MaksBoslukFarki);
            }
            return enUst + (float)_rastgele.NextDouble() * (enAlt - enUst);
        }
    }
}
