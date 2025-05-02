using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure.Repositories
{
    public class StatusGameRepository : IStatusGameRepository
    {
        private readonly AppDbContext _context;

        public StatusGameRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(StatusGame statusGame)
        {
            await _context.Set<StatusGame>().AddAsync(statusGame);
        }

        public async Task<StatusGame?> GetByUserAndGameAsync(Guid userId, int gameId)
        {
            return await _context.Set<StatusGame>()
                .Include(sg => sg.Game)
                .FirstOrDefaultAsync(sg => sg.UserId == userId && sg.GameId == gameId);
        }

        public async Task<IEnumerable<StatusGame>> GetByUserAsync(Guid userId)
        {
            return await _context.Set<StatusGame>()
                .Include(sg => sg.Game)
                .Where(sg => sg.UserId == userId)
                .ToListAsync();
        }

        public void Remove(StatusGame statusGame)
        {
            _context.Set<StatusGame>().Remove(statusGame);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}