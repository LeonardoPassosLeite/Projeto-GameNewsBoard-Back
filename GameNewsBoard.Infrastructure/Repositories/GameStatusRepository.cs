using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure.Repositories
{
    public class GameStatusRepository : IGameStatusRepository
    {
        private readonly AppDbContext _context;

        public GameStatusRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(GameStatus gameStatus)
        {
            await _context.Set<GameStatus>().AddAsync(gameStatus);
        }

        public async Task<GameStatus?> GetByUserAndGameAsync(Guid userId, int gameId)
        {
            return await _context.Set<GameStatus>()
                .Include(sg => sg.Game)
                .FirstOrDefaultAsync(sg => sg.UserId == userId && sg.GameId == gameId);
        }

        public async Task<IEnumerable<GameStatus>> GetByUserAsync(Guid userId)
        {
            return await _context.Set<GameStatus>()
                .Include(sg => sg.Game)
                .Where(sg => sg.UserId == userId)
                .ToListAsync();
        }

        public void Remove(GameStatus gameStatus)
        {
            _context.Set<GameStatus>().Remove(gameStatus);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}