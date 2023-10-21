using RedLoader;
using SonsSdk;
using UnityEngine;

namespace PlayerUpdadeStats;

public static class Config
{
    public static ConfigCategory PlayerUpdadeStats { get; private set; }
    public static ConfigCategory PlayerUpdadeStatsAdvanced { get; private set; }
    public static KeybindConfigEntry ToggleMenuKey { get; private set; }
    public static ConfigEntry<bool> DebugLogging { get; private set; }
    public static ConfigEntry<bool> UiTesting { get; private set; }


    public static void Init()
    {
        PlayerUpdadeStats = ConfigSystem.CreateCategory("playerUpgradeStats", "PlayerUpdadeStats");
        PlayerUpdadeStatsAdvanced = ConfigSystem.CreateCategory("Advanced", "PlayerUpdadeStatsAdvanced", true);

        ToggleMenuKey = PlayerUpdadeStats.CreateKeybindEntry(
            "menu_key",
            "numpad3",
            "Toggle Menu Key",
            "The key that toggles the Points Menu.");
        ToggleMenuKey.Notify(() =>
        {
            PlayerStatsFunctions.OnMenuKeyPressed();
        });

        DebugLogging = PlayerUpdadeStatsAdvanced.CreateEntry(
            "enable_logging_advanced",
            false,
            "Enable Debug Logs",
            "Enables PlayerUpgradeStats Debug Logs of the game to the console.");

        UiTesting = PlayerUpdadeStatsAdvanced.CreateEntry(
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