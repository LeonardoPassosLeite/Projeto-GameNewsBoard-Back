using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Api.Helpers;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using System.Security.Claims;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetUser()
    {
        try
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(userId))
                return ApiResponseHelper.CreateError("Usuário não autenticado", "Não foi possível identificar o usuário atual.", 401);

            var result = new UserResponse(true, username, userId);

            return Ok(ApiResponseHelper.CreateSuccess(result, "Sessão válida"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar dados do usuário.");
            return ApiResponseHelper.CreateError("Erro no servidor", "Erro ao validar a sessão do usuário. Tente novamente mais tarde.", 500);
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            Console.WriteLine("[/profile] Claims recebidas:");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Type: {claim.Type} | Value: {claim.Value}");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("[/profile] userId extraído: " + userId);

            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                Console.WriteLine("[/profile] Falha na extração do userId");
                return ApiResponseHelper.CreateError("Usuário inválido", "Não foi possível identificar o usuário.", 401);
            }

            var profile = await _userService.GetUserProfileAsync(userGuid);

            return Ok(ApiResponseHelper.CreateSuccess(profile, "Perfil do usuário carregado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar perfil do usuário.");
            return ApiResponseHelper.CreateError("Erro ao buscar perfil", ex.Message);
        }
    }

}
