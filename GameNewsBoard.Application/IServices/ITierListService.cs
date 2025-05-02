using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.IServices
{
    public interface ITierListService
    {
        Task<Result> CreateTierListAsync(TierListRequest request);
        Task<Result> AddGameToTierAsync(Guid tierListId, int gameId, TierLevel tier);
        Task<Result> UpdateTierListAsync(Guid tierListId, string? newTitle = null, string? newImage = null);
        Task<Result> UpdateGameTierAsync(Guid tierListId, int gameId, TierLevel newTier);
        Task<Result> DeleteTierListAsync(Guid tierListId);
        Task<Result> RemoveGameFromTierAsync(Guid tierListId, int gameId);
        Task<Result<TierListResponse>> GetTierListByIdAsync(Guid tierListId);
    }
}
