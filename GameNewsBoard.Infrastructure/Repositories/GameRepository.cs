using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Domain.Platforms;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGamesAsync(IEnumerable<Game> games)
        {
            var existingGames = await _context.Games
                .Where(g => games.Select(x => x.Title).Contains(g.Title))
                .ToListAsync();

            var newGames = games.Where(g => !existingGames.Any(eg => eg.Title == g.Title)).ToList();

            if (newGames.Any())
            {
                await _context.Games.AddRangeAsync(newGames);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<(IEnumerable<Game> games, int totalCount)> GetGamesExclusivePlatformAsync(
            int platformId, string? searchTerm, int offset, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Game> query = _context.Games;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var loweredTerm = searchTerm.ToLower();
                query = query.Where(g => g.Title.ToLower().Contains(loweredTerm));
            }

            if (platformId != 0)
            {
                var platform = (Platform)platformId;
                var selectedPlatformName = PlatformMapping.GetPlatformName(platform);

                var lineage = PlatformLineage.PcOrder.GetValueOrDefault(platform)
                             ?? PlatformLineage.MicrosoftOrder.GetValueOrDefault(platform)
                             ?? PlatformLineage.SonyOrder.GetValueOrDefault(platform)
                             ?? PlatformLineage.NintendoOrder.GetValueOrDefault(platform);

                if (lineage != null && lineage.Any())
                {
                    var allGames = await query.ToListAsync(cancellationToken);

                    var filtered = allGames
                        .Where(g =>
                        {
                            var gamePlatforms = g.Platform
                                .Split(',')
                                .Select(p => p.Trim())
                                .ToList();

                            if (platform == Platform.PCMicrosoftWindows)
                                return gamePlatforms.Contains("PC (Microsoft Windows)");

                            var onlyLineagePlatforms = gamePlatforms.All(p => lineage.Contains(p));
                            var hasSelectedPlatform = gamePlatforms.Contains(selectedPlatformName);

                            return onlyLineagePlatforms && hasSelectedPlatform;
                        })
                        .ToList();

                    var totalCount = filtered.Count;
                    var games = filtered
                        .Skip(offset)
                        .Take(pageSize)
                        .ToList();

                    return (games, totalCount);
                }
            }

            return (new List<Game>(), 0);
        }

        public async Task<(IEnumerable<Game> games, int totalCount)> GetGamesByYearCategoryAsync(
            int? startYear, int? endYear, string? searchTerm, int offset, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Game> query = _context.Games;

            if (startYear.HasValue && endYear.HasValue)
                query = query.Where(g => g.Released.Year >= startYear.Value && g.Released.Year <= endYear.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var loweredTerm = searchTerm.ToLower();
                query = query.Where(g => g.Title.ToLower().Contains(loweredTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var games = await query.Skip(offset).Take(pageSize).ToListAsync(cancellationToken);

            return (games, totalCount);
        }

        public async Task<List<Game>> GetByTitlesAsync(List<string> titles)
        {
            return await _context.Games
                .Where(g => titles.Contains(g.Title.ToLower()))
                .ToListAsync();
        }
    }
}
