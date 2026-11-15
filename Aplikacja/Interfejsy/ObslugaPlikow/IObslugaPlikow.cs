using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow
{
    public interface IObslugaPlikow
    {
        public void PrzypiszSciezkePNG();
        public void PrzypiszSciezkeDoMiejscaZapisu();
        public Bitmap WczytajObrazekPNG();
    }
}
