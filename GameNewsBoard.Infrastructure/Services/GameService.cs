using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using GameNewsBoard.Application.DTOs;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Exceptions;
using GameNewsBoard.Application.Exceptions.Api;
using GameNewsBoard.Application.Exceptions.Domain;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Repositories;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Application.Validators;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Infrastructure.ExternalDtos;
using GameNewsBoard.Infrastructure.Helpers;
using GameNewsBoard.Infrastructure.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GameNewsBoard.Infrastructure.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _clientId;
        private readonly string _accessToken;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GameService> _logger;

        public GameService(HttpClient httpClient, IConfiguration configuration, IMapper mapper, IGameRepository gameRepository, ILogger<GameService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _clientId = configuration["Twitch:ClientId"]
                        ?? throw new InvalidOperationException("Client ID da Twitch não configurado.");
            _accessToken = configuration["Twitch:AccessToken"]
                        ?? throw new InvalidOperationException("Access Token da Twitch não configurado.");
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PaginatedFromApiResult<GameResponse>> GetPaginedGamesAsync(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                PaginationValidator.Validate(page, pageSize);

                var dataQuery = ExternalApiQueryStore.Igdb.GenerateGamesQuery(page, pageSize);
                var dataRequest = CreateIgdbRequest(dataQuery);

                var games = await SendIgdbRequestAsync(dataRequest, cancellationToken);

                return new PaginatedFromApiResult<GameResponse>(games, page, pageSize);
            }
            catch (InvalidPaginationException ex)
            {
                _logger.LogWarning(ex, "Parâmetros inválidos para a paginação.");
                throw;
            }
            catch (IgdbApiException ex)
            {
                _logger.LogError(ex, "Erro na comunicação com a IGDB.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar jogos.");
                throw;
            }
        }

        public async Task SaveGamesAsync(
            int batchSize = 500, CancellationToken cancellationToken = default)
        {
            int offset = 0;
            bool hasMoreGames = true;

            try
            {
                while (hasMoreGames)
                {
                    var dataQuery = ExternalApiQueryStore.Igdb.GenerateGamesQueryWithOffset(offset, batchSize);
                    var dataRequest = CreateIgdbRequest(dataQuery);

                    var games = await SendIgdbRequestAsync(dataRequest, cancellationToken);

                    if (games == null || !games.Any())
                    {
                        hasMoreGames = false;
                        continue;
                    }

                    var gamesToSave = _mapper.Map<IEnumerable<Game>>(games);

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
                _logger.LogError(ex, "Erro ao acessar a API da IGDB durante SaveGamesAsync.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao salvar jogos em lotes.");
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
                        break; // sem filtro
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


        private async Task<List<GameResponse>> SendIgdbRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                    throw new IgdbApiException($"Erro IGDB: {response.StatusCode}, Resposta: {content}");

                var igdbGames = JsonSerializer.Deserialize<List<IgdbGameDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IgdbGameDto>();

                var gameResponses = _mapper.Map<List<GameResponse>>(igdbGames);

                return gameResponses;
            }
            catch (HttpRequestException ex)
            {
                throw new IgdbApiException("Erro de rede ao tentar acessar a IGDB.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new IgdbApiException("Timeout na requisição para a IGDB.", ex);
            }
            catch (Exception ex)
            {
                throw new IgdbApiException("Erro inesperado ao acessar a IGDB.", ex);
            }
        }

        private HttpRequestMessage CreateIgdbRequest(string query)
        {
            var url = ExternalApiUrlBuilder.BuildIgdbGamesUrl();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Client-ID", _clientId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            request.Content = new StringContent(query, Encoding.UTF8, "text/plain");
            return request;
        }

    }
}