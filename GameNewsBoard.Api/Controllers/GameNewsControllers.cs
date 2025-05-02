using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Api.Models.Responses;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameNewsController : ControllerBase
{
    private readonly IGameNewsService _newsService;
    private readonly ILogger<GameNewsController> _logger;

    public GameNewsController(IGameNewsService newsService, ILogger<GameNewsController> logger)
    {
        _newsService = newsService;
        _logger = logger;
    }

    [HttpGet("{platform}")]
    public async Task<IActionResult> Get(string platform)
    {
        if (string.IsNullOrWhiteSpace(platform))
            return ApiResponseHelper.CreateError("Parâmetro inválido", "O parâmetro 'platform' é obrigatório.", 400);

        try
        {
            var result = await _newsService.GetLatestNewsAsync(platform);

            if (result == null || result.Articles.Count == 0)
                return ApiResponseHelper.CreateError("Notícias não encontradas", $"Nenhuma notícia encontrada para a plataforma '{platform}'.", 404);

            return Ok(new ApiResponse<GameNewsResponse>("Notícias carregadas com sucesso.", result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar notícias para a plataforma {Platform}", platform);
            return ApiResponseHelper.CreateError("Erro interno", $"Erro interno ao buscar notícias: {ex.Message}", 500);
        }
    }
}