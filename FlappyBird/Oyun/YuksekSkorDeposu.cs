namespace FlappyBird.Oyun
{
    /// <summary>
    /// En yüksek skoru bir metin dosyasında saklar; oyun kapanıp açılsa da rekor korunur.
    /// Dosya: %AppData%\YMH219-FlappyBird\enyuksekskor.txt
    /// </summary>
    public class YuksekSkorDeposu
    {
        public string DosyaYolu { get; }

        public YuksekSkorDeposu()
        {
            string klasor = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "YMH219-FlappyBird");
            DosyaYolu = Path.Combine(klasor, "enyuksekskor.txt");
        }

        /// <summary>Kayıtlı rekoru okur. Dosya yoksa veya okunamazsa 0 döner (oyun yine açılır).</summary>
        public int Oku()
        {
            try
            {
                if (!File.Exists(DosyaYolu))
                {
                    return 0;
                }
                string metin = File.ReadAllText(DosyaYolu).Trim();
                return int.TryParse(metin, out int skor) && skor > 0 ? skor : 0;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return 0;
            }
        }

        /// <summary>Rekoru dosyaya yazar.</summary>
        /// <returns>Yazma başarılıysa true.</returns>
        public bool Kaydet(int skor)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DosyaYolu)!);
                File.WriteAllText(DosyaYolu, skor.ToString());
                return true;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
