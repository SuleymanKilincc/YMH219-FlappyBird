namespace FlappyBird.Oyun
{
    /// <summary>
    /// Ekranda çizilen her oyun nesnesinin ortak atası.
    /// Kuş ve borular bu sınıftan türer (kalıtım) ve kendilerini
    /// nasıl çizeceklerini kendileri bilir (çok biçimlilik: abstract Ciz).
    /// </summary>
    public abstract class OyunNesnesi
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }

        protected OyunNesnesi(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Her alt sınıf kendini ekrana farklı çizer.</summary>
        public abstract void Ciz(Graphics g);
    }
}
