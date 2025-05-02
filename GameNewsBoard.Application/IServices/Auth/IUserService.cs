using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;

public interface IAuthService
{
    Task<Result<User>> AuthenticateAsync(LoginRequest request);
    Task<Result> RegisterAsync(RegisterRequest request);
}