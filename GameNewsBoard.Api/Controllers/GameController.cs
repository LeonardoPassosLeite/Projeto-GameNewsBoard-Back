using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Api.Helpers;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(IGameService gameService, ILogger<GamesController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery pagination)
    {
        try
        {
            var result = await _gameService.GetPaginedGamesAsync(pagination.Page, pagination.PageSize);

            if (result == null || result.Items.Count == 0)
                return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(pagination.Page, pagination.PageSize, "Nenhum jogo foi retornado da IGDB."));

            return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameResponse>(
                result.Items, pagination.Page, pagination.PageSize,
                "Jogos carregados com sucesso."
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar jogos.");
            return ApiResponseHelper.CreateError("Erro interno ao buscar jogos", ex.Message);
        }
    }

    [HttpGet("get-games-by-platform")]
    public async Task<IActionResult> GetGamesByPlatform([FromQuery] PaginationQuery pagination, [FromQuery] Platform platform, [FromQuery] string searchTerm = "")
    {
        try
        {
            var platformName = platform.ToString();
            _logger.LogInformation("Buscando jogos para a plataforma: {PlatformName}", platformName);

            var result = await _gameService.GetGameExclusiveByPlatformAsync(platform, searchTerm, pagination.Page, pagination.PageSize);

            if (result == null || !result.Items.Any())
                return Ok(ApiResponseHelper.CreateEmptyPaginated<GameDTO>(pagination.Page, pagination.PageSize, $"Nenhum jogo encontrado para a plataforma {platformName}."));

            return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameDTO>(result.Items, pagination.Page, pagination.PageSize, $"Jogos da plataforma {platformName} carregados com sucesso.", result.TotalCount, result.TotalPages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar jogos pela plataforma {Platform}", platform);
            return ApiResponseHelper.CreateError($"Erro ao buscar jogos pela plataforma {platform}", ex.Message);
        }
    }

    [HttpGet("get-games-by-year-category")]
    public async Task<IActionResult> GetGamesByYearCategory([FromQuery] PaginationQuery pagination, [FromQuery] YearCategory yearCategory = YearCategory.All, [FromQuery] string searchTerm = "")
    {
        try
        {
            var result = await _gameService.GetGamesByYearCategoryAsync(yearCategory, searchTerm, pagination.Page, pagination.PageSize);

            if (result == null || !result.Items.Any())
                return Ok(ApiResponseHelper.CreateEmptyPaginated<GameDTO>(pagination.Page, pagination.PageSize, "Nenhum jogo encontrado para a categoria de ano especificada."));

            return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameDTO>(result.Items, pagination.Page, pagination.PageSize, "Jogos carregados com sucesso.", result.TotalCount, result.TotalPages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar jogos por categoria de ano.");
            return ApiResponseHelper.CreateError("Erro ao buscar jogos por categoria de ano", ex.Message);
        }
    }

    [HttpPost("save-games")]
    public async Task<IActionResult> Create()
    {
        try
        {
            await _gameService.SaveGamesAsync(500);
            return Ok(ApiResponseHelper.CreateSuccess(message: "Jogos carregados e salvos com sucesso."));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Erro de aplicação ao salvar jogos.");
            return ApiResponseHelper.CreateError("Erro na aplicação", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao salvar jogos. Stack: {StackTrace}, Inner: {InnerException}", ex.StackTrace, ex.InnerException?.ToString());
            return ApiResponseHelper.CreateError("Erro inesperado ao salvar jogos", ex.Message);
        }
    }
}