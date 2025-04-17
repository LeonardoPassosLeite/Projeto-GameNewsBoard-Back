using System.Text.Json.Serialization;

namespace GameNewsBoard.Infrastructure.ExternalDtos
{
    public class GameNewsDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Link { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("publishedAt")]
        public DateTime PubDate { get; set; }
    }
}