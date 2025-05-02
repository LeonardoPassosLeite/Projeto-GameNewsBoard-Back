namespace GameNewsBoard.Application.Responses.DTOs.Responses;

public class TierListResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<TierListEntryResponse> Entries { get; set; } = new();
}
