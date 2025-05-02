using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Application.Responses.DTOs.Responses;

namespace GameNewsBoard.Application.IServices
{
    public interface IStatusGameService
    {
        Task<Result> SetStatusAsync(Guid userId, int gameId, Status status);
        Task<Result<IEnumerable<StatusGameResponse>>> GetUserGameStatusesAsync(Guid userId);
        Task<Result> RemoveStatusAsync(Guid userId, int gameId);
    }
}
