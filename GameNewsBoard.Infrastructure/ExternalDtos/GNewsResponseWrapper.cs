using System.Text.Json.Serialization;

namespace GameNewsBoard.Infrastructure.ExternalDtos
{
    public class GNewsResponseWrapper
    {
        [JsonPropertyName("totalArticles")]
        public int TotalResults { get; set; }

        [JsonPropertyName("articles")]
        public List<GameNewsDto> Articles { get; set; } = new();
    }
}