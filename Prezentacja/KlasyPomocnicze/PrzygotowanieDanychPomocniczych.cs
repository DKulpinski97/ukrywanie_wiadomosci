using UkrywanieWiadomosci.Aplikacja.Model;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;

namespace UkrywanieWiadomosci.Prezentacja.KlasyPomocnicze
{

    public class PrzygotowanieDanychPomocniczych
    {
        private readonly IZarzadzanieGui _zarzadzanieGui;
        public PrzygotowanieDanychPomocniczych(IZarzadzanieGui zarzadzanieGui)
        {
            _zarzadzanieGui = zarzadzanieGui;
        }
       

        public DanePoczatkowe PrzygotujDane()
        {
            return new DanePoczatkowe(
                _zarzadzanieGui.PobierzWartoscPNGTextBox(),
                _zarzadzanieGui.PobierzWartoscSciezkaDoZapisuTextBox(),
                _zarzadzanieGui.PobierzWartoscWiadomoscTextBox(),
                _zarzadzanieGui.PobierzWartoscSeedTextBox(),
                PrzypiszRodzajAlgorytmu(),
                PrzypiszCoRobic()
            );
        }
        public DaneRaportowaniaPostepu UtworzRaportowaniePostepu()
        {
            return new DaneRaportowaniaPostepu(
                new Progress<string>(text => _zarzadzanieGui.PrzypiszWartoscStatusLabel(text)),
                new Progress<string>(text => _zarzadzanieGui.PrzypiszWartoscKtoryZnakLabel(text))
            );
        }
        private RodzajAlgorytmu PrzypiszRodzajAlgorytmu()
        {
            return _zarzadzanieGui.PobierzRodzajAlgorytmuRadioBox()
                ? RodzajAlgorytmu.Standard
                : RodzajAlgorytmu.Losowy;
        }

        private CoRobic PrzypiszCoRobic()
        {
            return _zarzadzanieGui.PobierzCzyUkrycRadioBox()
                ? CoRobic.Ukryj
                : CoRobic.Odczytaj;
        }
    }
}
