using GameNewsBoard.Application.IServices;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Infrastructure.Services.Image;

public class UploadedImageService : IUploadedImageService
{
    private readonly IUploadedImageRepository _repository;

    public UploadedImageService(IUploadedImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UploadedImageDto>> RegisterImageAsync(Guid userId, string url)
    {
        var image = new UploadedImage
        {
            UserId = userId,
            Url = url
        };

        await _repository.AddAsync(image);
        await _repository.SaveChangesAsync();

        return Result<UploadedImageDto>.Success(new UploadedImageDto(image.Id, image.Url, image.IsUsed));
    }

    public async Task MarkImageAsUsedAsync(Guid imageId)
    {
        var image = await _repository.GetByIdAsync(imageId);
        if (image != null)
        {
            image.IsUsed = true;
            await _repository.SaveChangesAsync();
        }
    }
}
