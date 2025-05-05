using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Application.IServices.Images;

namespace GameNewsBoard.Infrastructure.Services.Image
{
    public class UploadedImageService : IUploadedImageService
    {
        private readonly IUploadedImageRepository _uploadedImageRepository;
        private readonly IPhysicalImageService _physicalImageService;

        public UploadedImageService(
            IUploadedImageRepository uploadedImageRepository,
            IPhysicalImageService physicalImageService)
        {
            _uploadedImageRepository = uploadedImageRepository ?? throw new ArgumentNullException(nameof(uploadedImageRepository));
            _physicalImageService = physicalImageService ?? throw new ArgumentNullException(nameof(physicalImageService));
        }

        public async Task<Result<UploadedImageDto>> RegisterImageAsync(Guid userId, string url)
        {
            var image = new UploadedImage
            {
                UserId = userId,
                Url = url
            };

            await _uploadedImageRepository.AddAsync(image);
            await _uploadedImageRepository.SaveChangesAsync();

            return Result<UploadedImageDto>.Success(new UploadedImageDto(image.Id, image.Url, image.IsUsed));
        }

        public async Task MarkImageAsUsedAsync(Guid imageId)
        {
            var image = await _uploadedImageRepository.GetByIdAsync(imageId);
            if (image != null)
            {
                image.IsUsed = true;
                await _uploadedImageRepository.SaveChangesAsync();
            }
        }

        public async Task<Result> DeleteImageAsync(Guid userId, Guid imageId)
        {
            var image = await _uploadedImageRepository.GetByIdAsync(imageId);

            if (image == null)
                return Result.Failure("Imagem não encontrada.");

            if (image.UserId != userId)
                return Result.Failure("Você não tem permissão para deletar esta imagem.");

            if (image.IsUsed)
                return Result.Failure("Imagem já foi utilizada e não pode ser removida.");

            await _physicalImageService.DeleteFileAsync(image.Url);

            await _uploadedImageRepository.DeleteAsync(image);
            await _uploadedImageRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}