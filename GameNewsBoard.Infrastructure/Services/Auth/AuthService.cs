
using AutoMapper;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GameNewsBoard.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.ExistsByUsernameAsync(request.Username))
                return Result.Failure("Nome de usuário já está em uso.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(request.Username, passwordHash);
            await _userRepository.AddAsync(user);

            _logger.LogInformation("Usuário registrado com sucesso: {Username}", request.Username);
            return Result.Success();
        }

        public async Task<Result<User>> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Result<User>.Failure("Usuário ou senha inválidos.");

            _logger.LogInformation("Usuário autenticado com sucesso: {Username}", request.Username);
            return Result<User>.Success(user);
        }
    }
}