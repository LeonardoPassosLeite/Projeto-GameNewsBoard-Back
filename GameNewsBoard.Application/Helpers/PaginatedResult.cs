using GameNewsBoard.Application.DTOs.Shared;

namespace GameNewsBoard.Application.Helpers
{
    public static class PaginatedResultHelper
    {
        public static PaginatedResult<T> Empty<T>(int page, int pageSize)
        {
            return new PaginatedResult<T>
            {
                Items = new List<T>(),
                Page = page,
                PageSize = pageSize
            };
        }
    }
}