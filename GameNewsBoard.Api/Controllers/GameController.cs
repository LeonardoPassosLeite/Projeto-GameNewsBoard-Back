using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Api.Helpers;

namespace GameNewsBoard.Api.Controllers
{
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
                {
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        "Nenhum jogo foi retornado da IGDB."
                    ));
                }

                return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameResponse>(
                    result.Items,
                    pagination.Page,
                    pagination.PageSize,
                    "Jogos carregados com sucesso."
                ));
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Erro interno ao buscar jogos: {ex.Message}",
                    title: "Erro Interno",
                    statusCode: 500
                );
            }
        }

        [HttpGet("get-games-by-platform")]
        public async Task<IActionResult> GetGamesByPlatform(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] Platform platform,
            [FromQuery] string searchTerm = "")
        {
            try
            {
                var platformName = platform.ToString();
                _logger.LogInformation($"Buscando jogos para a plataforma: {platformName}"); // Log para verificar

                var result = await _gameService.GetGameExclusiveByPlatformAsync(
                    platform, searchTerm, pagination.Page, pagination.PageSize);

                if (result == null || !result.Items.Any())
                {
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameDTO>(
                        pagination.Page,
                        pagination.PageSize,
                        $"Nenhum jogo encontrado para a plataforma {platformName}."
                    ));
                }

                return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameDTO>(
                    result.Items,
                    pagination.Page,
                    pagination.PageSize,
                    $"Jogos da plataforma {platformName} carregados com sucesso.",
                    result.TotalCount,
                    result.TotalPages
                ));
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Erro ao buscar jogos pela plataforma {platform}: {ex.Message}",
                    title: "Erro Interno",
                    statusCode: 500
                );
            }
        }

        [HttpGet("get-games-by-year-category")]
        public async Task<IActionResult> GetGamesByYearCategory(
            [FromQuery] PaginationQuery pagination,
            [FromQuery] YearCategory yearCategory = YearCategory.All,
            [FromQuery] string searchTerm = ""
        )
        {
            try
            {

                var result = await _gameService.GetGamesByYearCategoryAsync(
                    yearCategory, searchTerm, pagination.Page, pagination.PageSize);

                if (result == null || !result.Items.Any())
                {
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameDTO>(
                        pagination.Page,
                        pagination.PageSize,
                        "No games found for the specified year category."
                    ));
                }

                return Ok(ApiResponseHelper.CreatePaginatedSuccess<GameDTO>(
                    result.Items,
                    pagination.Page,
                    pagination.PageSize,
                    "Games loaded successfully.",
                    result.TotalCount,
                    result.TotalPages
                ));
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Error fetching games by year category: {ex.Message}",
                    title: "Internal Error",
                    statusCode: 500
                );
            }
        }

        [HttpPost("save-games")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            try
            {
                await _gameService.SaveGamesAsync(500);

                return Ok(new
                {
                    message = "Jogos carregados e salvos com sucesso."
                });
            }
            catch (ApplicationException ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Erro na aplicação",
                    statusCode: 500
                );
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Erro inesperado ao salvar jogos: {ex.Message}",
                    title: "Erro Interno",
                    statusCode: 500
                );
            }
        }

    }
}
