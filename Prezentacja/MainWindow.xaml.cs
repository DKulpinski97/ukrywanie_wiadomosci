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
        public MainWindow(WskaznikiDoGUISerwis wskaznikiGUI)
        {
            InitializeComponent();
            _wskaznikiGUI = wskaznikiGUI;
            przypiszGUI();
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

        private void WyborPNGButton_Click(object sender, RoutedEventArgs e)
        {
            var a = _wskaznikiGUI;
        }

        private void WyborSciezkiZapisuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RozpocznijButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}