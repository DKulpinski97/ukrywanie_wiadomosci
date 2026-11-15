using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow
{
    public class ObslugaPlikowSerwisy:IObslugaPlikow
    {
        private readonly IZarzadzanieGui _zarzadzanieGui;
        public ObslugaPlikowSerwisy(IZarzadzanieGui zarzadzanieGui)
        {
            _zarzadzanieGui = zarzadzanieGui;
        }

        public void PrzypiszSciezkePNG()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Wybierz plik PNG",
                Filter = "Pliki PNG (*.png)|*.png",
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true
            };
            openFileDialog.ShowDialog();
            _zarzadzanieGui.PrzypiszWartoscPNGTextBox(openFileDialog.FileName);
        }
        public void PrzypiszSciezkeDoMiejscaZapisu()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.ShowDialog();
            _zarzadzanieGui.PrzypiszWartoscSciezkaDoZapisuTextBox(openFolderDialog.FolderName);
        }
    }
}
