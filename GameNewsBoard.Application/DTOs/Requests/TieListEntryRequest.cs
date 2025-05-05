using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.DTOs.Requests
{
    public class TierListEntryRequest
    {
        public int GameId { get; set; }
        public TierLevel Tier { get; set; }
    }
}