using System;
using System.Collections.Generic;
using System.Text;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory
{
    public interface IWeryfikacjaDanych
    {
        public (bool Succes, string komunikat) WeryfikacjaDanychPoczaatkowych();
    }
}
