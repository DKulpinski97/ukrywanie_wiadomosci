using System.Windows.Controls;
using static UkrywanieWiadomosci.Prezentacja.enumy.EnumDoGUI;

namespace UkrywanieWiadomosci.Prezentacja.Wskazniki
{
    public class WskaznikiDoGUISerwis
    {
        private Dictionary<IdLabel, Label> Labels = new();
        private Dictionary<IdTextBox, TextBox> TextBoxes = new();
        private Dictionary<IdRadioBox, RadioButton> RadioBoxes = new();

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
    }
}
