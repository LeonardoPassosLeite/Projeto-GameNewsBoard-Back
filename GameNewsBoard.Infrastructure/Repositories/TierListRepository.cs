using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure.Repositories
{
    public class TierListRepository : ITierListRepository
    {
        private readonly AppDbContext _context;

        public TierListRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(TierList tierList)
        {
            await _context.TierLists.AddAsync(tierList);
        }

        public async Task AddEntryAsync(TierListEntry entry)
        {
            await _context.Set<TierListEntry>().AddAsync(entry);
        }

        public async Task<TierList?> GetByIdAsync(Guid id)
        {
            return await _context.TierLists
                .Include(t => t.Entries)
                    .ThenInclude(e => e.Game)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<(TierList Tier, Guid? ImageId)?> GetTierWithImageIdAsync(Guid tierListId)
        {
            return await _context.TierLists
                .Where(t => t.Id == tierListId)
                .Select(t => new ValueTuple<TierList, Guid?>(t, t.ImageId))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TierList>> GetByUserAsync(Guid userId)
        {
            return await _context.TierLists
                .Include(t => t.Entries)
                    .ThenInclude(e => e.Game)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public void Remove(TierList tierList)
        {
            _context.TierLists.Remove(tierList);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
