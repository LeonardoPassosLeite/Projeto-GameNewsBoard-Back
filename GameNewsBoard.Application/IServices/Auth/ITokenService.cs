
using System.Security.Claims;
using GameNewsBoard.Domain.Entities;

namespace GameNewsBoard.Application.IServices.Auth;
public interface ITokenService
{
    string GenerateToken(User user, TimeSpan? expiration = null);
    ClaimsPrincipal? ValidateToken(string token);
}
