using Microsoft.AspNetCore.Http;

namespace GameNewsBoard.Application.IServices.Images
{
    public interface IPhysicalImageService
    {
        Task<string> SaveFileAsync(IFormFile file, string imageId);
        Task DeleteFileAsync(string imageUrl);
    }
}