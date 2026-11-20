namespace UkrywanieWiadomosci.Aplikacja.Model
{

    public enum RodzajAlgorytmu { Standard, Losowy }
    public enum CoRobic { Ukryj, Odczytaj }
    public record DanePoczatkowe(
    string SciezkaDoPNG,
    string SciezkaDoZapisu,
    string Wiadomosc,
    string Seed,
    RodzajAlgorytmu Algorytm,
    CoRobic Proces
    );

}
