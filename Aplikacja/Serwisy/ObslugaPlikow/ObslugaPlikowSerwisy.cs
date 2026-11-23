using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Domena.Reguly;
using UkrywanieWiadomosci.Exceptions;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow
{
    public class ObslugaPlikowSerwisy : IObslugaPlikow
    {
        public Bitmap WczytajObrazekPNG(string sciezka)
        {
            byte[] bytes;

            try
            {
                bytes = File.ReadAllBytes(sciezka);
            }
            catch (IOException ex)
            {
                throw new BladOperacjiException("Nie można odczytać pliku.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new BladOperacjiException("Brak uprawnień do odczytu pliku.",ex);
            }

            try
            {
                using var stream = new MemoryStream(bytes);
                using var temp = System.Drawing.Image.FromStream(stream, true, true);

                return KopiujBezKomponowania((Bitmap)temp);
            }
            catch (Exception ex)
            {
                throw new BladOperacjiException("Wystąpił błąd krytyczny. Wybrany plik prawdopodobnie nie jest prawidłowym obrazem PNG.", ex);
            }
        }

        // Kopia bajt po bajcie (LockBits). new Bitmap(Image) rysuje obraz przez Graphics.DrawImage
        // na przezroczystym tle, co niszczy RGB pikseli z alfa < 255, a razem z nim ukryte bity.
        private static Bitmap KopiujBezKomponowania(Bitmap zrodlo)
        {
            int szerokosc = zrodlo.Width;
            int wysokosc = zrodlo.Height;
            var kopia = new Bitmap(szerokosc, wysokosc, PixelFormat.Format32bppArgb);

            try
            {
                var prostokat = new Rectangle(0, 0, szerokosc, wysokosc);
                var daneZrodla = zrodlo.LockBits(prostokat, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try
                {
                    var daneKopii = kopia.LockBits(prostokat, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    try
                    {
                        int bajtyWiersza = szerokosc * 4;
                        var wiersz = new byte[bajtyWiersza];
                        for (int y = 0; y < wysokosc; y++)
                        {
                            Marshal.Copy(IntPtr.Add(daneZrodla.Scan0, y * daneZrodla.Stride), wiersz, 0, bajtyWiersza);
                            Marshal.Copy(wiersz, 0, IntPtr.Add(daneKopii.Scan0, y * daneKopii.Stride), bajtyWiersza);
                        }
                    }
                    finally
                    {
                        kopia.UnlockBits(daneKopii);
                    }
                }
                finally
                {
                    zrodlo.UnlockBits(daneZrodla);
                }
                return kopia;
            }
            catch
            {
                kopia.Dispose();
                throw;
            }
        }

        public void ZapisNowegoObrazka(Bitmap obraz,string sciezka)
        {
            try
            {
                string nowaSciezka = Path.Combine(sciezka, NazwaDlaPliku.NazwaPliku);
                obraz.Save(nowaSciezka, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new BladOperacjiException("Nie udało się zapisać pliku.",ex);
            }
        }
    }
}
