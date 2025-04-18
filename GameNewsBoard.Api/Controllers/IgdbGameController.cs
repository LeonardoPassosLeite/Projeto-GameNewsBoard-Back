using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Helpers;
using GameNewsBoard.Application.Enums;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Responses.DTOs;

namespace GameNewsBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IIgdbGameService _gameService;

        public GamesController(IIgdbGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationQuery pagination)
        {
            try
            {
                var result = await _gameService.GetGamesByPlatformAsync(pagination.Page, pagination.PageSize);

                if (result == null || result.Items.Count == 0)
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        "Nenhum jogo foi retornado da IGDB."));
                
                return Ok(ApiResponseHelper.CreatePaginatedSuccess(
                    result.Items,
                    pagination.Page,
                    pagination.PageSize,
                    "Jogos carregados com sucesso."));
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

        [HttpGet("search")]
        public async Task<IActionResult> Search(
                [FromQuery] PaginationQuery pagination,
                [FromQuery] string? name,
                [FromQuery] IgdbPlatform? platform = null,
                [FromQuery] int? year = null)
        {
            try
            {
                var result = await _gameService.SearchGamesAsync(name, platform, year, pagination.Page, pagination.PageSize);

                // Se jogos foram encontrados, retorna a resposta com os dados
                if (result.Any())
                    return Ok(ApiResponseHelper.CreatePaginatedSuccess(
                        result,
                        pagination.Page,
                        pagination.PageSize,
                        "Jogos encontrados com base nos critérios."));


                // Se o filtro de ano não retornar resultados, enviar mensagem para o usuário
                if (year.HasValue)
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        $"Nenhum jogo encontrado para o ano {year.Value}."));


                // Mensagem específica para o caso de não encontrar jogos com o nome e plataforma
                if (!string.IsNullOrWhiteSpace(name) && platform != null)
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        "O jogo não está disponível na plataforma selecionada."));


                // Mensagem específica para o caso de não encontrar jogos com o nome
                if (!string.IsNullOrWhiteSpace(name))
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        "Nenhum jogo encontrado com esse nome."));


                // Mensagem específica para o caso de não encontrar jogos com a plataforma
                if (platform != null)
                    return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                        pagination.Page,
                        pagination.PageSize,
                        "Nenhum jogo encontrado para a plataforma selecionada."));


                // Mensagem padrão para quando não encontrar nenhum jogo
                return Ok(ApiResponseHelper.CreateEmptyPaginated<GameResponse>(
                    pagination.Page,
                    pagination.PageSize,
                    "Nenhum jogo encontrado."));
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Erro ao buscar jogos: {ex.Message}",
                    title: "Erro Interno",
                    statusCode: 500
                );
            }
        }
    }
}