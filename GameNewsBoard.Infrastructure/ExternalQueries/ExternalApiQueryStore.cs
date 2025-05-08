namespace GameNewsBoard.Infrastructure.Queries
{
    public static class ExternalApiQueryStore
    {
        public static class Igdb
        {
            public static string GenerateGamesQuery(int page, int pageSize)
            {
                return $@"
                    fields name, aggregated_rating, rating, first_release_date, cover.url, platforms.name;
                    where rating != null & platforms != null & cover != null;
                    sort aggregated_rating desc;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }

            public static string GenerateGamesQueryWithOffset(int offset, int pageSize)
            {
                return $@"
                    fields name, aggregated_rating, rating, first_release_date, cover.url, platforms.name;
                    where rating != null & platforms != null & cover != null;
                    sort aggregated_rating desc;
                    limit {pageSize};
                    offset {offset};";
            }

            public static string GenerateReleasesBetweenQuery(long startUnix, long endUnix, int limit = 50)
            {
                return $@"
                    fields game.name, game.cover.url, game.platforms.name, date;
                    where date >= {startUnix} & date <= {endUnix};
                    sort date asc;
                    limit {limit};";
            }
        }
    }
}