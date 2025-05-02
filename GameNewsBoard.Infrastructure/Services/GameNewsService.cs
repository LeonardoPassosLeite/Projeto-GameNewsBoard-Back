using System.Text.Json;
using AutoMapper;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Infrastructure.ExternalDtos;
using GameNewsBoard.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;


namespace GameNewsBoard.Infrastructure.Services
{
    public class GameNewsService : IGameNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public GameNewsService(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _apiKey = configuration["Keys:NewsApiKey"]
                ?? throw new InvalidOperationException("API key da NewsData não configurada.");
            _baseUrl = configuration["ExternalApis:GNewsBaseUrl"]
                ?? throw new InvalidOperationException("Base URL da NewsData não configurada.");
        }

        public async Task<GameNewsResponse> GetLatestNewsAsync(string platform)
        {
            var url = ExternalApiUrlBuilder.BuildNewsApiUrl(_baseUrl, _apiKey, platform);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro da API GNews: {response.StatusCode} - {content}");

            var gnewsData = JsonSerializer.Deserialize<GNewsResponseWrapper>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (gnewsData is null || gnewsData.Articles.Count == 0)
                return new GameNewsResponse();

            return _mapper.Map<GameNewsResponse>(gnewsData);
        }
    }
}