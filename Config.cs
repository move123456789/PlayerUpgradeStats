using RedLoader;
using UnityEngine;

namespace PlayerUpdadeStats;

public static class Config
{
    public static ConfigCategory PlayerUpdadeStats { get; private set; }
    public static ConfigEntry<KeyCode> ToggleMenuKey { get; private set; }
    public static ConfigEntry<bool> DebugLogging { get; private set; }
    public static ConfigEntry<bool> UiTesting { get; private set; }

    public static ConfigEntry<float> testStamina { get; private set; }

    public static void Init()
    {
        PlayerUpdadeStats = ConfigSystem.CreateCategory("playerUpgradeStats", "PlayerUpdadeStats");

        ToggleMenuKey = PlayerUpdadeStats.CreateEntry(
            "menu_key",
            KeyCode.Keypad3,
            "Toggle Menu Key",
            "The key that toggles the Points Menu.");

        DebugLogging = PlayerUpdadeStats.CreateEntry(
            "enable_logging",
            false,
            "Enable Debug Logs",
            "Enables PlayerUpgradeStats Debug Logs of the game to the console.");

        UiTesting = PlayerUpdadeStats.CreateEntry(
            "enable_ui_on_main_page",
            false,
            "Enable UI Testing",
            "Enables UI to be opened out of the game.");
        testStamina = PlayerUpdadeStats.CreateEntry(
            "test_stam",
            1f,
            "test_stam",
            "test_stam");
    }
}