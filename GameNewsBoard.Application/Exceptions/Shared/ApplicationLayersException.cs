namespace GameNewsBoard.Application.Exceptions.Shared
{
    public abstract class ApplicationLayerException : Exception
    {
        protected ApplicationLayerException(string message) : base(message) { }
        protected ApplicationLayerException(string message, Exception inner) : base(message, inner) { }
    }
}