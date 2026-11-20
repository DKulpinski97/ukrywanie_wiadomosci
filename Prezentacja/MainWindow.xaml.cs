using System.Diagnostics;
using System.Drawing;
using System.Windows;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Aplikacja.Model;
using UkrywanieWiadomosci.Domena.Reguly;
using UkrywanieWiadomosci.Exceptions;
using UkrywanieWiadomosci.Prezentacja.enumy;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;
using UkrywanieWiadomosci.Prezentacja.KlasyPomocnicze;
using UkrywanieWiadomosci.Prezentacja.Serwisy;
using UkrywanieWiadomosci.Prezentacja.Wskazniki;

namespace UkrywanieWiadomosci
{
    public partial class MainWindow : Window
    {
        private readonly WskaznikiDoGUISerwis _wskaznikiGUI;
        private readonly IObslugaPlikow _obslugaPlikow;
        private readonly IWeryfikacjaDanych _weryfikacjaDanych;
        private readonly IKontrolaProcesuUkrywania _kontrolaProcesuUkrywania;
        private readonly IZarzadzanieGui _zarzadzanieGui;
        private readonly PrzygotowanieDanychPomocniczych _przygotowanieDanychPomocniczych;
        private readonly DaneRaportowaniaPostepu _raportowaniePostepuLabel;
        private readonly DialogiPlikow _dialogiPlikow;
        public MainWindow(WskaznikiDoGUISerwis wskaznikiGUI, IObslugaPlikow obslugaPlikow, IWeryfikacjaDanych weryfikacjaDanych,
            IKontrolaProcesuUkrywania kontrolaProcesuUkrywania, IZarzadzanieGui zarzadzanieGui, PrzygotowanieDanychPomocniczych przygotowanieDanychPomocniczych, DialogiPlikow dialogiPlikow)
        {
            InitializeComponent();
            _dialogiPlikow = dialogiPlikow;
            _obslugaPlikow = obslugaPlikow;
            _wskaznikiGUI = wskaznikiGUI;
            _weryfikacjaDanych = weryfikacjaDanych;
            _kontrolaProcesuUkrywania = kontrolaProcesuUkrywania;
            _zarzadzanieGui = zarzadzanieGui;
            _przygotowanieDanychPomocniczych = przygotowanieDanychPomocniczych;
            PrzypiszElementyGUI();
            _raportowaniePostepuLabel = _przygotowanieDanychPomocniczych.UtworzRaportowaniePostepu();
        }

        private void WyborPNGButton_Click(object sender, RoutedEventArgs e)
        {
            string? sciezkaDoPNG = _dialogiPlikow.PobierzSciezkePNG();
            if(sciezkaDoPNG != null)
            {
                _zarzadzanieGui.PrzypiszWartoscPNGTextBox(sciezkaDoPNG);
            }
        }

        private void WyborSciezkiZapisuButton_Click(object sender, RoutedEventArgs e)
        {
            string? sciezkaDoMiejscaZapisu = _dialogiPlikow.PobierzSciezkeDoMiejscaZapisu();
            if(sciezkaDoMiejscaZapisu != null)
            {
                _zarzadzanieGui.PrzypiszWartoscSciezkaDoZapisuTextBox(sciezkaDoMiejscaZapisu);
            }
        }

        private void RozpocznijButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _raportowaniePostepuLabel.KtoryBit.Report("0/0");
                var przygotujDane = _przygotowanieDanychPomocniczych.PrzygotujDane();
                var sprawdzenieSciezki = _weryfikacjaDanych.SprawdzanieCzySciezkaNieJestPusta(przygotujDane);
                if (!sprawdzenieSciezki.Success)
                {
                    _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.PoprawaDanych);
                    MessageBox.Show(sprawdzenieSciezki.Komunikat);
                    return;
                }
                _raportowaniePostepuLabel.Status.Report("Sprawdzanie danych początkowych");
                using (Bitmap obraz = _obslugaPlikow.WczytajObrazekPNG(przygotujDane.SciezkaDoPNG))
                {
                    var danePoczatkowe = _weryfikacjaDanych.WeryfikacjaDanychPoczatkowych(obraz, przygotujDane);
                    if (!danePoczatkowe.Success)
                    {
                        _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.PoprawaDanych);
                        MessageBox.Show(danePoczatkowe.Komunikat);
                        return;
                    }
                    else
                    {
                        //czy ukryć
                        if (przygotujDane.Proces == CoRobic.Ukryj)
                        {
                            if (przygotujDane.Algorytm == RodzajAlgorytmu.Standard)
                            {
                                _kontrolaProcesuUkrywania.KontrolaUkrywaniaStandard(obraz, przygotujDane, _raportowaniePostepuLabel);
                            }
                            else
                            {
                                _kontrolaProcesuUkrywania.KontrolaUkrywaniaLosowa(obraz, przygotujDane, _raportowaniePostepuLabel);
                            }
                            _obslugaPlikow.ZapisNowegoObrazka(obraz, przygotujDane.SciezkaDoZapisu);
                            _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.NowePolecenie);
                            MessageBox.Show("Obraz zapisany");
                            return;
                        }
                        else
                        {
                            _raportowaniePostepuLabel.Status.Report("Odczytuję dane");
                            string wiadomosc = "";
                            bool czyUdane = false;
                            if (przygotujDane.Algorytm == RodzajAlgorytmu.Standard)
                            {
                                var wynik = _kontrolaProcesuUkrywania.KontrolaOdczytuStandard(obraz, _raportowaniePostepuLabel);
                                wiadomosc = wynik.Komunikat;
                                czyUdane = wynik.Success;
                            }
                            else
                            {
                                var wynik = _kontrolaProcesuUkrywania.KontrolaOdczytuLosowo(obraz, przygotujDane, _raportowaniePostepuLabel);
                                wiadomosc = wynik.Komunikat;
                                czyUdane = wynik.Success;
                            }
                            _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.NowePolecenie);
                            if (czyUdane)
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
            }
            catch (BladOperacjiException ex)
            {
                //Do podglądu błędu
                Debug.WriteLine(ex);
                _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.PoprawaDanych);
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                //Do podglądu błędu
                Debug.WriteLine(ex);
                _raportowaniePostepuLabel.Status.Report(StaleKomunikatow.PoprawaDanych);
                MessageBox.Show("Wystąpił błąd krytyczny, spróbuj ponownie");
            }
        }

        private void PrzypiszElementyGUI()
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