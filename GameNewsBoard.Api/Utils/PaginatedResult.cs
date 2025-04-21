namespace GameNewsBoard.Api.Utils
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(List<T> items, int page, int pageSize)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
        }
    }
}