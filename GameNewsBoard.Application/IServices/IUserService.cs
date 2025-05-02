using GameNewsBoard.Application.Responses.DTOs.Responses;

namespace GameNewsBoard.Application.IServices
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserProfileAsync(Guid userId);
    }
}