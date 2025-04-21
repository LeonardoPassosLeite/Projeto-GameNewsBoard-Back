namespace GameNewsBoard.Application.Exceptions.Domain
{
    public class InvalidPaginationException : Exception
    {
        public InvalidPaginationException(string? message = null)
            : base(message ?? "Parâmetros de paginação inválidos.") { }
    }
}
