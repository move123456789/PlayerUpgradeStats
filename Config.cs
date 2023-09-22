using RedLoader;
using UnityEngine;

namespace PlayerUpdadeStats;

public static class Config
{
    public static ConfigCategory Category { get; private set; }
    public static ConfigCategory Advanced { get; private set; }
    public static ConfigEntry<KeyCode> ToggleMenuKey { get; private set; }
    public static ConfigEntry<bool> DebugLogging { get; private set; }

    public static void Init()
    {
        Category = ConfigSystem.CreateCategory("default", "Category");
        Advanced = ConfigSystem.CreateCategory("advanced", "Advanced");

        ToggleMenuKey = Category.CreateEntry(
            "menu_key",
            KeyCode.Keypad3,
            "Toggle Menu Key",
            "The key that toggles the Points Menu.");

        DebugLogging = Advanced.CreateEntry(
            "enable_logging",
            false,
            "Enable Debug Logs",
            "Enables PlayerUpgradeStats Debug Logs of the game to the console.");
    }
}