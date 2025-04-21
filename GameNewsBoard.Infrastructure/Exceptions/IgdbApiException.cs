namespace GameNewsBoard.Application.Exceptions.Api
{
    public class IgdbApiException : Exception
    {
        public IgdbApiException(string message) : base(message) { }

        public IgdbApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}