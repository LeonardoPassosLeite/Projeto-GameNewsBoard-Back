using GameNewsBoard.Application.IServices;
using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Api.Helpers;
using GameNewsBoard.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameStatusController : ControllerBase
{
    private readonly IGameStatusService _gameStatusService;
    private readonly ILogger<GameStatusController> _logger;

    public GameStatusController(
        IGameStatusService gameStatusService,
        ILogger<GameStatusController> logger)
    {
        _gameStatusService = gameStatusService ?? throw new ArgumentNullException(nameof(gameStatusService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPut("{gameId}/status")]
    [Authorize]
    public async Task<IActionResult> SetGameStatus(int gameId, [FromQuery] Status status)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _gameStatusService.SetGameStatusAsync(userId, gameId, status);

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
    public async Task<IActionResult> RemoveGameStatus(int gameId)
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _gameStatusService.RemoveGameStatusAsync(userId, gameId);

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
    public async Task<IActionResult> GetMyGameStatuses()
    {
        try
        {
            var userId = User.GetUserId();
            var result = await _gameStatusService.GetUserGameStatusesAsync(userId);

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
