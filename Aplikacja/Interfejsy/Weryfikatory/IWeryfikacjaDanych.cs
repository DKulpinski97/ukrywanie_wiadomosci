using System.Drawing;
using UkrywanieWiadomosci.Aplikacja.Model;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory
{
    public interface IWeryfikacjaDanych
    {
        public (bool Success, string Komunikat) WeryfikacjaDanychPoczatkowych(Bitmap bitmap, DanePoczatkowe dane);
        public (bool Success, string Komunikat) SprawdzanieCzySciezkaNieJestPusta(DanePoczatkowe dane);
    }
}
