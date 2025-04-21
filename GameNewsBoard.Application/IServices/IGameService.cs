using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.DTOs;

namespace GameNewsBoard.Application.IServices
{
  public interface IGameService
  {
    Task<PaginatedFromApiResult<GameResponse>> GetPaginedGamesAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task SaveGamesAsync(int batchSize = 500, CancellationToken cancellationToken = default);
    Task<PaginatedResult<GameDTO>> GetGameExclusiveByPlatformAsync(Platform? platform, string? searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<PaginatedResult<GameDTO>> GetGamesByYearCategoryAsync(YearCategory? yearCategory, string? searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
  }
}
