using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Repositories;
using GameNewsBoard.Infrastructure.Repositories;
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
        }
    }
}
