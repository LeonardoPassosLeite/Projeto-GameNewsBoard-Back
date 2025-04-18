namespace GameNewsBoard.Application.Responses.DTOs
{
    public class GameNewsResponse
    {
        public int TotalResults { get; set; }
        public List<GameNewsArticleResponse> Articles { get; set; } = new();
    }

    public class GameNewsArticleResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime PubDate { get; set; }
    }
}