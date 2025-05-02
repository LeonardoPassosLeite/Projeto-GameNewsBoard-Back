using GameNewsBoard.Application.IServices.Auth;
using Microsoft.AspNetCore.Http;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public JwtMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["jwtToken"];

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine("[JwtMiddleware] jwtToken encontrado no cookie");
            var principal = _tokenService.ValidateToken(token);

            if (principal != null)
            {
                Console.WriteLine("[JwtMiddleware] Token válido. Usuário autenticado.");
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"[JwtMiddleware] Claim: {claim.Type} = {claim.Value}");
                }

                context.User = principal;
            }
            else
            {
                Console.WriteLine("[JwtMiddleware] Token inválido.");
            }
        }
        else
        {
            Console.WriteLine("[JwtMiddleware] Nenhum jwtToken encontrado no cookie");
        }

        await _next(context);
    }
}