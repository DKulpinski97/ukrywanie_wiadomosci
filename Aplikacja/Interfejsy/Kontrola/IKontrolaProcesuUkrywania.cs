using System.Drawing;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola
{
    public interface IKontrolaProcesuUkrywania
    {
        public void KontrolaUkrywaniaStandard(Bitmap obraz);
        public (bool Succes, string komunikat) KontrolaOdczytuStandard(Bitmap obraz);
        public void KontrolaUkrywaniaLosowa(Bitmap obraz);
        public (bool Succes, string komunikat) KontrolaOdczytuLosowo(Bitmap obraz);
    }
}
