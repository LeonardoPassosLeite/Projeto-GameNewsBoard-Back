namespace GameNewsBoard.Api.Utils
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}