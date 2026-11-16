namespace UkrywanieWiadomosci.Prezentacja.Interfejsy
{
    public interface IZarzadzanieGui
    {
        public void PrzypiszWartoscPNGTextBox(string text);
        public string PobierzWartoscPNGTextBox();
        public void PrzypiszWartoscSciezkaDoZapisuTextBox(string text);
        public void PrzypiszWartoscWiadomoscTextBox(string text);
        public string PobierzWartoscSciezkaDoZapisuTextBox();
        public string PobierzWartoscSeedTextBox();
        public string PobierzWartoscWiadomoscTextBox();
        public void PrzypiszWartoscStatusLabel(string text);
        public void PrzypiszWartoscKtoryZnakLabel(string text);
        public bool PobierzRodzajAlgorytmuRadioBox();
        public bool PobierzCzyUkrycRadioBox();


    }
}
