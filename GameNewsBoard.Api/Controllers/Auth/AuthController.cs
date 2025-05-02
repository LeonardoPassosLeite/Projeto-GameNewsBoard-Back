using GameNewsBoard.Application.IServices.Auth;
using GameNewsBoard.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using GameNewsBoard.Application.IServices;

namespace GameNewsBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ITokenService tokenService,
        ICookieService cookieService,
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _cookieService = cookieService ?? throw new ArgumentNullException(nameof(cookieService));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.CreateError("Erro de validação", "Dados inválidos para criação do usuário.", 400);

        try
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.IsSuccess)
                return ApiResponseHelper.CreateError("Falha no registro", result.Error, 400);

            return Ok(ApiResponseHelper.CreateSuccess("Usuário registrado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao registrar usuário.");
            return ApiResponseHelper.CreateError("Erro no servidor", "Ocorreu um erro ao registrar o usuário. Tente novamente mais tarde.", 500);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return ApiResponseHelper.CreateError("Erro de validação", "Dados de login inválidos.", 400);

        try
        {
            var result = await _authService.AuthenticateAsync(request);

            if (!result.IsSuccess || result.Value is null)
                return ApiResponseHelper.CreateError("Falha na autenticação", result.Error ?? "Usuário ou senha incorretos.", 401);

            var token = _tokenService.GenerateToken(result.Value);
            _cookieService.SetJwtCookie(Response, token, TimeSpan.FromHours(1));

            return Ok(ApiResponseHelper.CreateSuccess("Login realizado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao fazer login.");
            return ApiResponseHelper.CreateError("Erro no servidor", "Ocorreu um erro ao processar o login. Tente novamente mais tarde.", 500);
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            _cookieService.ClearJwtCookie(Response);
            return Ok(ApiResponseHelper.CreateSuccess("Logout realizado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao fazer logout.");
            return ApiResponseHelper.CreateError("Erro no servidor", "Erro ao encerrar a sessão. Tente novamente mais tarde.", 500);
        }
    }
}