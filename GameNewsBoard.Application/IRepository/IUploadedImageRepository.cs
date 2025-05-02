using GameNewsBoard.Domain.Entities;

public interface IUploadedImageRepository
{
    Task AddAsync(UploadedImage image);
    Task<UploadedImage?> GetByIdAsync(Guid id);
    Task<IEnumerable<UploadedImage>> GetUnusedByUserAsync(Guid userId);
    Task SaveChangesAsync();
}