using GameNewsBoard.Application.DTOs.Shared;

namespace GameNewsBoard.Application.Results
{
    public static class PaginatedResultHelper
    {
        public static PaginatedResult<T> EmptyWithTotal<T>(int page, int pageSize)
        {
            return new PaginatedResult<T>(new List<T>(), page, pageSize, 0, 0);
        }

        public static PaginatedFromApiResult<T> Empty<T>(int page, int pageSize)
        {
            return new PaginatedFromApiResult<T>(new List<T>(), page, pageSize);
        }
    }
}
