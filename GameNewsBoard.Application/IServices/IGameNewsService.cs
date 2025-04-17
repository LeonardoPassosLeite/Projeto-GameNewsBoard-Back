using GameNewsBoard.Application.DTOs;

namespace GameNewsBoard.Application.IServices
{
    public interface IGameNewsService
    {
        Task<GameNewsResponse> GetLatestNewsAsync(string platform);
    }
}
