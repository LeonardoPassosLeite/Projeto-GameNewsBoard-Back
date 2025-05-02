using GameNewsBoard.Application.IServices;
using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Api.Helpers;
using GameNewsBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusGameController : ControllerBase
{
    private readonly IStatusGameService _statusGameService;
    private readonly ILogger<StatusGameController> _logger;

    public StatusGameController(
        IStatusGameService statusGameService,
        ILogger<StatusGameController> logger)
    {
        _statusGameService = statusGameService ?? throw new ArgumentNullException(nameof(statusGameService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPut("{gameId}/status")]
    [Authorize]
    public async Task<IActionResult> SetStatus(int gameId, [FromQuery] Status status)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _statusGameService.SetStatusAsync(userId, gameId, status);

            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao definir status", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Status definido com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao definir status para o jogo.");
            return ApiResponseHelper.CreateError("Erro ao definir status", ex.Message);
        }
    }

    [HttpDelete("{gameId}/status")]
    [Authorize]
    public async Task<IActionResult> RemoveStatus(int gameId)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _statusGameService.RemoveStatusAsync(userId, gameId);

            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao remover status", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess("Status removido com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover status do jogo.");
            return ApiResponseHelper.CreateError("Erro ao remover status", ex.Message);
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyStatuses()
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _statusGameService.GetUserGameStatusesAsync(userId);

            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Erro ao buscar status", result.Error);

            return Ok(ApiResponseHelper.CreateSuccess(result.Value, "Status carregados com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar status dos jogos do usuário.");
            return ApiResponseHelper.CreateError("Erro ao buscar status", ex.Message);
        }
    }
}
