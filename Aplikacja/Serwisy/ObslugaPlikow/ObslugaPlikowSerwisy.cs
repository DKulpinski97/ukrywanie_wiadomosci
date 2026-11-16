using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Domena.Reguly;
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
            if (openFileDialog.ShowDialog() == true)
            {
                _zarzadzanieGui.PrzypiszWartoscPNGTextBox(openFileDialog.FileName);
            }
        }
        public void PrzypiszSciezkeDoMiejscaZapisu()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (openFolderDialog.ShowDialog() == true)
            {
                _zarzadzanieGui.PrzypiszWartoscSciezkaDoZapisuTextBox(openFolderDialog.FolderName);
            }
        }
        public Bitmap WczytajObrazekPNG()
        {
            var sciezka = _zarzadzanieGui.PobierzWartoscPNGTextBox();

            byte[] bytes;

            try
            {
                bytes = File.ReadAllBytes(sciezka);
            }
            catch (IOException ex)
            {
                throw new BladOperacjiException("Nie można odczytać pliku.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new BladOperacjiException("Brak uprawnień do odczytu pliku.",ex);
            }

            try
            {
                using var stream = new MemoryStream(bytes);
                using var temp = System.Drawing.Image.FromStream(stream, true, true);

                return new Bitmap(temp);
            }
            catch (Exception ex)
            {
                throw new BladOperacjiException("Błąd krytyczny najprawdopodobniej to nie PNG.", ex);
            }
        }

        public void ZapisNowegoObrazka(Bitmap obraz)
        {
            try
            {
                string nowaSciezka = Path.Combine(_zarzadzanieGui.PobierzWartoscSciezkaDoZapisuTextBox(), RegulyNazwa.NazwaPliku);
                obraz.Save(nowaSciezka, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new BladOperacjiException("Nie udało się zapisać pliku",ex);
            }
        }
    }
}
