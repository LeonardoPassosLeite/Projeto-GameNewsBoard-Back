namespace GameNewsBoard.Application.DTOs.Shared
{
    public class PaginatedBaseResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginatedResult<T> : PaginatedBaseResult<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(List<T> items, int page, int pageSize, int totalCount, int totalPages)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
    }

    public class PaginatedFromApiResult<T> : PaginatedBaseResult<T>
    {
        public PaginatedFromApiResult(List<T> items, int page, int pageSize)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
        }
    }
}
