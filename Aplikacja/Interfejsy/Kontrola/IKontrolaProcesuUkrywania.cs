using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola
{
    public interface IKontrolaProcesuUkrywania
    {
        public Bitmap KontrolaUkrywaniaStandard();
        string KontrolaOdczytuStandard();
        public Bitmap KontrolaUkrywaniaLosowa();
    }
}
