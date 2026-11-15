using System;
using System.Collections.Generic;
using System.Text;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.Weryfikatory
{
    public class WeryfikacjaDanychSerwiy:IWeryfikacjaDanych
    {
    private readonly IZarzadzanieGui _zarzadzanieGui;
        public WeryfikacjaDanychSerwiy(IZarzadzanieGui zarzadzanieGui)
        {
            _zarzadzanieGui = zarzadzanieGui;
        }

        public (bool Succes, string komunikat) WeryfikacjaDanychPoczaatkowych()
        {
            var sprawdzenieMiejsceObrazu = SprawdzenieSciezkiPNG();
            if (sprawdzenieMiejsceObrazu.Succes==false)
            {
                return (false, "Nie wybrano pliku PNG.");
            }
            if (_zarzadzanieGui.PobierzCzyUkrycRadioBox() == true)
            {
                var sprawdzenieMiejscaZapisu = SprawdzenieMiejscaZapisu();
                if (sprawdzenieMiejscaZapisu.Succes == false)
                {
                    return sprawdzenieMiejscaZapisu;
                }
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscJestPusta();
                if (sprawdzenieMiejscaWiadomosci.Succes == true) 
                {
                    return (false,"Nie podano wiadomości do ukrycia");
                }
            }
            else
            {
                var sprawdzenieMiejscaWiadomosci = SprawdzCzyWiadomoscJestPusta();
                if (sprawdzenieMiejscaWiadomosci.Succes == false)
                {
                    return sprawdzenieMiejscaWiadomosci;
                }
            }
            if(_zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox()==false)
            {
                var sprawdzSeed = SprawdzCzySeedcJestPusta();
                if(sprawdzSeed.Succes == false)
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
