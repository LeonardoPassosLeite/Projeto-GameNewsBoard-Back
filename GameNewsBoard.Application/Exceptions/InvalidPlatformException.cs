namespace GameNewsBoard.Application.Exceptions.Domain
{
    public class InvalidPlatformException : Exception
    {
        public InvalidPlatformException() : base("Plataforma informada é inválida.") { }

        public InvalidPlatformException(string message) : base(message) { }

        public InvalidPlatformException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}