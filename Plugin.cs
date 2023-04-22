using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.IO;
using TheForest;
using TheForest.Utils;
using Sons.Gameplay;
using Sons.Save;
using Sons.Gui;
using Sons.Gameplay.GPS;
using UniverseLib.UI;
using static PlayerUpgradeStats.Plugin;
using Sons.Gameplay.GameSetup;
using Sons;
using System.Threading.Tasks;
using Sons.StatSystem;

namespace PlayerUpgradeStats;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public partial class Plugin : BasePlugin
{
    public const string PLUGIN_GUID = "Smokyace.PlayerUpgradeStats";
    public const string PLUGIN_NAME = "PlayerUpgradeStats";
    public const string PLUGIN_VERSION = "1.0.2";
    private const string author = "SmokyAce";

    public static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "PlayerUpgradeStats.cfg"), true);
    public static ConfigEntry<KeyCode> smokyaceMenurKey = configFile.Bind("General", "MenuKey", KeyCode.Keypad4, "Hotkey for opening menu");
    public static ConfigEntry<bool> smokyaceLogToConsole = configFile.Bind("Advanced", "ShowLogs", false, new ConfigDescription("Logs will display in the console", null, "Advanced"));
    public static ConfigEntry<bool> smokyacePostFixLogsToConsole = configFile.Bind("Advanced", "DisplayPostFixLogs", false, new ConfigDescription("Display PostFix Logs each time its called to console", null, "Advanced"));
    public static ConfigEntry<bool> smokyaceDeactivate = configFile.Bind("General", "SoftDeactivate", false, "Disables the mod, but does not unregister objects");


    public static ManualLogSource DLog = new ManualLogSource("DLog");
    public override void Load()
    {
        BepInEx.Logging.Logger.Sources.Add(DLog);

        Log.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

        //For harmony
        Harmony.CreateAndPatchAll(typeof(PlayerStatsPatcher), null);

        //For Mono Behavior
        base.AddComponent<PlayerStatsMono>();

        // Data Folder
        string dir = @"PlayerUpgradeStatsData";
        // If directory does not exist, create it
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public class PlayerStatsPatcher
    {
        private static bool hasPostFixLogged = false;

        [HarmonyPatch(typeof(GPSTrackerSystem), "OnEnable")]
        [HarmonyPostfix]
        public static void OnEnablePostfix(ref GPSTrackerSystem __instance)
        {
            if (smokyacePostFixLogsToConsole.Value)
            {
                PostLogsToConsole("IN From GPS OnEnablePostfix From PlayerUpgradeStats");
            } else
            {
                if (!hasPostFixLogged)
                {
                    PostLogsToConsole("IN From GPS OnEnablePostfix From PlayerUpgradeStats");
                    hasPostFixLogged = true;
                }
            }
            
            if (myUIBase == null)
            {
                PostLogsToConsole("Regestrating PlayerUpgradeStats UI");
                myUIBase = UniversalUI.RegisterUI(Plugin.PLUGIN_GUID, null);
                myPanel = new(myUIBase);
                myPanel.SetActive(false);
            }
            DataHandler.GetStrengthLevelVitals();
        }
        public static UIBase myUIBase;
        public static MyPanel myPanel;
        public static int currentStrengthLevel;

        [HarmonyPatch(typeof(GameSetupManager), "GetSelectedSaveId")]
        [HarmonyPostfix]
        public static void PostfixGetLoadedSaveID(uint __result)
        {
            PostLogsToConsole("Postfix PostfixGetLoadedSaveID Loaded");
            uint? nullable = __result;
            if (nullable.HasValue)
            {
                PostLogsToConsole("Save Id = " + __result);
                if (__result != 0)
                {
                    postfixSaveID = __result;
                    DataHandler.GetData();
                }
            }
            else { PostLogsToConsole("SaveId Posfix __result Does Not Have A Value"); }
            
        }
        
        public static uint postfixSaveID;
    }
}
