using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Api.Helpers;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Application.Exceptions.Api;
using GameNewsBoard.Application.DTOs.Shared;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameReleaseController : ControllerBase
{
    private readonly IGameReleaseService _gameReleaseService;
    private readonly ILogger<GameReleaseController> _logger;

    public GameReleaseController(IGameReleaseService gameReleaseService, ILogger<GameReleaseController> logger)
    {
        _gameReleaseService = gameReleaseService;
        _logger = logger;
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingReleases([FromQuery] int daysAhead = 7)
    {
        try
        {
            var releases = await _gameReleaseService.GetUpcomingGamesAsync(daysAhead);

            if (releases == null || releases.Count == 0)
                return Ok(ApiResponseHelper.CreateEmptySuccess($"Nenhum jogo será lançado nos próximos {daysAhead} dias."));

            return Ok(ApiResponseHelper.CreateSuccess(releases, $"Jogos que serão lançados nos próximos {daysAhead} dias carregados com sucesso."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lançamentos futuros.");
            return ApiResponseHelper.CreateError("Erro ao buscar lançamentos futuros", ex.Message);
        }
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentReleases([FromQuery] int daysBack = 7)
    {
        try
        {
            var releases = await _gameReleaseService.GetRecentlyReleasedGamesAsync(daysBack);

            if (releases == null || releases.Count == 0)
                return Ok(ApiResponseHelper.CreateEmptySuccess($"Nenhum jogo foi lançado nos últimos {daysBack} dias."));

            return Ok(ApiResponseHelper.CreateSuccess(releases, $"Jogos lançados nos últimos {daysBack} dias carregados com sucesso."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lançamentos recentes.");
            return ApiResponseHelper.CreateError("Erro ao buscar lançamentos recentes", ex.Message);
        }
    }
}
