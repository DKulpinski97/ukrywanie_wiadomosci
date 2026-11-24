using Moq;
using System.Drawing;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Aplikacja.Model;
using UkrywanieWiadomosci.Aplikacja.Serwisy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Serwisy.Weryfikatory;

namespace UkrywanieWiadomosci.Testy.Aplikacja.Serwisy.Kontrola
{
    public class KontrolaUkrywania
    {
        [Fact]
        public void KontrolaUkrywaniaStandard_PelnyProces_ZapisOdczytPoprawnejWiadomosci()
        {
            // Arrange
            string katalogTymczasowy = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(katalogTymczasowy);

            string sciezkaPNG = Path.Combine(katalogTymczasowy, "Wejscie.png");

            string wiadomosc = "Ala ma kota";

            var raportowanie = new DaneRaportowaniaPostepu(Mock.Of<IProgress<string>>(), Mock.Of<IProgress<string>>());

            var dane = new DanePoczatkowe(sciezkaPNG, katalogTymczasowy,
                wiadomosc, "", RodzajAlgorytmu.Standard, CoRobic.Ukryj);

            var kontrolaProcesu = new KontrolaProcesuUkrywaniaSerwis();
            var obslugaPlikow = new ObslugaPlikowSerwisy();

            try
            {
                using var bitmapa = new Bitmap(64, 8);

                // Wypełnienie bitmapy deterministycznym wzorem
                for (int y = 0; y < bitmapa.Height; y++)
                {
                    for (int x = 0; x < bitmapa.Width; x++)
                    {
                        bitmapa.SetPixel(x, y,
                            Color.FromArgb(
                                255,
                                (x * 7) % 256,
                                (y * 13) % 256,
                                100));
                    }
                }

                // Ustawienie przezroczystości w pierwszych 16 pikselach
                for (int x = 0; x < 16; x++)
                {
                    Color kolor = bitmapa.GetPixel(x, 0);

                    bitmapa.SetPixel(
                        x,
                        0,
                        Color.FromArgb(
                            x < 8 ? 0 : 128,
                            kolor.R,
                            kolor.G,
                            kolor.B));
                }

                // Act - ukrywanie wiadomości
                kontrolaProcesu.KontrolaUkrywaniaStandard(bitmapa, dane, raportowanie);

                // Zapis do PNG
                obslugaPlikow.ZapisNowegoObrazka(bitmapa, katalogTymczasowy);

                string sciezkaZapisanegoPNG = Path.Combine(katalogTymczasowy, "NowyObraz.png");

                // Wczytanie zapisanej bitmapy
                using Bitmap wczytanaBitmapa =
                    obslugaPlikow.WczytajObrazekPNG(sciezkaZapisanegoPNG);

                // Odczyt wiadomości
                var wynik = kontrolaProcesu.KontrolaOdczytuStandard(wczytanaBitmapa, raportowanie);

                // Assert
                Assert.True(wynik.Success, wynik.Komunikat);
                Assert.Equal(wiadomosc, wynik.Komunikat);
            }
            finally
            {
                if (Directory.Exists(katalogTymczasowy))
                {
                    Directory.Delete(katalogTymczasowy, true);
                }
            }
        }
        [Theory]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void WeryfikacjaDanychPoczatkowych_Losowy_Pojemnosc(int liczbaBajtow,bool oczekiwanyWynik)
        {
            // Arrange
            using var bitmap = new Bitmap(48, 4);

            var dane = new DanePoczatkowe(
                SciezkaDoPNG: "bitmapa_48x4.png",
                SciezkaDoZapisu: Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString()),
                Wiadomosc: new string('a', liczbaBajtow),
                Seed: "cos",
                Algorytm: RodzajAlgorytmu.Losowy,
                Proces: CoRobic.Ukryj);

            WeryfikacjaDanychSerwisy weryfikacjaDanych =new WeryfikacjaDanychSerwisy();

            // Act
            var wynik = weryfikacjaDanych.WeryfikacjaDanychPoczatkowych(bitmap, dane);

            // Assert
            Assert.Equal(oczekiwanyWynik, wynik.Success);

            if (!oczekiwanyWynik)
            {
                Assert.Equal(
                    "Obrazek jest za mały by ukryć wiadomość",
                    wynik.Komunikat);
            }
        }

    }
}
