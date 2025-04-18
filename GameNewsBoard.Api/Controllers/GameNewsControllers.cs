using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GameNewsBoard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameNewsController : ControllerBase
    {
        private readonly IGameNewsService _newsService;

        public GameNewsController(IGameNewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("{platform}")]
        public async Task<ActionResult<GameNewsResponse>> Get(string platform)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(platform))
                {
                    return Problem(
                        detail: "O parâmetro 'platform' é obrigatório.",
                        title: "Parâmetro inválido",
                        statusCode: 400
                    );
                }

                var result = await _newsService.GetLatestNewsAsync(platform);

                if (result == null || result.Articles.Count == 0)
                {
                    return Problem(
                        detail: $"Nenhuma notícia encontrada para a plataforma '{platform}'.",
                        title: "Notícias não encontradas",
                        statusCode: 404
                    );
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"Erro interno ao buscar notícias: {ex.Message}",
                    title: "Erro Interno",
                    statusCode: 500
                );
            }
        }
    }
}