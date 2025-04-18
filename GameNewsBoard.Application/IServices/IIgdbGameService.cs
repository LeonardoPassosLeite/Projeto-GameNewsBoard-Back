using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Enums;
using GameNewsBoard.Application.Responses.DTOs;

namespace GameNewsBoard.Application.IServices
{
    public interface IIgdbGameService
    {
        Task<PaginatedResult<GameResponse>> GetGamesByPlatformAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<List<GameResponse>> SearchGamesAsync(string? name = null, IgdbPlatform? platform = null, int? year = null, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    }
}