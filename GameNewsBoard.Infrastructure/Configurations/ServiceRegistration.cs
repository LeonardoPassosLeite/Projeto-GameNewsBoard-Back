using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.IServices.Auth;
using GameNewsBoard.Application.IServices.Images;
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
            services.AddScoped<IGameReleaseService, GameReleaseService>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Auth
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthService, AuthService>();

            //TierList
            services.AddScoped<ITierListService, TierListService>();
            services.AddScoped<ITierListRepository, TierListRepository>();

            //Status
            services.AddScoped<IGameStatusService, GameStatusService>();
            services.AddScoped<IGameStatusRepository, GameStatusRepository>();

            services.AddScoped<IUploadedImageService, UploadedImageService>();
            services.AddScoped<IUploadedImageRepository, UploadedImageRepository>();
            services.AddScoped<IPhysicalImageService, PhysicalImageService>();
        }
    }
}