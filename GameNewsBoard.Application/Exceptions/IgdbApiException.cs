namespace GameNewsBoard.Application.Exceptions
{
    public class IgdbApiException : Exception
    {
        public IgdbApiException(string message) : base(message) { }

        public IgdbApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}