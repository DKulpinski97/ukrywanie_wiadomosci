namespace UkrywanieWiadomosci.Aplikacja.Model
{
    public record DaneRaportowaniaPostepu(
    IProgress<string> Status,
    IProgress<string> KtoryBit);
}
