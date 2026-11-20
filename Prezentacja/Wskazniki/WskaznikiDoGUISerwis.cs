using System.Windows.Controls;
using static UkrywanieWiadomosci.Prezentacja.enumy.EnumDoGUI;

namespace UkrywanieWiadomosci.Prezentacja.Wskazniki
{
    public class WskaznikiDoGUISerwis
    {
        private readonly Dictionary<IdLabel, Label> Labels = new();
        private readonly Dictionary<IdTextBox, TextBox> TextBoxes = new();
        private readonly Dictionary<IdRadioBox, RadioButton> RadioBoxes = new();
        private readonly Dictionary<IdButton, Button> Buttons = new();

        // Button 
        public void Ustaw(IdButton id, Button element)
        {
            Buttons[id] = element;
        }
        // LABEL
        public void Ustaw(IdLabel id, Label element)
        {
            Labels[id] = element;
        }

        public Label Pobierz(IdLabel id)
        {
            return Labels[id];
        }

        // TEXTBOX
        public void Ustaw(IdTextBox id, TextBox element)
        {
            TextBoxes[id] = element;
        }

        public TextBox Pobierz(IdTextBox id)
        {
            return TextBoxes[id];
        }

        // RADIOBUTTON
        public void Ustaw(IdRadioBox id, RadioButton element)
        {
            RadioBoxes[id] = element;
        }

        public RadioButton Pobierz(IdRadioBox id)
        {
            return RadioBoxes[id];
        }
        public void UstawAktywnosc(bool aktywne)
        {
            foreach (var element in Buttons.Values)
            {
                element.IsEnabled = aktywne;
            }
            foreach (var element in TextBoxes.Values)
            {
                element.IsEnabled = aktywne;
            }

            foreach (var element in RadioBoxes.Values)
            {
                element.IsEnabled = aktywne;
            }
        }
    }
}
