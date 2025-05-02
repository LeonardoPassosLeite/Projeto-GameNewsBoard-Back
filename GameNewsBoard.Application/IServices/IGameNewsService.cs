using GameNewsBoard.Application.Responses.DTOs.Responses;

namespace GameNewsBoard.Application.IServices
{
    public interface IGameNewsService
    {
        Task<GameNewsResponse> GetLatestNewsAsync(string platform);
    }
}
