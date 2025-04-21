
using GameNewsBoard.Application.Exceptions.Domain;

namespace GameNewsBoard.Application.Validators
{
    public static class PaginationValidator
    {
        public static void Validate(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
                throw new InvalidPaginationException("Página e tamanho devem ser maiores que zero.");
        }
    }
}