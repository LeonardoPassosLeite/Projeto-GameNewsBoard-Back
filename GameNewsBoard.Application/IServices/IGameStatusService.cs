using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Application.Responses.DTOs.Responses;

namespace GameNewsBoard.Application.IServices
{
    public interface IGameStatusService
    {
        Task<Result> SetGameStatusAsync(Guid userId, int gameId, Status status);
        Task<Result<IEnumerable<GameStatusResponse>>> GetUserGameStatusesAsync(Guid userId);
        Task<Result> RemoveGameStatusAsync(Guid userId, int gameId);
    }
}
