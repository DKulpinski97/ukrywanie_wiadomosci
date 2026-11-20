using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Domena.Reguly;
using UkrywanieWiadomosci.Exceptions;

namespace UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow
{
    public class ObslugaPlikowSerwisy : IObslugaPlikow
    {
        public Bitmap WczytajObrazekPNG(string sciezka)
        {
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
                throw new BladOperacjiException("Wystąpił błąd krytyczny. Wybrany plik prawdopodobnie nie jest prawidłowym obrazem PNG.", ex);
            }
        }

        public void ZapisNowegoObrazka(Bitmap obraz,string sciezka)
        {
            try
            {
                string nowaSciezka = Path.Combine(sciezka, NazwaDlaPliku.NazwaPliku);
                obraz.Save(nowaSciezka, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new BladOperacjiException("Nie udało się zapisać pliku.",ex);
            }
        }
    }
}
