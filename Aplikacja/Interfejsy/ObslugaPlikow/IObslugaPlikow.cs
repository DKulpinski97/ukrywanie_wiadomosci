using System.Drawing;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow
{
    public interface IObslugaPlikow
    {
        public Bitmap WczytajObrazekPNG(string sciezka);
        public void ZapisNowegoObrazka(Bitmap obrazek, string sciezka);
    }
}
