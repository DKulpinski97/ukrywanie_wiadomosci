using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Controls;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Exceptions;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow
{
    public class ObslugaPlikowSerwisy : IObslugaPlikow
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
        public Bitmap WczytajObrazekPNG()
        {
            var sciezka = _zarzadzanieGui.PobierzWartoscPNGTextBox();

            byte[] bytes = File.ReadAllBytes(sciezka);
            try
            {
                using var stream = new MemoryStream(bytes);
                using var temp = System.Drawing.Image.FromStream(stream, true, true);
                return new Bitmap(temp);
            }
            catch
            {
                throw new BladOperacjiException("Błąd krytyczny najprawdopodobniej to nie PNG");
            }
        }

        public void ZapisNowegoObrazka(Bitmap obraz)
        {
            try
            {
                string nowaSciezka = _zarzadzanieGui.PobierzWartoscSciezkaDoZapisuTextBox() + "/NowyObraz.png";
                obraz.Save(nowaSciezka, ImageFormat.Png);
            }
            catch
            {
                throw new BladOperacjiException("Nie udało się zapisać pliku");
            }
        }
    }
}
