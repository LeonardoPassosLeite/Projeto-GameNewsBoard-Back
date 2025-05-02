using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.IRepository
{
    public interface IGameRepository
    {
        Task AddGamesAsync(IEnumerable<Game> games);
        Task<Game?> GetByIdAsync(int id);
        Task<(IEnumerable<Game> games, int totalCount)> GetGamesExclusivePlatformAsync(int platformId, string? searchTerm, int offset, int pageSize, CancellationToken cancellationToken);
        Task<(IEnumerable<Game> games, int totalCount)> GetGamesByYearCategoryAsync(int? startYear, int? endYear, string? searchTerm, int offset, int pageSize, CancellationToken cancellationToken);
        Task<List<Game>> GetByTitlesAsync(List<string> titles);
    }
}