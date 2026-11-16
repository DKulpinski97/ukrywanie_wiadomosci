using System.Drawing;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow
{
    public interface IObslugaPlikow
    {
        public void PrzypiszSciezkePNG();
        public void PrzypiszSciezkeDoMiejscaZapisu();
        public Bitmap WczytajObrazekPNG();
        public void ZapisNowegoObrazka(Bitmap obrazek);
    }
}
