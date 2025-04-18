public static class UnixTimeHelper
{
    public static (long start, long end) GetUnixRangeForYear(int year)
    {
        var start = new DateTimeOffset(new DateTime(year, 1, 1)).ToUnixTimeSeconds();
        var end = new DateTimeOffset(new DateTime(year + 1, 1, 1)).ToUnixTimeSeconds();
        return (start, end);
    }
}
