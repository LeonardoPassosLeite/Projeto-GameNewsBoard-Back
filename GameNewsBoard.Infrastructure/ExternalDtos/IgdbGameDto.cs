using System.Text.Json.Serialization;
using GameNewsBoard.Infrastructure.ExternalDtos.Commons;

namespace GameNewsBoard.Infrastructure.ExternalDtos
{
    public class IgdbGameDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
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
}