using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.Responses.DTOs.Responses
{
    public class StatusGameResponse
    {
        public int GameId { get; set; }
        public GameResponse Game { get; set; } = null!;
        public Status Status { get; set; }
    }
}