using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GameNewsBoard.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Obtém o caminho do arquivo appsettings.json
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Configuração do caminho para appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Define o caminho onde o arquivo appsettings.json está localizado
                .AddJsonFile("appsettings.json") // Adiciona o arquivo appsettings.json
                .Build();

            // Configura o DbContext com a string de conexão
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
