using Microsoft.Win32;

namespace UkrywanieWiadomosci.Prezentacja.Serwisy
{
    public class DialogiPlikow
    {
        public string? PobierzSciezkePNG()
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
                return openFileDialog.FileName;
            }
            return null;
        }
        public string? PobierzSciezkeDoMiejscaZapisu()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (openFolderDialog.ShowDialog() == true)
            {
                return openFolderDialog.FolderName;
            }
            return null;
        }
    }
}
