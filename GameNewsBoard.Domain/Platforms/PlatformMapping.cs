public static class PlatformMapping
{
    private static readonly Dictionary<Platform, string> PlatformNames = new Dictionary<Platform, string>
    {
        { Platform.PCMicrosoftWindows, "PC Microsoft Windows" },
        { Platform.Xbox, "Xbox" },
        { Platform.Xbox360, "Xbox 360" },
        { Platform.XboxOne, "Xbox One" },
        { Platform.XboxSeriesXS, "Xbox Series X|S" },
        { Platform.PlayStation2, "PlayStation 2" },
        { Platform.PlayStation3, "PlayStation 3" },
        { Platform.PlayStation4, "PlayStation 4" },
        { Platform.PlayStation5, "PlayStation 5" },
        { Platform.NintendoSwitch, "Nintendo Switch" },
        { Platform.Nintendo3DS, "Nintendo 3DS" },
        { Platform.SuperNintendoEntertainmentSystem, "Super Nintendo Entertainment System" },
        { Platform.Wii, "Wii" },
        { Platform.WiiU, "Wii U" },
        { Platform.Arcade, "Arcade" }
    };

    public static string GetPlatformNameById(int platformId)
    {
        if (Enum.IsDefined(typeof(Platform), platformId))
        {
            var platform = (Platform)platformId;
            return PlatformNames.TryGetValue(platform, out var name) ? name : "Unknown Platform";
        }
        return "Unknown Platform";
    }

    public static string GetPlatformName(Platform platform)
    {
        return PlatformNames.TryGetValue(platform, out var name) ? name : "Unknown Platform";
    }
}
