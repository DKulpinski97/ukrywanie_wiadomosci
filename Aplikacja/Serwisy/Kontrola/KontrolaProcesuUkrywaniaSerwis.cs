using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Kontrola
{
    public class KontrolaProcesuUkrywaniaSerwis : IKontrolaProcesuUkrywania
    {
        private readonly IObslugaPlikow _obslugaPlikow;
        private readonly IZarzadzanieGui _zarzadzanieGui;
        public KontrolaProcesuUkrywaniaSerwis(IObslugaPlikow obslugaPlikow,IZarzadzanieGui zarzadzanieGui)
        {
            _obslugaPlikow = obslugaPlikow;
            _zarzadzanieGui = zarzadzanieGui;
        }
        public void KontrolaUkrywania()
        {
            Bitmap obraz = _obslugaPlikow.WczytajObrazekPNG();
            var bityDoZapisu = KonwertujWiadomoscNaBity();
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            string[] calaWiadomosc = ileBitowWBinarnym.Concat(bityDoZapisu).ToArray();
            UkryjWiadomoscStandard(obraz, calaWiadomosc);
        }
        private string[] KonwertujWiadomoscNaBity()
        {
            byte[] bajty = Encoding.UTF8.GetBytes(_zarzadzanieGui.PobierzWartoscWiadomoscTextBox());

            return bajty
                .Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
                .SelectMany(b => b.Select(bit => bit.ToString()))
                .ToArray();
        }
        private string[] ZamienLiczbeNaBity(int liczba)
        {
            return Convert.ToString(liczba, 2)
                .PadLeft(32, '0')
                .Select(bit => bit.ToString())
                .ToArray();
        }
        private Bitmap UkryjWiadomoscStandard(Bitmap obraz, string[] calaWiadomosc)
        {
            Bitmap wynik = new Bitmap(obraz);

            int indeks = 0;

            for (int y = 0; y < wynik.Height && indeks < calaWiadomosc.Length; y++)
            {
                for (int x = 0; x < wynik.Width && indeks < calaWiadomosc.Length; x++)
                {
                    Color pixel = wynik.GetPixel(x, y);
                    int tmp = (pixel.R & 0xFE) | int.Parse(calaWiadomosc[indeks++]);

                    wynik.SetPixel(x, y, Color.FromArgb(
                        pixel.A,
                        tmp,
                        pixel.G,
                        pixel.B
                    ));
                }
            }
            return wynik;
        }
    }
}
