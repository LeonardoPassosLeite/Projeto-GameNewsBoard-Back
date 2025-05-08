namespace GameNewsBoard.Infrastructure.Helpers
{
    public static class ExternalApiUrlBuilder
    {
        public static string BuildNewsApiUrl(string baseUrl, string apiKey, string platform)
        {
            var query = platform.ToLower() switch
            {
                "xbox" => "xbox",
                "ps5" => "playstation",
                "nintendo" => "nintendo",
                "pc" => "steam",
                _ => platform
            };

            var encodedQuery = Uri.EscapeDataString(query);

            return $"{baseUrl}?q={encodedQuery}&lang=pt&token={apiKey}";
        }

        public static string BuildIgdbUrl(string endpoint)
        {
            return $"https://api.igdb.com/v4/{endpoint}";
        }

        public static string BuildIgdbGamesUrl()
        {
            return BuildIgdbUrl("games");
        }

        public static string BuildMetacriticUrl(int year)
        {
            return $"https://www.metacritic.com/browse/game/all/all/coming-soon?releaseYear={year}";
        }
    }
}