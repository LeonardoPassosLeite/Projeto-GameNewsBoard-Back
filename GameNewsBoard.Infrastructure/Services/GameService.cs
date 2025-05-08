using AutoMapper;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Exceptions;
using GameNewsBoard.Application.Exceptions.Api;
using GameNewsBoard.Application.Exceptions.Domain;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Validators;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Domain.Enums;
using GameNewsBoard.Infrastructure.Commons;
using GameNewsBoard.Infrastructure.Configurations;
using GameNewsBoard.Infrastructure.ExternalDtos;
using GameNewsBoard.Infrastructure.Queries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameNewsBoard.Infrastructure.Services;

public class GameService : IgdbApiBaseService, IGameService
{
    private readonly IMapper _mapper;
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GameService> _logger;

    public GameService(HttpClient httpClient,
                       IOptions<IgdbSettings> igdbOptions,
                       IMapper mapper,
                       IGameRepository gameRepository,
                       ILogger<GameService> logger)
        : base(httpClient,
               igdbOptions.Value.ClientId,
               igdbOptions.Value.AccessToken)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaginatedFromApiResult<GameResponse>> GetPaginedGamesAsync(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            PaginationValidator.Validate(page, pageSize);

            var query = ExternalApiQueryStore.Igdb.GenerateGamesQuery(page, pageSize);
            var request = CreateIgdbRequest(query, "games");

            var igdbGames = await SendIgdbRequestAsync<IgdbGameDto>(request, cancellationToken);

            var titles = igdbGames.Select(g => g.Name.Trim().ToLower()).ToList();
            var savedGames = await _gameRepository.GetByTitlesAsync(titles);

            var responses = igdbGames.Select(igdbGame =>
            {
                var savedGame = savedGames.FirstOrDefault(g =>
                    g.Title.Trim().Equals(igdbGame.Name.Trim(), StringComparison.OrdinalIgnoreCase));

                var response = _mapper.Map<GameResponse>(igdbGame);
                response.Id = savedGame?.Id ?? 0;

                return response;
            }).ToList();

            return new PaginatedFromApiResult<GameResponse>(responses, page, pageSize);
        }
        catch (InvalidPaginationException ex)
        {
            _logger.LogWarning(ex, "Invalid pagination parameters.");
            throw;
        }
        catch (IgdbApiException ex)
        {
            _logger.LogError(ex, "Error communicating with IGDB.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching games.");
            throw;
        }
    }

    public async Task SaveGamesAsync(int batchSize = 500, CancellationToken cancellationToken = default)
    {
        int offset = 0;
        bool hasMoreGames = true;

        try
        {
            while (hasMoreGames)
            {
                var query = ExternalApiQueryStore.Igdb.GenerateGamesQueryWithOffset(offset, batchSize);
                var request = CreateIgdbRequest(query, "games");

                var igdbGames = await SendIgdbRequestAsync<IgdbGameDto>(request, cancellationToken);

                if (igdbGames == null || !igdbGames.Any())
                {
                    hasMoreGames = false;
                    continue;
                }

                var gamesToSave = _mapper.Map<IEnumerable<Game>>(igdbGames);
                foreach (var game in gamesToSave)
                {
                    game.Released = game.Released.ToUniversalTime();
                }

                await _gameRepository.AddGamesAsync(gamesToSave);
                offset += batchSize;
            }
        }
        catch (IgdbApiException ex)
        {
            _logger.LogError(ex, "Error accessing IGDB API during SaveGamesAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while saving games in batch.");
            throw;
        }
    }

    public async Task<PaginatedResult<GameDTO>> GetGameExclusiveByPlatformAsync(
        Platform? platform, string? searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            PlatformSearchValidator.Validate(platform, searchTerm);

            var offset = (page - 1) * pageSize;
            var platformId = platform.HasValue ? (int)platform.Value : 0;
            var platformName = platform.HasValue ? PlatformMapping.GetPlatformName(platform.Value) : "Unknown Platform";

            if (platformName == "Unknown Platform" && platformId != 0)
                throw new InvalidPlatformException($"Plataforma com ID {platformId} não encontrada.");

            var result = await _gameRepository.GetGamesExclusivePlatformAsync(platformId, searchTerm, offset, pageSize, cancellationToken);

            var gameDTOs = _mapper.Map<IEnumerable<GameDTO>>(result.games);
            var totalPages = (int)Math.Ceiling((double)result.totalCount / pageSize);

            return new PaginatedResult<GameDTO>(gameDTOs.ToList(), page, pageSize, result.totalCount, totalPages);
        }
        catch (InvalidPlatformException ex)
        {
            _logger.LogWarning(ex, "Plataforma inválida informada.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar jogos pela plataforma.");
            throw;
        }
    }

    public async Task<PaginatedResult<GameDTO>> GetGamesByYearCategoryAsync(
        YearCategory? yearCategory, string? searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            YearCategoryValidator.Validate(yearCategory);

            var category = yearCategory!.Value;
            var offset = (page - 1) * pageSize;

            int? startYear = null, endYear = null;

            switch (category)
            {
                case YearCategory.Classics:
                    startYear = 1980;
                    endYear = 1999;
                    break;

                case YearCategory.Recent:
                    startYear = 2000;
                    endYear = 2024;
                    break;

                case YearCategory.Release:
                    startYear = 2025;
                    endYear = 2025;
                    break;

                case YearCategory.All:
                    break;
            }

            var result = await _gameRepository.GetGamesByYearCategoryAsync(
                startYear, endYear, searchTerm, offset, pageSize, cancellationToken);

            var gameDTOs = _mapper.Map<IEnumerable<GameDTO>>(result.games);
            var totalPages = (int)Math.Ceiling((double)result.totalCount / pageSize);

            return new PaginatedResult<GameDTO>(
                gameDTOs.ToList(), page, pageSize, result.totalCount, totalPages);
        }
        catch (InvalidYearCategoryException ex)
        {
            _logger.LogWarning(ex, "Categoria de ano inválida fornecida.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar jogos por categoria de ano.");
            throw;
        }
    }
}