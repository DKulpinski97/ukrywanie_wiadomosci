using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola
{
    public interface IKontrolaProcesuUkrywania
    {
        public Bitmap KontrolaUkrywaniaStandard();
        public (bool Succes, string komunikat) KontrolaOdczytuStandard();
        public Bitmap KontrolaUkrywaniaLosowa();
        public (bool Succes, string komunikat) KontrolaOdczytuLosowo();
    }
}
