using GameNewsBoard.Infrastructure.ExternalDtos.Commons;
using System.Text.Json.Serialization;

namespace GameNewsBoard.Infrastructure.ExternalDtos
{

    public record IgdbGameReleaseWrapperDto
    {
        public long? Date { get; init; }

        [JsonPropertyName("game")]
        public IgdbGameReleaseGameDto Game { get; init; } = new();
    }

    public record IgdbGameReleaseGameDto
    {
        public string Name { get; init; } = string.Empty;
        public List<IgdbPlatformDto>? Platforms { get; init; }
        public IgdbCoverDto? Cover { get; init; }
    }
}