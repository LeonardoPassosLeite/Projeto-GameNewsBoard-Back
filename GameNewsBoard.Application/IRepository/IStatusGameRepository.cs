using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.IRepository
{
    public interface IStatusGameRepository
    {
        Task AddAsync(StatusGame statusGame);
        Task<StatusGame?> GetByUserAndGameAsync(Guid userId, int gameId);
        Task<IEnumerable<StatusGame>> GetByUserAsync(Guid userId);
        void Remove(StatusGame statusGame);
        Task SaveChangesAsync();
    }
}