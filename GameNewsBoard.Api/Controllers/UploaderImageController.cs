using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameNewsBoard.Infrastructure.Auth;
using GameNewsBoard.Application.IServices.Images;

namespace GameNewsBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadedImageController : ControllerBase
    {
        private readonly IUploadedImageService _uploadedImageService;
        private readonly IPhysicalImageService _physicalImageService;
        private readonly ILogger<UploadedImageController> _logger;

        public UploadedImageController(
            IUploadedImageService uploadedImageService,
            IPhysicalImageService physicalImageService,
            ILogger<UploadedImageController> logger)
        {
            _uploadedImageService = uploadedImageService ?? throw new ArgumentNullException(nameof(uploadedImageService));
            _physicalImageService = physicalImageService ?? throw new ArgumentNullException(nameof(physicalImageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            var image = request.Image;

            if (image == null || image.Length == 0)
                return BadRequest(ApiResponseHelper.CreateError("Imagem ausente", "Imagem não fornecida."));

            if (image.Length > 5 * 1024 * 1024)
                return BadRequest(ApiResponseHelper.CreateError("Imagem muito grande", "Imagem excede o tamanho máximo de 5MB."));

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(image.ContentType))
                return BadRequest(ApiResponseHelper.CreateError("Tipo inválido", "Tipo de imagem não permitido."));

            var userId = User.GetUserId();
            var imageId = Guid.NewGuid();
            var imageUrl = await _physicalImageService.SaveFileAsync(image, imageId.ToString());

            var result = await _uploadedImageService.RegisterImageAsync(userId, imageUrl);

            if (!result.IsSuccess || result.Value is null)
                return ApiResponseHelper.CreateError("Falha no upload", result.Error ?? "Erro ao registrar a imagem.");

            return Ok(ApiResponseHelper.CreateSuccess(new
            {
                imageId = result.Value.Id,
                imageUrl = result.Value.Url
            }, "Imagem enviada com sucesso"));
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var userId = User.GetUserId();

            var result = await _uploadedImageService.DeleteImageAsync(userId, id);

            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao deletar imagem", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Imagem deletada com sucesso"));
        }
    }
}
