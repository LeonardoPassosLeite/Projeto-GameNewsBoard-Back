namespace GameNewsBoard.Infrastructure.Queries
{
    public static class ExternalApiQueryStore
    {
        public static class Igdb
        {
            public static string BuildGamesByPlatform(int page, int pageSize)
            {
                return $@"
                    fields name, aggregated_rating, rating, first_release_date, cover.url, platforms.name;
                    where rating != null & platforms != null & cover != null;
                    sort aggregated_rating desc;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }

            public static string BuildSearchGameByName(string name, int page, int pageSize)
            {
                return $@"
                    search ""{name}"";
                    fields name, rating, cover.url, platforms.name, first_release_date;
                    where cover != null;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }

            public static string BuildSearchGameByNameWithPlatform(string name, int platformId, int page, int pageSize, int? year = null)
            {
                // Verificar se o ano foi fornecido
                var yearFilter = year.HasValue
                    ? $"& first_release_date >= {UnixTimeHelper.GetUnixRangeForYear(year.Value).Item1} & first_release_date < {UnixTimeHelper.GetUnixRangeForYear(year.Value).Item2}"
                    : string.Empty;

                return $@"
        search ""{name}"";
        fields name, rating, cover.url, platforms.name, first_release_date;
        where cover != null & platforms = ({platformId}) {yearFilter};
        limit {pageSize};
        offset {(page - 1) * pageSize};";
            }


            public static string BuildSearchGameByPlatform(int? platformId, int page, int pageSize)
            {
                var platformFilter = platformId.HasValue
                    ? $"& platforms = ({platformId.Value})"
                    : string.Empty;

                return $@"
                    fields name, aggregated_rating, rating, first_release_date, cover.url, platforms.name;
                    where rating != null & cover != null & platforms != null {platformFilter};
                    sort aggregated_rating desc;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }

            public static string BuildSearchGameByYear(int year, int page, int pageSize)
            {
                var (start, end) = UnixTimeHelper.GetUnixRangeForYear(year);

                return $@"
                    fields name, rating, cover.url, platforms.name, first_release_date;
                    where cover != null & first_release_date >= {start} & first_release_date < {end};
                    sort first_release_date desc;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }

            public static string BuildSearchGameByYearAndPlatform(int year, int platformId, int page, int pageSize)
            {
                var (start, end) = UnixTimeHelper.GetUnixRangeForYear(year);

                return $@"
                    fields name, rating, cover.url, platforms.name, first_release_date;
                    where cover != null
                    & platforms = ({platformId})
                    & first_release_date >= {start}
                    & first_release_date < {end};
                    sort first_release_date desc;
                    limit {pageSize};
                    offset {(page - 1) * pageSize};";
            }
        }
    }
}