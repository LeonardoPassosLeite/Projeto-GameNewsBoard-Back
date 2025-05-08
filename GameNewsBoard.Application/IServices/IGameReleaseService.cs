using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Responses.DTOs.Responses;

namespace GameNewsBoard.Application.IServices
{
    public interface IGameReleaseService
    {
        Task<List<GameReleaseResponse>> GetUpcomingGamesAsync(int daysAhead = 7, CancellationToken cancellationToken = default);
        Task<List<GameReleaseResponse>> GetRecentlyReleasedGamesAsync(int daysBack = 7, CancellationToken cancellationToken = default);
    }
}