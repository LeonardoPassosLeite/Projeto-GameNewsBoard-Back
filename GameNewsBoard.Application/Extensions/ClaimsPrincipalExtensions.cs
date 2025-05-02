using System.Security.Claims;

namespace GameNewsBoard.Infrastructure.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            return Guid.Parse(userId);
        }
    }
}