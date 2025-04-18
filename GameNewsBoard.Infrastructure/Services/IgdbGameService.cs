using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using GameNewsBoard.Application.DTOs.Shared;
using GameNewsBoard.Application.Enums;
using GameNewsBoard.Application.Exceptions;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs;
using GameNewsBoard.Infrastructure.ExternalDtos;
using GameNewsBoard.Infrastructure.Helpers;
using GameNewsBoard.Infrastructure.Queries;
using Microsoft.Extensions.Configuration;

namespace GameNewsBoard.Infrastructure.Services
{
    public class IgdbGameService : IIgdbGameService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _clientId;
        private readonly string _accessToken;

        public IgdbGameService(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _clientId = configuration["Twitch:ClientId"]
                        ?? throw new InvalidOperationException("Client ID da Twitch não configurado.");
            _accessToken = configuration["Twitch:AccessToken"]
                        ?? throw new InvalidOperationException("Access Token da Twitch não configurado.");
        }

        public async Task<PaginatedResult<GameResponse>> GetGamesByPlatformAsync(
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (page <= 0 || pageSize <= 0)
                throw new ArgumentException("Página e tamanho devem ser maiores que zero.");

            var dataQuery = ExternalApiQueryStore.Igdb.BuildGamesByPlatform(page, pageSize);
            var dataRequest = CreateIgdbRequest(dataQuery);
            var games = await SendIgdbRequestAsync(dataRequest, cancellationToken);

            return new PaginatedResult<GameResponse>
            {
                Items = games,
                Page = page,
                PageSize = pageSize,
            };
        }

        public async Task<List<GameResponse>> SearchGamesAsync(string? name = null, IgdbPlatform? platform = null, int? year = null, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            string query;
            var platformId = (platform.HasValue && platform.Value != IgdbPlatform.All) ? (int?)platform.Value : null;

            if (!string.IsNullOrWhiteSpace(name) && platformId.HasValue && year.HasValue)
                query = ExternalApiQueryStore.Igdb.BuildSearchGameByNameWithPlatform(name, platformId.Value, page, pageSize, year);

            else if (!string.IsNullOrWhiteSpace(name) && platformId.HasValue)

                query = ExternalApiQueryStore.Igdb.BuildSearchGameByNameWithPlatform(name, platformId.Value, page, pageSize);

            else if (!string.IsNullOrWhiteSpace(name))
                query = ExternalApiQueryStore.Igdb.BuildSearchGameByName(name, page, pageSize);

            else if (platformId.HasValue)
                query = ExternalApiQueryStore.Igdb.BuildSearchGameByPlatform(platformId, page, pageSize);

            else if (year.HasValue)
                query = ExternalApiQueryStore.Igdb.BuildSearchGameByYear(year.Value, page, pageSize);

            else
                query = ExternalApiQueryStore.Igdb.BuildGamesByPlatform(page, pageSize);

            var request = CreateIgdbRequest(query);
            return await SendIgdbRequestAsync(request, cancellationToken);
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
                }) ?? new();

                return _mapper.Map<List<GameResponse>>(igdbGames);
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
    }
}