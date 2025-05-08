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
        _logger.LogInformation("Requisição recebida para a plataforma: {Platform}", platform);

        if (string.IsNullOrWhiteSpace(platform))
        {
            _logger.LogWarning("Parâmetro inválido: O parâmetro 'platform' não foi fornecido.");
            return ApiResponseHelper.CreateError("Parâmetro inválido", "O parâmetro 'platform' é obrigatório.", 400);
        }

        try
        {
            var result = await _newsService.GetLatestNewsAsync(platform);
            _logger.LogInformation("Notícias para a plataforma {Platform} carregadas com sucesso", platform);

            if (result == null || result.Articles.Count == 0)
            {
                _logger.LogWarning("Nenhuma notícia encontrada para a plataforma {Platform}", platform);
                return ApiResponseHelper.CreateError("Notícias não encontradas", $"Nenhuma notícia encontrada para a plataforma '{platform}'.", 404);
            }

            _logger.LogInformation("Notícias carregadas com sucesso para a plataforma {Platform}", platform);
            return Ok(new ApiResponse<GameNewsResponse>("Notícias carregadas com sucesso.", result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar notícias para a plataforma {Platform}", platform);
            return ApiResponseHelper.CreateError("Erro interno", $"Erro interno ao buscar notícias: {ex.Message}", 500);
        }
    }
}