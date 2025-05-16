using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameNewsBoard.Infrastructure.Auth;
using GameNewsBoard.Application.IServices.Images;

namespace GameNewsBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TierListController : ControllerBase
    {
        private readonly ITierListService _tierListService;
        private readonly IUploadedImageService _uploadedImageService;
        private readonly ILogger<TierListController> _logger;

        public TierListController(
            ITierListService tierListService,
            IUploadedImageService uploadedImageService,
            ILogger<TierListController> logger)
        {
            _tierListService = tierListService ?? throw new ArgumentNullException(nameof(tierListService));
            _uploadedImageService = uploadedImageService ?? throw new ArgumentNullException(nameof(uploadedImageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TierListRequest request)
        {
            try
            {
                var userId = User.GetUserId();

                var result = await _tierListService.CreateTierListAsync(userId, request);
                if (!result.IsSuccess)
                    return ApiResponseHelper.CreateError("Erro ao criar tier", result.Error);

                if (request.ImageId.HasValue)
                    await _uploadedImageService.MarkImageAsUsedAsync(request.ImageId.Value);

                if (result.IsSuccess)
                    return Ok(ApiResponseHelper.CreateSuccess(request, "Tier criado com sucesso"));

                return ApiResponseHelper.CreateError("Erro inesperado", "Falha ao criar tier.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar tier.");
                return ApiResponseHelper.CreateError("Erro ao criar tier", ex.Message);
            }
        }

        [HttpPut("{tierListId}")]
        public async Task<IActionResult> Update(Guid tierListId, [FromBody] UpdateTierListRequest request)
        {
            try
            {
                var result = await _tierListService.UpdateTierListAsync(tierListId, request);
                if (!result.IsSuccess)
                    return ApiResponseHelper.CreateError("Erro ao atualizar tier", result.Error);

                if (result.IsSuccess)
                    return Ok(ApiResponseHelper.CreateSuccess(request, $"Tier Editado com sucesso: {tierListId}"));

                return ApiResponseHelper.CreateError("Erro inesperado", "Falha ao editar tier.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar tier.");
                return ApiResponseHelper.CreateError("Erro ao atualizar tier", ex.Message);
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

                if (result.IsSuccess)
                    return Ok(ApiResponseHelper.CreateSuccess($"Tier Deletado com sucesso: {tierListId}"));

                return ApiResponseHelper.CreateError("Erro inesperado", "Falha ao deletar tier.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar tier.");
                return ApiResponseHelper.CreateError("Erro ao deletar tier", ex.Message);
            }
        }

        [HttpPut("{tierListId}/entries")]
        public async Task<IActionResult> SetGameTier(Guid tierListId, [FromBody] TierListEntryRequest request)
        {
            try
            {
                var result = await _tierListService.SetGameTierAsync(tierListId, request);
                if (!result.IsSuccess)
                    return ApiResponseHelper.CreateError("Erro ao definir tier do jogo", result.Error);

                var gameId = request.GameId;

                if (result.IsSuccess)
                    return Ok(ApiResponseHelper.CreateSuccess(new { GameId = gameId }, $"Tier do jogo '{gameId}' definido com sucesso"));

                return ApiResponseHelper.CreateError("Erro inesperado", "Falha ao criar a tier.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao definir tier do jogo.");
                return ApiResponseHelper.CreateError("Erro ao definir tier do jogo", ex.Message);
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

                if (result.IsSuccess)
                    return Ok(ApiResponseHelper.CreateSuccess($"Jogo removido do tier com sucesso: {gameId}"));

                return ApiResponseHelper.CreateError("Erro inesperado", "Falha ao criar a tier.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover jogo do tier.");
                return ApiResponseHelper.CreateError("Erro ao remover jogo do tier", ex.Message);
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyTierLists()
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _tierListService.GetTierListsByUserAsync(userId);

                if (!result.IsSuccess)
                    return ApiResponseHelper.CreateError("Erro ao buscar tiers", result.Error);

                return Ok(ApiResponseHelper.CreateSuccess(result.Value, "Tiers carregados com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tiers do usuário.");
                return ApiResponseHelper.CreateError("Erro ao buscar tiers", ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _tierListService.GetTierListByIdAsync(id);

            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao buscar tier", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess(result.Value, "Tier carregado com sucesso"));
        }
    }
}