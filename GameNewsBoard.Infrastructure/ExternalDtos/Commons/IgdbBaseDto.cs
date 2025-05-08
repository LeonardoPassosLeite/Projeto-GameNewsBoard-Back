using System.Text.Json.Serialization;

namespace GameNewsBoard.Infrastructure.ExternalDtos.Commons;

public record IgdbPlatformDto
{
    public int Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

public record IgdbCoverDto
{
    public string Url { get; init; } = string.Empty;
}
