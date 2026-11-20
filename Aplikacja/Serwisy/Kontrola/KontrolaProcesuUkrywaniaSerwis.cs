using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Model;


namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Kontrola
{
    public class KontrolaProcesuUkrywaniaSerwis : IKontrolaProcesuUkrywania
    {

        public void KontrolaUkrywaniaStandard(Bitmap obraz, DanePoczatkowe dane, DaneRaportowaniaPostepu raportery)
        {
            var bityDoZapisu = KonwertujWiadomoscNaBity(dane.Wiadomosc);
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            string[] calaWiadomosc = ileBitowWBinarnym.Concat(bityDoZapisu).ToArray();
            raportery.Status.Report("Ukrywam wiadomość");
            UkryjWiadomoscStandard(obraz, calaWiadomosc, raportery);
        }
        public (bool Success, string Komunikat) KontrolaOdczytuStandard(Bitmap obraz, DaneRaportowaniaPostepu raportery)
        {
            int ileBitowDoOdczytu = OdczytajIleZnakow(obraz);
            if (ileBitowDoOdczytu < 0 || ileBitowDoOdczytu > (obraz.Width * obraz.Height) - 32)
            {
                return (false, "Obrazek ma źle zakodowaną ilość bitów do odczytu");
            }
            raportery.Status.Report("Odczytuję wiadomość");
            return (true, OdczytajWiadomoscStandard(obraz, ileBitowDoOdczytu, raportery));
        }
        public void KontrolaUkrywaniaLosowa(Bitmap obraz, DanePoczatkowe dane, DaneRaportowaniaPostepu raportery)
        {
            var bityDoZapisu = KonwertujWiadomoscNaBity(dane.Wiadomosc);
            var ileBitowWBinarnym = ZamienLiczbeNaBity(bityDoZapisu.Length);
            raportery.Status.Report("Generuję seed");
            var pozycjeIKolory = Generate(bityDoZapisu.Length, SeedDoInt(dane.Seed), obraz);
            UkryjIleZnakow(obraz, ileBitowWBinarnym);
            raportery.Status.Report("Ukrywam wiadomość");
            UkryjLosowo(obraz, pozycjeIKolory, bityDoZapisu, raportery);
        }
        public (bool Success, string Komunikat) KontrolaOdczytuLosowo(Bitmap obraz, DanePoczatkowe dane, DaneRaportowaniaPostepu raportery)
        {
            int ileBitowDoOdczytu = OdczytajIleZnakow(obraz);
            if (ileBitowDoOdczytu < 0 || ileBitowDoOdczytu > (obraz.Width - 32) * obraz.Height)
            {
                return (false, "Obrazek ma źle zakodowaną ilość bitów do odczytu");
            }
            raportery.Status.Report("Generuję seed");
            var pozycjeIKolory = Generate(ileBitowDoOdczytu, SeedDoInt(dane.Seed), obraz);
            raportery.Status.Report("Odczytuję wiadomość");
            return (true, OdczytajLosowo(obraz, pozycjeIKolory, raportery));
        }

        private int SeedDoInt(string seed)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(seed));
            return BitConverter.ToInt32(hash, 0);
        }
        private static ((int x, int y)[] positions, char[] colors) Generate(int ileZnakow, int seed, Bitmap obraz)
        {
            Random random = new Random(seed);

            var positions = new (int x, int y)[ileZnakow];
            var colors = new char[ileZnakow];

            char[] options = { 'r', 'g', 'b' };

            var used = new HashSet<(int x, int y)>();


            int maxWidth = obraz.Width;
            int maxHeight = obraz.Height;
            for (int i = 0; i < ileZnakow; i++)
            {
                int x, y;

                do
                {
                    x = random.Next(32, maxWidth);
                    y = random.Next(0, maxHeight);
                }
                while (!used.Add((x, y)));

                positions[i] = (x, y);
                colors[i] = options[random.Next(3)];
            }

            return (positions, colors);
        }
        private string[] KonwertujWiadomoscNaBity(string wiadomosc)
        {
            byte[] bajty = Encoding.UTF8.GetBytes(wiadomosc);

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
        private void UkryjWiadomoscStandard(Bitmap obraz, string[] calaWiadomosc, DaneRaportowaniaPostepu raportery)
        {
            int indeks = 0;
            int max = calaWiadomosc.Length;
            int krok = Math.Max(1, max / 100);
            int oczekiwanyIndeks = krok;

            for (int y = 0; y < obraz.Height && indeks < calaWiadomosc.Length; y++)
            {
                for (int x = 0; x < obraz.Width && indeks < calaWiadomosc.Length; x++)
                {
                    Color pixel = obraz.GetPixel(x, y);
                    int zmodyfikowanaWartoscRed = (pixel.R & 0xFE) | int.Parse(calaWiadomosc[indeks++]);


                    if (indeks >= oczekiwanyIndeks || indeks == max)
                    {
                        oczekiwanyIndeks += krok;
                        raportery.KtoryBit.Report($"{indeks}/{max}");
                    }

                    obraz.SetPixel(x, y, Color.FromArgb(
                        pixel.A,
                        zmodyfikowanaWartoscRed,
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
        private string OdczytajWiadomoscStandard(Bitmap obraz, int ileBitow, DaneRaportowaniaPostepu raportery)
        {
            List<byte> bajty = new List<byte>();
            string aktualnyBajt = "";

            int indeks = 0;
            int max = 32 + ileBitow;
            int krok = Math.Max(1, max / 100);
            int oczekiwanyIndeks = krok;
            
            for (int y = 0; y < obraz.Height; y++)
            {
                for (int x = 0; x < obraz.Width; x++)
                {
                    Color pixel = obraz.GetPixel(x, y);

                    // Pomijamy pierwsze 32 bity (długość wiadomości)
                    if (indeks >= 32 && indeks < max)
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

                    if (indeks >= oczekiwanyIndeks || indeks == max)
                    {
                        oczekiwanyIndeks += krok;
                        raportery.KtoryBit.Report($"{indeks}/{max}");
                    }

                    if (indeks >= max)
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
                int zmodyfikowanaWartoscRed = (pixel.R & 0xFE) | int.Parse(IleZnakowBit[x]);

                obraz.SetPixel(x, 0, Color.FromArgb(
                    pixel.A,
                    zmodyfikowanaWartoscRed,
                    pixel.G,
                    pixel.B
                ));
            }
        }
        private void UkryjLosowo(Bitmap obraz, ((int x, int y)[] positions, char[] colors) kontrolka, string[] wiadomosc, DaneRaportowaniaPostepu raportery)
        {
            int max = wiadomosc.Length;
            int krok = Math.Max(1, max / 100);
            int oczekiwanyIndeks = krok;

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
                if (i + 1 >= oczekiwanyIndeks || i + 1 == max)
                {
                    oczekiwanyIndeks += krok;
                    raportery.KtoryBit.Report($"{i + 1}/{max}");
                }

                obraz.SetPixel(pozycja.x, pozycja.y, Color.FromArgb(pixel.A, r, g, b)
                );
            }
        }
        private string OdczytajLosowo(Bitmap obraz, ((int x, int y)[] positions, char[] colors) kontrolka, DaneRaportowaniaPostepu raportery)
        {

            List<byte> bajty = new List<byte>();
            string aktualnyBajt = "";

            int max = kontrolka.colors.Length;
            int krok = Math.Max(1, max / 100);
            int oczekiwanyIndeks = krok;
           
            for (int i = 0; i < max; i++)
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
                if (i + 1 >= oczekiwanyIndeks || i + 1 == max)
                {
                    oczekiwanyIndeks += krok;
                    raportery.KtoryBit.Report($"{i + 1}/{max}");
                }

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
