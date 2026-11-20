using System.Drawing;
using System.IO;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Domena.Reguly;
using UkrywanieWiadomosci.Aplikacja.Model;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Weryfikatory
{
    public class WeryfikacjaDanychSerwisy : IWeryfikacjaDanych
    {
        public (bool Success, string Komunikat) SprawdzanieCzySciezkaNieJestPusta(DanePoczatkowe dane)
        {
            return SprawdzenieSciezkiPNG(dane.SciezkaDoPNG);
        }
        public (bool Success, string Komunikat) WeryfikacjaDanychPoczatkowych(Bitmap bitmap, DanePoczatkowe dane)
        {
            
            
            int szerokosc = bitmap.Width;
            int wysokosc = bitmap.Height;
            //Sprawdzanie czy obrazek ma przynajmniej 32 piksele szerokości 
            if (szerokosc < 32)
            {
                return (false, "Obrazek musi mieć szerokość przynajmniej 32 pikseli");
            }
            //Sprawdzanie warunków zapisu
            if (dane.Proces == CoRobic.Ukryj)
            {
                //Sprawdzanie czy podano miejsce zapisu
                var sprawdzenieMiejscaZapisu = SprawdzenieMiejscaZapisu(dane.SciezkaDoZapisu);
                if (!sprawdzenieMiejscaZapisu.Success)
                {
                    return sprawdzenieMiejscaZapisu;
                }
                //Sprawdzanie czy nie ma obrazu
                var sprawdzanieCzyNieMaObrazu = SprawdzCzyNieMaTakiegoObrazu(dane.SciezkaDoZapisu);
                if(!sprawdzanieCzyNieMaObrazu.Success)
                {
                    return sprawdzanieCzyNieMaObrazu;
                }
                //Sprawdzanie czy podano wiadomosc
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscPodana(dane.Wiadomosc);
                if (!sprawdzenieMiejscaWiadomosci.Success)
                {
                    return (false, sprawdzenieMiejscaWiadomosci.Komunikat);
                }
                //Sprawdzanie czy wiadomość się zmieści
                long ileBitowDoUkrycia = (long)Encoding.UTF8.GetByteCount(dane.Wiadomosc) * 8 + 32;
                if (dane.Algorytm == RodzajAlgorytmu.Standard)
                {
                    long liczbaPikseli = (long)szerokosc * wysokosc;
                    if (ileBitowDoUkrycia > liczbaPikseli)
                    {
                        return (false, "Obrazek jest za mały by ukryć wiadomość");
                    }
                }
                else
                {
                    long liczbaPikseli = (long)(szerokosc -32) * wysokosc;
                    if (ileBitowDoUkrycia > liczbaPikseli)
                    {
                        return (false, "Obrazek jest za mały by ukryć wiadomość");
                    }
                }
            }
            else
            {
                //sprawdzanie czy wiadomość jest pusta
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscNieJestPusta(dane.Wiadomosc);
                if (!sprawdzenieMiejscaWiadomosci.Success)
                {
                    return (false, sprawdzenieMiejscaWiadomosci.Komunikat);
                }
            }
            //Sprawdzanie rodzaju algorytmu
            if (dane.Algorytm == RodzajAlgorytmu.Losowy)
            {
                var sprawdzSeed = SprawdzCzySeedJestPusta(dane.Seed);
                if (!sprawdzSeed.Success)
                {
                    return sprawdzSeed;
                }
            }

            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzenieSciezkiPNG(string sciezka)
        {
            if (string.IsNullOrWhiteSpace(sciezka))
            {
                return (false, "Nie wybrano pliku PNG.");
            }
            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzenieMiejscaZapisu(string sciezka)
        {
            if (string.IsNullOrWhiteSpace(sciezka))
            {
                return (false, "Nie wybrano miejsca zapisu");
            }
            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzCzyNieMaTakiegoObrazu(string sciezka)
        {
            string nowaSciezka = Path.Combine(sciezka, NazwaDlaPliku.NazwaPliku);
            if(File.Exists(nowaSciezka))
            {
                return (false, "We wskazanym miejscu jest już obraz. Wybierz inne miejsce lub usuń stary.");
            }
            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzCzyWiadomoscPodana(string wiadomosc)
        {
            if (string.IsNullOrWhiteSpace(wiadomosc))
            {
                return (false, "Nie podano wiadomości do ukrycia");
            }
            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzCzyWiadomoscNieJestPusta(string wiadomosc)
        {
            if (!string.IsNullOrWhiteSpace(wiadomosc))
            {
                return (false, "Pole wiadomości musi być puste do odczytu");
            }
            return (true, "");
        }
        private (bool Success, string Komunikat) SprawdzCzySeedJestPusta(string seed)
        {
            if (string.IsNullOrWhiteSpace(seed))
            {
                return (false, "Nie podano seed");
            }
            return (true, "");
        }
    }
}
