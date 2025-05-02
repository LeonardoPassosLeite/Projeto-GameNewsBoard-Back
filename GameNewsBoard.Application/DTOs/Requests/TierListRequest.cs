using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.DTOs.Requests;

public class UpdateTierListRequest
{
    public string? NewTitle { get; set; }
    public string? NewImage { get; set; }
    public List<UpdateTierListEntryRequest> Entries { get; set; } = new();
}

public class UpdateTierListEntryRequest
{
    public int GameId { get; set; }
    public TierLevel Tier { get; set; }
}

public class TierListRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public Guid? ImageId { get; set; }
}
