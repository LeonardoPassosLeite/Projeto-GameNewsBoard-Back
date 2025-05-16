using System.Text;
using GameNewsBoard.Domain.Enums;

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

            public static string GenerateReleasesBetweenQuery(long startUnix, long endUnix, Platform? platform = null, int limit = 50)
            {
                var whereClause = $"date >= {startUnix} & date <= {endUnix}";

                if (platform.HasValue && platform.Value != Platform.All)
                {
                    whereClause += $" & platform = {(int)platform.Value}";
                }

                var sb = new StringBuilder();
                sb.AppendLine("fields game.name, game.cover.url, game.id, platform.name, platform.abbreviation, date;");
                sb.AppendLine($"where {whereClause};");
                sb.AppendLine("sort date asc;");
                sb.AppendLine($"limit {limit};");

                return sb.ToString();
            }


        }
    }
}