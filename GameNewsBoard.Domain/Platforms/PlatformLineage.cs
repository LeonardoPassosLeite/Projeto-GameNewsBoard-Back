
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Domain.Platforms
{
    public static class PlatformLineage
    {
        public static readonly Dictionary<Platform, List<string>> PcOrder = new()
    {
        { Platform.PCMicrosoftWindows, new() { "PC (Microsoft Windows)", "Mac", "Linux" } }
    };
        public static readonly Dictionary<Platform, List<string>> MicrosoftOrder = new()
    {
        { Platform.Xbox, new() { "Xbox" } },
        { Platform.Xbox360, new() { "Xbox", "Xbox 360" } },
        { Platform.XboxOne, new() { "Xbox", "Xbox 360", "Xbox One" } },
        { Platform.XboxSeriesXS, new() { "Xbox Series X|S", "Xbox One", "PC (Microsoft Windows)" } }
    };

        public static readonly Dictionary<Platform, List<string>> SonyOrder = new()
    {
        { Platform.PlayStation2, new() { "PlayStation 2" } },
        { Platform.PlayStation3, new() { "PlayStation 2", "PlayStation 3" } },
        { Platform.PlayStation4, new() { "PlayStation 2", "PlayStation 3", "PlayStation 4" } },
        { Platform.PlayStation5, new() { "PlayStation 2", "PlayStation 3", "PlayStation 4", "PlayStation 5",  "PC (Microsoft Windows)" } }
    };

        public static readonly Dictionary<Platform, List<string>> NintendoOrder = new()
    {
        { Platform.SuperNintendoEntertainmentSystem, new() { "Super Nintendo" } },
        { Platform.Wii, new() { "Super Nintendo", "Wii" } },
        { Platform.WiiU, new() { "Super Nintendo", "Wii", "Wii U" } },
        { Platform.Nintendo3DS, new() { "Super Nintendo", "Wii", "Wii U", "Nintendo 3DS" } },
        { Platform.NintendoSwitch, new() { "Super Nintendo", "Wii", "Wii U", "Nintendo 3DS", "Nintendo Switch" } }
    };
    }
}