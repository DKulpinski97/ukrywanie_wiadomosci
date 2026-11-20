using System.Drawing;
using UkrywanieWiadomosci.Aplikacja.Model;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola
{
    public interface IKontrolaProcesuUkrywania
    {
        public void KontrolaUkrywaniaStandard(Bitmap obraz, DanePoczatkowe dane, DaneRaportowaniaPostepu raportery);
        public (bool Success, string Komunikat) KontrolaOdczytuStandard(Bitmap obraz, DaneRaportowaniaPostepu raportery);
        public void KontrolaUkrywaniaLosowa(Bitmap obraz,DanePoczatkowe dane, DaneRaportowaniaPostepu raportery);
        public (bool Success, string Komunikat) KontrolaOdczytuLosowo(Bitmap obraz, DanePoczatkowe dane, DaneRaportowaniaPostepu raportery);
    }
}
