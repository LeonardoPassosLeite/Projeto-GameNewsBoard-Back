namespace GameNewsBoard.Application.Responses.DTOs.Responses;

public class TierListEntryResponse
{
    public int GameId { get; set; }
    public GameResponse Game { get; set; } = null!;
    public TierLevel Tier { get; set; }
}
