using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Results;

namespace GameNewsBoard.Api.Helpers
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<PaginatedResult<T>> CreatePaginatedSuccess<T>(
            IEnumerable<T> items, int page, int pageSize, string message, int totalCount, int totalPages)
        {
            return new ApiResponse<PaginatedResult<T>>
            {
                Message = message,
                Data = new PaginatedResult<T>(items.ToList(), page, pageSize, totalCount, totalPages)  
            };
        }

        public static ApiResponse<PaginatedFromApiResult<T>> CreatePaginatedSuccess<T>(
            IEnumerable<T> items, int page, int pageSize, string message)
        {
            return new ApiResponse<PaginatedFromApiResult<T>>
            {
                Message = message,
                Data = new PaginatedFromApiResult<T>(items.ToList(), page, pageSize)  
            };
        }

        public static ApiResponse<PaginatedFromApiResult<T>> CreateEmptyPaginated<T>(
            int page, int pageSize, string message)
        {
            return new ApiResponse<PaginatedFromApiResult<T>>
            {
                Message = message,
                Data = PaginatedResultHelper.Empty<T>(page, pageSize) 
            };
        }
    }
}
