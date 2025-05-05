using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Commons;

namespace GameNewsBoard.Application.IServices
{
    public interface ITierListService
    {
        Task<Result> CreateTierListAsync(Guid userId, TierListRequest request);
        Task<Result> UpdateTierListAsync(Guid tierListId, UpdateTierListRequest request);
        Task<Result> DeleteTierListAsync(Guid tierListId);
        Task<Result<IEnumerable<TierListResponse>>> GetTierListsByUserAsync(Guid userId);


        Task<Result> SetGameTierAsync(Guid tierListId, TierListEntryRequest request);
        Task<Result> RemoveGameFromTierAsync(Guid tierListId, int gameId);
        Task<Result<TierListResponse>> GetTierListByIdAsync(Guid tierListId);
    }
}
