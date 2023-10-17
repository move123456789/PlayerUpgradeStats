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
    }

    // Mega Upgrades Multiplyers
    public static float MeleeStaminaPercentageReduction = 0.80f; // 20% Reduction For Each Level
    public static float TreeSwingStaminaPercentageReduction = 0.80f; // 20% Reduction For Each Level
    public static float StaminaBarRecoverRate = 1.20f; // 20% Increased Stamina Regen For Each Level
    public static float RunStaminaCostPerSec = 0.80f; // 20% Reduction For Each Level
    public static float TimeToRecoverFromRun = 0.80f; // 20% Reduction For Each Level
    public static float JumpStaminaCost = 0.80f; // 20% Reduction For Each Level

    // UI Mega Upgrades Multipiers
    public const int UpdateMegaUIIncreace = 20; // Level x Value.Here = Total Ui % Text
}