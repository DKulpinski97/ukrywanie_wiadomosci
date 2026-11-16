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
using UkrywanieWiadomosci.Prezentacja.enumy;
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
        public MainWindow(WskaznikiDoGUISerwis wskaznikiGUI, IObslugaPlikow obslugaPlikow, IWeryfikacjaDanych weryfikacjaDanych,
            IKontrolaProcesuUkrywania kontrolaProcesuUkrywania)
        {
            _obslugaPlikow = obslugaPlikow;
            _wskaznikiGUI = wskaznikiGUI;
            _weryfikacjaDanych = weryfikacjaDanych;
            _kontrolaProcesuUkrywania = kontrolaProcesuUkrywania;
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
            var tmp = _weryfikacjaDanych.WeryfikacjaDanychPoczaatkowych();
            if (tmp.Succes == false)
            {
                MessageBox.Show(tmp.komunikat);
            }
            else
            {
                Bitmap wynik =  _kontrolaProcesuUkrywania.KontrolaUkrywaniaStandard();
            }
        }
        private void przypiszGUI()
        {
            // Ustawianie Label
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdLabel.StatusLabel, this.StatusLabel);
            _wskaznikiGUI.Ustaw(EnumDoGUI.IdLabel.KtoryZnak, this.KtoryZnak);

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