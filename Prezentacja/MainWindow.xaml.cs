using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Exceptions;
using UkrywanieWiadomosci.Prezentacja.enumy;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;
using UkrywanieWiadomosci.Prezentacja.Wskazniki;

namespace UkrywanieWiadomosci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WskaznikiDoGUISerwis _wskaznikiGUI;
        private readonly IObslugaPlikow _obslugaPlikow;
        private readonly IWeryfikacjaDanych _weryfikacjaDanych;
        private readonly IKontrolaProcesuUkrywania _kontrolaProcesuUkrywania;
        private readonly IZarzadzanieGui _zarzadzanieGui;
        public MainWindow(WskaznikiDoGUISerwis wskaznikiGUI, IObslugaPlikow obslugaPlikow, IWeryfikacjaDanych weryfikacjaDanych,
            IKontrolaProcesuUkrywania kontrolaProcesuUkrywania, IZarzadzanieGui zarzadzanieGui)
        {
            _obslugaPlikow = obslugaPlikow;
            _wskaznikiGUI = wskaznikiGUI;
            _weryfikacjaDanych = weryfikacjaDanych;
            _kontrolaProcesuUkrywania = kontrolaProcesuUkrywania;
            _zarzadzanieGui = zarzadzanieGui;
            InitializeComponent();
            przypiszGUI();
        }

        private void WyborPNGButton_Click(object sender, RoutedEventArgs e)
        {
            _obslugaPlikow.PrzypiszSciezkePNG();
        }

        private void WyborSciezkiZapisuButton_Click(object sender, RoutedEventArgs e)
        {
            _obslugaPlikow.PrzypiszSciezkeDoMiejscaZapisu();
        }

        private void RozpocznijButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _zarzadzanieGui.PrzypiszWartoscStatusLabel("Sprawdzanie danych początkowych");
                var tmp = _weryfikacjaDanych.WeryfikacjaDanychPoczatkowych();
                if (tmp.Succes == false)
                {
                    _zarzadzanieGui.PrzypiszWartoscStatusLabel("Czekam na poprawe danych");
                    MessageBox.Show(tmp.komunikat);
                    return;
                }
                else
                {
                    if (_zarzadzanieGui.PobierzCzyUkrycRadioBox() == true)
                    {
                        _zarzadzanieGui.PrzypiszWartoscStatusLabel("Ukrywam wiadomość");
                        Bitmap wynik;
                        if (_zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox() == true)
                        {
                            wynik = _kontrolaProcesuUkrywania.KontrolaUkrywaniaStandard();
                        }
                        else
                        {
                            wynik = _kontrolaProcesuUkrywania.KontrolaUkrywaniaLosowa();
                        }
                        _zarzadzanieGui.PrzypiszWartoscStatusLabel("Czekam na nowe polecenie");
                        _obslugaPlikow.ZapisNowegoObrazka(wynik);
                        MessageBox.Show("Obraz zapisany");
                        return;
                    }
                    else
                    {
                        _zarzadzanieGui.PrzypiszWartoscStatusLabel("Odczytuje dane");
                        string wiadomosc = "";
                        bool czyUdane = false;
                        if (_zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox() == true)
                        {
                            var wynik = _kontrolaProcesuUkrywania.KontrolaOdczytuStandard();
                            wiadomosc = wynik.komunikat;
                            czyUdane = wynik.Succes;
                        }
                        else
                        {
                            var wynik = _kontrolaProcesuUkrywania.KontrolaOdczytuLosowo();
                            wiadomosc = wynik.komunikat;
                            czyUdane = wynik.Succes;
                        }
                        _zarzadzanieGui.PrzypiszWartoscStatusLabel("Czekam na nowe polecenie");
                        if (czyUdane == true)
                        {
                            _zarzadzanieGui.PrzypiszWartoscWiadomoscTextBox(wiadomosc);
                            MessageBox.Show("Wiadomość odczytana");
                            return;
                        }
                        else
                        {
                            MessageBox.Show(wiadomosc);
                            return;
                        }
                    }
                }
            }
            catch(BladOperacjiException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd krytyczny, spróbuj ponownie");
            }
        }
        
        private void przypiszGUI()
        {
            // Ustawianie Label
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdLabel.StatusLabel, this.StatusLabel);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdLabel.KtoryBit, this.KtoryBit);

            // Ustawianie TextBox
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdTextBox.SciezkaDoPNGTextbox, this.SciezkaDoPNGTextbox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdTextBox.SciezkaDoZapisuTextbox, this.SciezkaDoZapisuTextbox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdTextBox.WiadomoscTextBox, this.WiadomoscTextBox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdTextBox.SeedTextBox, this.SeedTextBox);

            // Ustawianie RadioButton
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdRadioBox.UkrycRadioBox, this.UkrycRadioBox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdRadioBox.OdczytacRadioBox, this.OdczytacRadioBox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdRadioBox.StandardRadioBox, this.StandardRadioBox);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdRadioBox.LosowyRadioBox, this.LosowyRadioBox);
        }
    }
}