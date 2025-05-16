using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.IServices
{
    public interface IGameReleaseService
    {
        Task<List<GameReleaseResponse>> GetUpcomingGamesAsync(int daysAhead = 7, Platform? platform = null, CancellationToken cancellationToken = default);
        Task<List<GameReleaseResponse>> GetRecentlyReleasedGamesAsync(int daysBack = 7, Platform? platform = null, CancellationToken cancellationToken = default);
        Task<List<GameReleaseResponse>> GetReleasesBetweenAsync(DateTime start, DateTime end, Platform? platform = null, CancellationToken cancellationToken = default);
    }
}