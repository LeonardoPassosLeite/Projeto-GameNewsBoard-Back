using GameNewsBoard.Domain.Commons;

namespace GameNewsBoard.Application.IServices.Images
{
    public interface IUploadedImageService
    {
        Task<Result<UploadedImageDto>> RegisterImageAsync(Guid userId, string url);
        Task MarkImageAsUsedAsync(Guid imageId);
        Task<Result> DeleteImageAsync(Guid userId, Guid imageId);
    }
}