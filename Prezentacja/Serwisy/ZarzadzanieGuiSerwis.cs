using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using UkrywanieWiadomosci.Prezentacja.enumy;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;
using UkrywanieWiadomosci.Prezentacja.Wskazniki;

namespace UkrywanieWiadomosci.Prezentacja.Serwisy
{
    public  class ZarzadzanieGuiSerwis : IZarzadzanieGui
    {
        private readonly WskaznikiDoGUISerwis _wskaznikiDoGUI;
        public ZarzadzanieGuiSerwis(WskaznikiDoGUISerwis wskaznikiDoGUI)
        {
            _wskaznikiDoGUI = wskaznikiDoGUI;
        }
        //Obsługa textBox
        public void PrzypiszWartoscPNGTextBox(string text) 
        {
            PrzypiszWartoscTextBox(_wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.SciezkaDoPNGTextbox), text);
        }
        public string PobierzWartoscPNGTextBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.SciezkaDoPNGTextbox).Text;
        }
        public void PrzypiszWartoscSciezkaDoZapisuTextBox(string text)
        {
            PrzypiszWartoscTextBox(_wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.SciezkaDoZapisuTextbox), text);
        }
        public void PrzypiszWartoscWiadomoscTextBox(string text)
        {
            PrzypiszWartoscTextBox(_wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.WiadomoscTextBox), text);
        }
        public string PobierzWartoscSciezkaDoZapisuTextBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.SciezkaDoZapisuTextbox).Text;
        }
        public string PobierzWartoscWiadomoscTextBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.WiadomoscTextBox).Text;
        }

        public string PobierzWartoscSeedTextBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdTextBox.SeedTextBox).Text;
        }
        //Obsługa label
        public void PrzypiszWartoscStatusLabel(string text)
        {
            PrzypiszWartoscLabel(_wskaznikiDoGUI.Pobierz(EnumDoGUI.IdLabel.StatusLabel), text);
        }
        public void PrzypiszWartoscKtoryZnakLabel(string text)
        {
            PrzypiszWartoscLabel(_wskaznikiDoGUI.Pobierz(EnumDoGUI.IdLabel.KtoryBit), text);
        }
        //Pobierz wartość z radiobox
        public bool PobierzRodzajAlgorytmuRadioBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdRadioBox.StandardRadioBox).IsChecked == true;
        }
        public bool PobierzCzyUkrycRadioBox()
        {
            return _wskaznikiDoGUI.Pobierz(EnumDoGUI.IdRadioBox.UkrycRadioBox).IsChecked == true;
        }
        //Funkcje pomocnicze
        private void PrzypiszWartoscLabel(Label label, string text)
        {
            if (label.Dispatcher.CheckAccess())
            {
                label.Content = text;
            }
            else
            {
                label.Dispatcher.Invoke(() =>
                {
                    label.Content = text;
                });
            }
        }
        private void PrzypiszWartoscTextBox(TextBox textBox, string text)
        {
            if (textBox.Dispatcher.CheckAccess())
            {
                textBox.Text = text;
            }
            else
            {
                textBox.Dispatcher.Invoke(() =>
                {
                    textBox.Text = text;
                });
            }
        }
    }
}
