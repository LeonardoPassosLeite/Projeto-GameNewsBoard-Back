using System.Text.Json.Serialization;

namespace GameNewsBoard.Infrastructure.ExternalDtos
{
    public class IgdbGameDto
    {
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("aggregated_rating")]
        public float? AggregatedRating { get; set; }

        [JsonPropertyName("rating")]
        public float? UserRating { get; set; }

        [JsonPropertyName("first_release_date")]
        public long? FirstReleaseDateUnix { get; set; }

        [JsonPropertyName("cover")]
        public IgdbCoverDto? Cover { get; set; }

        [JsonPropertyName("platforms")]
        public List<IgdbPlatformDto> Platforms { get; set; } = new();
    }

    public class IgdbCoverDto
    {
        public string Url { get; set; } = string.Empty;
    }

    public class IgdbPlatformDto
    {
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}