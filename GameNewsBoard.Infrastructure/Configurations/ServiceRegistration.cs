using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.IServices.Auth;
using GameNewsBoard.Infrastructure.Auth;
using GameNewsBoard.Infrastructure.Repositories;
using GameNewsBoard.Infrastructure.Services.Auth;
using GameNewsBoard.Infrastructure.Services.Image;
using Microsoft.Extensions.DependencyInjection;

namespace GameNewsBoard.Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IGameNewsService, GameNewsService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Serviços de autenticação
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthService, AuthService>();

            // Serviço de TierList
            services.AddScoped<ITierListService, TierListService>();
            services.AddScoped<ITierListRepository, TierListRepository>();

            services.AddScoped<IUploadedImageService, UploadedImageService>();
            services.AddScoped<IUploadedImageRepository, UploadedImageRepository>();
            services.AddScoped<PhysicalImageService>();
        }
    }
}