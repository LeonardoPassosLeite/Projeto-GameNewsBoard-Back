namespace GameNewsBoard.Application.Exceptions
{
    public class InvalidYearCategoryException : Exception
    {
        public InvalidYearCategoryException()
            : base("Categoria de ano inválida.") { }

        public InvalidYearCategoryException(string? message)
            : base(message ?? "Categoria de ano inválida.") { }

        public InvalidYearCategoryException(string? message, Exception? innerException)
            : base(message ?? "Categoria de ano inválida.", innerException) { }
    }
}