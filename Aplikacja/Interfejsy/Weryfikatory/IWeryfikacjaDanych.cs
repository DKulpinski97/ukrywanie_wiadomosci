using System.Drawing;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory
{
    public interface IWeryfikacjaDanych
    {
        public (bool Succes, string komunikat) WeryfikacjaDanychPoczatkowych(Bitmap bitmap);
        public (bool Succes, string komunikat) SprawdzanieCzySciezkaNieJestPusta();
    }
}
