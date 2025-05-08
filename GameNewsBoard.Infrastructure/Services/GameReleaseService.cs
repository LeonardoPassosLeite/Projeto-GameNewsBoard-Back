using AutoMapper;
using GameNewsBoard.Application.Exceptions.Api;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Infrastructure.Commons;
using GameNewsBoard.Infrastructure.Configurations;
using GameNewsBoard.Infrastructure.ExternalDtos;
using GameNewsBoard.Infrastructure.Queries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace GameNewsBoard.Infrastructure.Services;

public class GameReleaseService : IgdbApiBaseService, IGameReleaseService
{
    private readonly IMapper _mapper;
    private readonly ILogger<GameReleaseService> _logger;

    public GameReleaseService(HttpClient httpClient,
                              IOptions<IgdbSettings> igdbOptions,
                              IMapper mapper,
                              ILogger<GameReleaseService> logger)
        : base(httpClient,
               igdbOptions.Value.ClientId,
               igdbOptions.Value.AccessToken)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<GameReleaseResponse>> GetUpcomingGamesAsync(int daysAhead = 7, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var future = now.AddDays(daysAhead);

        return await GetReleasesBetweenAsync(now, future, cancellationToken);
    }

    public async Task<List<GameReleaseResponse>> GetRecentlyReleasedGamesAsync(int daysBack = 7, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var past = now.AddDays(-daysBack);

        return await GetReleasesBetweenAsync(past, now, cancellationToken);
    }

    public async Task<List<GameReleaseResponse>> GetReleasesBetweenAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        var startUnix = new DateTimeOffset(start).ToUnixTimeSeconds();
        var endUnix = new DateTimeOffset(end).ToUnixTimeSeconds();

        var query = ExternalApiQueryStore.Igdb.GenerateReleasesBetweenQuery(startUnix, endUnix);
        var request = CreateIgdbRequest(query, "release_dates");

        try
        {
            var igdbGames = await SendIgdbRequestAsync<IgdbGameReleaseWrapperDto>(request, cancellationToken);
            return _mapper.Map<List<GameReleaseResponse>>(igdbGames);
        }
        catch (IgdbApiException ex)
        {
            _logger.LogError(ex, "Erro ao se comunicar com a IGDB.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar lançamentos de jogos.");
            throw;
        }
    }
}
