
using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.IRepository;

public interface ITierListRepository
{
    Task AddAsync(TierList tierList);
    Task AddEntryAsync(TierListEntry entry);
    Task<TierList?> GetByIdAsync(Guid id);
    Task<IEnumerable<TierList>> GetByUserAsync(Guid userId);
    void Remove(TierList tierList);
    Task<bool> ExistsAsync(Guid tierListId);
    Task SaveChangesAsync();

}