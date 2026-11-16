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
        public Bitmap KontrolaUkrywaniaStandard()
        {
            Bitmap obraz = _obslugaPlikow.WczytajObrazekPNG();
            var bityDoZapisu = KonwertujWiadomoscNaBity();
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            string[] calaWiadomosc = ileBitowWBinarnym.Concat(bityDoZapisu).ToArray();
            return  UkryjWiadomoscStandard(obraz, calaWiadomosc);
        }
        public string KontrolaOdczytuStandard()
        {
            Bitmap obraz = _obslugaPlikow.WczytajObrazekPNG();
            int ileBitowDoOdczytu = OdczytajIleZnakow(obraz);
            return OdczytajWiadomoscStandard(obraz, ileBitowDoOdczytu);
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
        private int OdczytajIleZnakow(Bitmap obraz)
        {
            int dlugosc = 0;
            int licznik = 0;

            for (int y = 0; y < obraz.Height && licznik < 32; y++)
            {
                for (int x = 0; x < obraz.Width && licznik < 32; x++)
                {
                    Color pixel = obraz.GetPixel(x, y);

                    dlugosc = (dlugosc << 1) | (pixel.R & 1);

                    licznik++;
                }
            }
            return dlugosc;
        }
        private string OdczytajWiadomoscStandard(Bitmap obraz, int ileBitow)
        {
            List<byte> bajty = new List<byte>();
            string aktualnyBajt = "";

            int indeks = 0;

            for (int y = 0; y < obraz.Height; y++)
            {
                for (int x = 0; x < obraz.Width; x++)
                {
                    Color pixel = obraz.GetPixel(x, y);

                    // Pomijamy pierwsze 32 bity (długość wiadomości)
                    if (indeks >= 32 && indeks < 32 + ileBitow)
                    {
                        int bit = pixel.R & 1;

                        aktualnyBajt += bit.ToString();

                        if (aktualnyBajt.Length == 8)
                        {
                            bajty.Add(Convert.ToByte(aktualnyBajt, 2));
                            aktualnyBajt = "";
                        }
                    }

                    indeks++;

                    if (indeks >= 32 + ileBitow)
                        return Encoding.UTF8.GetString(bajty.ToArray());
                }
            }

            return Encoding.UTF8.GetString(bajty.ToArray());
        }
    }
}
