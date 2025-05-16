using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.IRepository
{
    public interface IGameStatusRepository
    {
        Task AddAsync(GameStatus gameStatus);
        Task<GameStatus?> GetByUserAndGameAsync(Guid userId, int gameId);
        Task<IEnumerable<GameStatus>> GetByUserAsync(Guid userId);
        void Remove(GameStatus gameStatus);
        Task SaveChangesAsync();
    }
}