using GameNewsBoard.Application.IServices.Images;
using Microsoft.AspNetCore.Http;

namespace GameNewsBoard.Infrastructure.Services.Image
{
    public class PhysicalImageService : IPhysicalImageService
    {
        private readonly string _uploadsFolder;

        public PhysicalImageService()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string imageId)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{imageId}_{Path.GetFileNameWithoutExtension(file.FileName)}{fileExtension}";
            var filePath = Path.Combine(_uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public Task DeleteFileAsync(string imageUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}
