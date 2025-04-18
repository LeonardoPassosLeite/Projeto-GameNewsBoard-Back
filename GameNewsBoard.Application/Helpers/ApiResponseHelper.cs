using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.Commons;

namespace GameNewsBoard.Application.Helpers
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<PaginatedResult<T>> CreatePaginatedSuccess<T>(
            List<T> items, int page, int pageSize, string message)
        {
            return new ApiResponse<PaginatedResult<T>>
            {
                Message = message,
                Data = new PaginatedResult<T>
                {
                    Items = items,
                    Page = page,
                    PageSize = pageSize
                }
            };
        }

        public static ApiResponse<PaginatedResult<T>> CreateEmptyPaginated<T>(
            int page, int pageSize, string message)
        {
            return new ApiResponse<PaginatedResult<T>>
            {
                Message = message,
                Data = PaginatedResultHelper.Empty<T>(page, pageSize)
            };
        }
    }
}