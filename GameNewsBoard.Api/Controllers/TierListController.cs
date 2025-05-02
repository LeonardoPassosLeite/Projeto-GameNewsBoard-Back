using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameNewsBoard.Infrastructure.Auth;
using GameNewsBoard.Infrastructure.Services.Image;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TierListController : ControllerBase
{
    private readonly ITierListService _tierListService;
    private readonly IUploadedImageService _uploadedImageService;
    private readonly PhysicalImageService _physicalImageService;
    private readonly ILogger<TierListController> _logger;

    public TierListController(
        ITierListService tierListService,
        IUploadedImageService uploadedImageService,
        PhysicalImageService physicalImageService,
        ILogger<TierListController> logger)
    {
        _tierListService = tierListService ?? throw new ArgumentNullException(nameof(tierListService));
        _uploadedImageService = uploadedImageService ?? throw new ArgumentNullException(nameof(uploadedImageService));
        _physicalImageService = physicalImageService ?? throw new ArgumentNullException(nameof(physicalImageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("upload-image")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
    {
        var image = request.Image;

        if (image == null || image.Length == 0)
            return BadRequest(ApiResponseHelper.CreateError("Imagem ausente", "Imagem não fornecida."));

        if (image.Length > 5 * 1024 * 1024)
            return BadRequest(ApiResponseHelper.CreateError("Imagem muito grande", "Imagem excede o tamanho máximo de 5MB."));

        var allowedTypes = new[] { "image/jpeg", "image/png" };
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] TierListRequest request)
    {
        try
        {
            var result = await _tierListService.CreateTierListAsync(request);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao criar tier", result.Error);

            if (request.ImageId.HasValue)
                await _uploadedImageService.MarkImageAsUsedAsync(request.ImageId.Value);

            return Ok(ApiResponseHelper.CreateSuccess("Tier criado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar tier.");
            return ApiResponseHelper.CreateError("Erro ao criar tier", ex.Message);
        }
    }

    [HttpPost("{tierListId}/add-game")]
    public async Task<IActionResult> AddGame(Guid tierListId, [FromQuery] int gameId, [FromQuery] TierLevel tier)
    {
        try
        {
            var result = await _tierListService.AddGameToTierAsync(tierListId, gameId, tier);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao adicionar jogo ao tier", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Jogo adicionado ao tier com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar jogo ao tier.");
            return ApiResponseHelper.CreateError("Erro ao adicionar jogo ao tier", ex.Message);
        }
    }

    [HttpPut("{tierListId}")]
    public async Task<IActionResult> Update(Guid tierListId, [FromBody] UpdateTierListRequest request)
    {
        try
        {
            var result = await _tierListService.UpdateTierListAsync(tierListId, request.NewTitle, request.NewImage);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao atualizar tier", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Tier atualizado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar tier.");
            return ApiResponseHelper.CreateError("Erro ao atualizar tier", ex.Message);
        }
    }

    [HttpPatch("{tierListId}/update-game")]
    public async Task<IActionResult> UpdateGameTier(Guid tierListId, [FromQuery] int gameId, [FromQuery] TierLevel newTier)
    {
        try
        {
            var result = await _tierListService.UpdateGameTierAsync(tierListId, gameId, newTier);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao atualizar tier do jogo", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Tier do jogo atualizado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar tier do jogo.");
            return ApiResponseHelper.CreateError("Erro ao atualizar tier do jogo", ex.Message);
        }
    }

    [HttpDelete("{tierListId}")]
    public async Task<IActionResult> Delete(Guid tierListId)
    {
        try
        {
            var result = await _tierListService.DeleteTierListAsync(tierListId);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao deletar tier", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Tier deletado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar tier.");
            return ApiResponseHelper.CreateError("Erro ao deletar tier", ex.Message);
        }
    }

    [HttpDelete("{tierListId}/remove-game")]
    public async Task<IActionResult> RemoveGame(Guid tierListId, [FromQuery] int gameId)
    {
        try
        {
            var result = await _tierListService.RemoveGameFromTierAsync(tierListId, gameId);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao remover jogo do tier", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Jogo removido do tier com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover jogo do tier.");
            return ApiResponseHelper.CreateError("Erro ao remover jogo do tier", ex.Message);
        }
    }

    [HttpGet("{tierListId}")]
    public async Task<IActionResult> GetTierList(Guid tierListId)
    {
        try
        {
            var result = await _tierListService.GetTierListByIdAsync(tierListId);

            if (!result.IsSuccess || result.Value == null)
                return ApiResponseHelper.CreateError("Erro ao buscar tier", result.Error ?? "Tier não encontrado.");

            return Ok(ApiResponseHelper.CreateSuccess(result.Value, "Tier carregado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar tier por ID.");
            return ApiResponseHelper.CreateError("Erro ao buscar tier", ex.Message);
        }
    }
}