using System;
using System.Collections.Generic;
using System.Text;

namespace UkrywanieWiadomosci.Prezentacja.Interfejsy
{
    public interface IZarzadzanieGui
    {
        public void PrzypiszWartoscPNGTextBox(string text);
        public string PobierzWartoscPNGTextBox(string text);
        public void PrzypiszWartoscSciezkaDoZapisuTextBox(string text);
        public string PobierzWartoscSciezkaDoZapisuTextBox(string text);
        public void PrzypiszWartoscWiadomoscTextBox(string text);
        public string PobierzWartoscWiadomoscTextBox(string text);
        public void PrzypiszWartoscStatusLabel(string text);
        public void PrzypiszWartoscKtoryZnakLabel(string text);
        public bool PobierzRodzajAlgorytmuRadioBox();
        public bool PobierzCzyUkrycRadioBox();


    }
}
