using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Weryfikatory
{
    public class WeryfikacjaDanychSerwiy : IWeryfikacjaDanych
    {
        private readonly IZarzadzanieGui _zarzadzanieGui;
        private readonly IObslugaPlikow _obslugaPlikow;
        public WeryfikacjaDanychSerwiy(IZarzadzanieGui zarzadzanieGui, IObslugaPlikow obslugaPlikow)
        {
            _zarzadzanieGui = zarzadzanieGui;
            _obslugaPlikow = obslugaPlikow;
        }

        public (bool Succes, string komunikat) WeryfikacjaDanychPoczatkowych()
        {
            var sprawdzenieMiejsceObrazu = SprawdzenieSciezkiPNG();
            Bitmap bitmap;
            if (sprawdzenieMiejsceObrazu.Succes == false)
            {
                return (false, "Nie wybrano pliku PNG.");
            }
            try
            {
                bitmap = _obslugaPlikow.WczytajObrazekPNG();
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            int szerokosc = bitmap.Width;
            int wysokosc = bitmap.Height;
            //Sprawdzanie czy obrazek ma przynajmniej 32 piksele szerokości 
            if (szerokosc < 32)
            {
                return (false, "Obrazek musi mieć szerokość przynajmniej 32 pixeli");
            }
            //Sprawdzanie warunków zapisu
            if (_zarzadzanieGui.PobierzCzyUkrycRadioBox() == true)
            {
                //Sprawdzanie czy podano miejsce zapisu
                var sprawdzenieMiejscaZapisu = SprawdzenieMiejscaZapisu();
                if (sprawdzenieMiejscaZapisu.Succes == false)
                {
                    return sprawdzenieMiejscaZapisu;
                }
                //Sprawdzanie czy nie ma obrazu
                var sprawdzanieCzyNieMaObrazu = SprawdzCzyNieMaTakiegoObrazu();
                if(sprawdzanieCzyNieMaObrazu.Succes ==false)
                {
                    return sprawdzanieCzyNieMaObrazu;
                }
                //Sprawdzanie czy podano wiadomosc
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscJestPusta();
                if (sprawdzenieMiejscaWiadomosci.Succes == true)
                {
                    return (false, "Nie podano wiadomości do ukrycia");
                }
                //Sprawdzanie czy wiadomość się zmieści
                string  wiadomosc = _zarzadzanieGui.PobierzWartoscWiadomoscTextBox();
                long ileBitowDoUkrycia = (long)Encoding.UTF8.GetByteCount(wiadomosc) * 8 + 32;
                if (_zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox() == true)
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
                //2147483647 - jest to maksymalna wartość dla 4 bajtów przy uwzglednienu po 2 bajty na by zapewnić polskie znaki.
                if (ileBitowDoUkrycia > 2147483647)
                {
                    return (false, "Wiadomość jest za długa. Maksymalnie może mieć 2147483647");
                }
            }
            else
            {
                //sprawdzanie czy wiadomość jest pusta
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscJestPusta();
                if (sprawdzenieMiejscaWiadomosci.Succes == false)
                {
                    return sprawdzenieMiejscaWiadomosci;
                }
            }
            //SPrawdzanie rodzaju algorytmu
            if (_zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox() == false)
            {
                var sprawdzSeed = SprawdzCzySeedcJestPusta();
                if (sprawdzSeed.Succes == false)
                {
                    return sprawdzSeed;
                }
            }

            return (true, "");
        }
        private (bool Succes, string komunikat) SprawdzenieSciezkiPNG()
        {
            if (string.IsNullOrWhiteSpace(_zarzadzanieGui.PobierzWartoscPNGTextBox()) == true)
            {
                return (false, "Nie wybrano pliku PNG.");
            }
            return (true, "");
        }
        private (bool Succes, string komunikat) SprawdzenieMiejscaZapisu()
        {
            if (string.IsNullOrWhiteSpace(_zarzadzanieGui.PobierzWartoscSciezkaDoZapisuTextBox()) == true)
            {
                return (false, "Nie wybrano miejsca zapisu");
            }
            return (true, "");
        }
        private (bool Succes, string Komunikat) SprawdzCzyNieMaTakiegoObrazu()
        {
            string nowaSciezka = _zarzadzanieGui.PobierzWartoscSciezkaDoZapisuTextBox() + "\\NowyObraz.png";
            if(File.Exists(nowaSciezka) ==true)
            {
                return (false, "W wskazanym miejscu jest już obraz. Wybierz inne miejsce lub usuń stary.");
            }
            return (true, "");
        }
        private (bool Succes, string komunikat) SprawdzCzyWiadomoscJestPusta()
        {
            if (string.IsNullOrWhiteSpace(_zarzadzanieGui.PobierzWartoscWiadomoscTextBox()) == false)
            {
                return (false, "Miejsce wiadomosci nie jest puste");
            }
            return (true, "");
        }
        private (bool Succes, string komunikat) SprawdzCzySeedcJestPusta()
        {
            if (string.IsNullOrWhiteSpace(_zarzadzanieGui.PobierzWartoscSeedTextBox()) == true)
            {
                return (false, "Nie podano seed");
            }
            return (true, "");
        }
    }
}
