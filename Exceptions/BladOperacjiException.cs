namespace UkrywanieWiadomosci.Exceptions
{
    public class BladOperacjiException : Exception
    {
        public BladOperacjiException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
