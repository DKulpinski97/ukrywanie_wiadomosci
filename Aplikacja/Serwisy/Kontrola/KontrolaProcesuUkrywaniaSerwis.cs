using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Kontrola
{
    public class KontrolaProcesuUkrywaniaSerwis : IKontrolaProcesuUkrywania
    {
        private readonly IZarzadzanieGui _zarzadzanieGui;
        public KontrolaProcesuUkrywaniaSerwis(IZarzadzanieGui zarzadzanieGui)
        {
            _zarzadzanieGui = zarzadzanieGui;
        }
        public void KontrolaUkrywaniaStandard(Bitmap obraz)
        {
            var bityDoZapisu = KonwertujWiadomoscNaBity();
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            string[] calaWiadomosc = ileBitowWBinarnym.Concat(bityDoZapisu).ToArray();
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Ukrywam wiadomość");
            UkryjWiadomoscStandard(obraz, calaWiadomosc);
        }
        public (bool Succes, string komunikat) KontrolaOdczytuStandard(Bitmap obraz)
        {
            int ileBitowDoOdczytu = OdczytajIleZnakow(obraz);
            if (ileBitowDoOdczytu < 0 || ileBitowDoOdczytu > (obraz.Width * obraz.Height) - 32)
            {
                return (false, "Obrazek ma źle zaszyfrowaną ilość bitów do odczytu");
            }
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Odczytuje wiadomość");
            return (true, OdczytajWiadomoscStandard(obraz, ileBitowDoOdczytu));
        }
        public void KontrolaUkrywaniaLosowa(Bitmap obraz)
        {
            var bityDoZapisu = KonwertujWiadomoscNaBity();
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            var seed = _zarzadzanieGui.PobierzWartoscSeedTextBox();
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Generuje seed");
            var WynikGeneratora = Generate(bityDoZapisu.Length, SeedDoInt(seed), obraz);
            UkryjIleZnakow(obraz, ileBitowWBinarnym);
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Ukrywam wiadomość");
            UkryjLosowo(obraz, WynikGeneratora, bityDoZapisu);
        }
        public (bool Succes, string komunikat) KontrolaOdczytuLosowo(Bitmap obraz)
        {
            int ileBitowDoOdczytu = OdczytajIleZnakow(obraz);
            if (ileBitowDoOdczytu < 0 || ileBitowDoOdczytu > (obraz.Width - 32) * obraz.Height)
            {
                return (false, "Obrazek ma źle zaszyfrowaną ilość bitów do odczytu");
            }
            var seed = _zarzadzanieGui.PobierzWartoscSeedTextBox();
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Generuje seed");
            var WynikGeneratora = Generate(ileBitowDoOdczytu, SeedDoInt(seed), obraz);
            _zarzadzanieGui.PrzypiszWartoscStatusLabel("Odczytuje wiadomość");
            return (true, OdczytajLosowo(obraz, WynikGeneratora));
        }

        private int SeedDoInt(string seed)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(seed));
            return BitConverter.ToInt32(hash, 0);
        }
        public static ((int x, int y)[] positions, char[] colors) Generate(int ileZnakow, int seed, Bitmap obraz)
        {
            Random random = new Random(seed);

            var positions = new (int x, int y)[ileZnakow];
            var colors = new char[ileZnakow];

            char[] options = { 'r', 'g', 'b' };

            var used = new HashSet<(int x, int y)>();


            int maxWith = obraz.Width;
            int maxHeight = obraz.Height;
            for (int i = 0; i < ileZnakow; i++)
            {
                int x, y;

                do
                {
                    x = random.Next(32, maxWith);
                    y = random.Next(0, maxHeight);
                }
                while (!used.Add((x, y)));

                positions[i] = (x, y);
                colors[i] = options[random.Next(3)];
            }

            return (positions, colors);
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
        private void UkryjWiadomoscStandard(Bitmap obraz, string[] calaWiadomosc)
        {
            int indeks = 0;

            for (int y = 0; y < obraz.Height && indeks < calaWiadomosc.Length; y++)
            {
                for (int x = 0; x < obraz.Width && indeks < calaWiadomosc.Length; x++)
                {
                    Color pixel = obraz.GetPixel(x, y);
                    int tmp = (pixel.R & 0xFE) | int.Parse(calaWiadomosc[indeks++]);
                    _zarzadzanieGui.PrzypiszWartoscKtoryZnakLabel($"{indeks.ToString()}/{calaWiadomosc.Length.ToString()}");

                    obraz.SetPixel(x, y, Color.FromArgb(
                        pixel.A,
                        tmp,
                        pixel.G,
                        pixel.B
                    ));
                }
            }
        }
        private int OdczytajIleZnakow(Bitmap obraz)
        {
            int dlugosc = 0;

            for (int x = 0; x < 32; x++)
            {
                Color pixel = obraz.GetPixel(x, 0);

                dlugosc = (dlugosc << 1) | (pixel.R & 1);
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
                    _zarzadzanieGui.PrzypiszWartoscKtoryZnakLabel($"{indeks.ToString()}/{ileBitow.ToString()}");

                    if (indeks >= 32 + ileBitow)
                        return Encoding.UTF8.GetString(bajty.ToArray());
                }
            }

            return Encoding.UTF8.GetString(bajty.ToArray());
        }
        private void UkryjIleZnakow(Bitmap obraz, string[] IleZnakowBit)
        {
            for (int x = 0; x < 32; x++)
            {
                Color pixel = obraz.GetPixel(x, 0);
                int tmp = (pixel.R & 0xFE) | int.Parse(IleZnakowBit[x]);

                obraz.SetPixel(x, 0, Color.FromArgb(
                    pixel.A,
                    tmp,
                    pixel.G,
                    pixel.B
                ));
            }
        }
        private void UkryjLosowo(Bitmap obraz, ((int x, int y)[] positions, char[] colors) kontrolka, string[] wiadomosc)
        {

            for (int i = 0; i < kontrolka.colors.Length; i++)
            {
                var pozycja = kontrolka.positions[i];

                Color pixel = obraz.GetPixel(pozycja.x, pozycja.y);

                int bit = int.Parse(wiadomosc[i]);

                int r = pixel.R;
                int g = pixel.G;
                int b = pixel.B;

                switch (kontrolka.colors[i])
                {
                    case 'r':
                        r = (r & 0xFE) | bit;
                        break;

                    case 'g':
                        g = (g & 0xFE) | bit;
                        break;

                    case 'b':
                        b = (b & 0xFE) | bit;
                        break;
                }
                _zarzadzanieGui.PrzypiszWartoscKtoryZnakLabel($"{(i + 1).ToString()}/{kontrolka.colors.Length}");
                obraz.SetPixel(pozycja.x, pozycja.y, Color.FromArgb(pixel.A, r, g, b)
                );
            }
        }
        private string OdczytajLosowo(Bitmap obraz, ((int x, int y)[] positions, char[] colors) kontrolka)
        {
            List<byte> bajty = new List<byte>();
            string aktualnyBajt = "";

            for (int i = 0; i < kontrolka.colors.Length; i++)
            {
                var pozycja = kontrolka.positions[i];

                Color pixel = obraz.GetPixel(pozycja.x, pozycja.y);

                int bit = 0;

                switch (kontrolka.colors[i])
                {
                    case 'r':
                        bit = pixel.R & 1;
                        break;

                    case 'g':
                        bit = pixel.G & 1;
                        break;

                    case 'b':
                        bit = pixel.B & 1;
                        break;
                }
                _zarzadzanieGui.PrzypiszWartoscKtoryZnakLabel($"{(i + 1).ToString()}/{kontrolka.colors.Length}");
                aktualnyBajt += bit.ToString();

                if (aktualnyBajt.Length == 8)
                {
                    bajty.Add(Convert.ToByte(aktualnyBajt, 2));
                    aktualnyBajt = "";
                }
            }
            return Encoding.UTF8.GetString(bajty.ToArray());
        }
    }
}
