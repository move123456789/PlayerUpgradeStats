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
using Sons.Weapon;
using System;
using Sons.Items.Core;

namespace PlayerUpgradeStats;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public partial class Plugin : BasePlugin
{
    public const string PLUGIN_GUID = "Smokyace.PlayerUpgradeStats";
    public const string PLUGIN_NAME = "PlayerUpgradeStats";
    public const string PLUGIN_VERSION = "1.0.5";
    private const string author = "SmokyAce";

    public static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "PlayerUpgradeStats.cfg"), true);
    public static ConfigEntry<KeyCode> smokyaceMenurKey = configFile.Bind("General", "MenuKey", KeyCode.Keypad4, "Hotkey for opening menu");
    public static ConfigEntry<bool> smokyaceLogToConsole = configFile.Bind("Advanced", "ShowLogs", false, new ConfigDescription("Logs will display in the console", null, "Advanced"));
    public static ConfigEntry<bool> smokyacePostFixLogsToConsole = configFile.Bind("Advanced", "DisplayPostFixLogs", false, new ConfigDescription("Display PostFix Logs each time its called to console", null, "Advanced"));
    public static ConfigEntry<bool> smokyaceDeactivate = configFile.Bind("General", "SoftDeactivate", false, "Disables some parts of the mod, does not unregister objects saveid identification");


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

        [HarmonyPatch(typeof(GenericMeleeWeaponController), "OnEnable")]
        [HarmonyPostfix]
        public static void PostfixGenericMeleeWeaponController(ref GenericMeleeWeaponController __instance)
        {
            PostLogsToConsole("OnEnable GenericMeleeWeaponController");
            string instanceName = __instance.name;
            if (instanceName == "TacticalChainsawHeld" || instanceName == "TacticalChainsawHeld(Clone)")
            {
                PostLogsToConsole("__instance.gameObject.name = " + __instance.gameObject.name);
                PostLogsToConsole("Trying to get component ChainsawWeaponController");
                ChainsawWeaponController chainsawWeaponController = __instance.gameObject.GetComponent<ChainsawWeaponController>();
                chainsawWeaponController._treeHitFrequency = 0.25f * (1 - BuyUpgrades.currentChainsawSpeedLevel * 19 / 100);
                PostLogsToConsole("_treeHitFrequency updated, new value = " + 0.25f * (1 - BuyUpgrades.currentChainsawSpeedLevel * 19 / 100));
            }

        }
        [HarmonyPatch(typeof(PlayerKnightVAction), nameof(PlayerKnightVAction.StartRiding))]
        [HarmonyPostfix]
        public static void PostfixKnightV(ref PlayerKnightVAction __instance)
        {
            PostLogsToConsole("OnEnable PlayerKnightVAction");
            if (BuyUpgrades.currentKnightVSpeedLevel == 0) { PostLogsToConsole("currentKnightVSpeedLevel == 0, no need for updating"); return; }
            try
            {
                float calculatedMaxVelocity = defaultMaxVelocity * (BuyUpgrades.currentKnightVSpeedLevel * 20 / 100 + 1);
                PostLogsToConsole("Calculated New Current KnightV Speed = " + calculatedMaxVelocity);
                if (__instance._controlDefinition.MaxVelocity == calculatedMaxVelocity) { PostLogsToConsole("KnightVControlDefinition does not need updating"); return; }
                __instance._controlDefinition.MaxVelocity = defaultMaxVelocity * (BuyUpgrades.currentKnightVSpeedLevel * 20 / 100 + 1);
                PostLogsToConsole("After Updated Veloicy, get result = " + __instance._controlDefinition.MaxVelocity);
                
            }
            catch (Exception e) { PostErrorToConsole("Something went wrong in PostfixKnightVControlDefinition, Error: " + e); }
        }
        private static float defaultMaxVelocity = 20f;


        [HarmonyPatch(typeof(RangedWeapon), "Start")]
        [HarmonyPostfix]
        public static void PostfixBowDamage(ref RangedWeapon __instance)
        {
            PostLogsToConsole("Awake RangedWeapon");
            if (__instance.name == "CraftedBowHeld" || __instance.name == "CraftedBowHeld(Clone)")
            {
                PostLogsToConsole("Bow Now In Hand");
                if (BuyUpgrades.currentBowDamageLevel == 0) { PostLogsToConsole("No Need for Updating, currentBowDamageLevel = 0"); return; }
                var item = LocalPlayer.Inventory.RightHandItem;
                if (item != null)
                {
                    PostLogsToConsole("CraftedBowHeld - Item != null");
                    var ranged = item.ItemObject.GetComponentInChildren<RangedWeapon>();
                    ranged._simulatedBulletInfo = ranged.GetAmmo()?._properties?.ProjectileInfo;
                    PostLogsToConsole($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                    ranged._simulatedBulletInfo.muzzleDamage = defaultBowDamage * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                    PostLogsToConsole($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                }
                else { PostLogsToConsole("CraftedBowHeld - item == null"); }
            }
        }
        private static float defaultBowDamage = 20f;
        [HarmonyPatch(typeof(RangedWeapon), "CycleAmmoType")]
        [HarmonyPostfix]
        public static async void PostfixBowDamageChangeAmmo(RangedWeapon __instance)
        {
            PostLogsToConsole("Awake RangedWeapon");
            if (__instance.name == "CraftedBowHeld" || __instance.name == "CraftedBowHeld(Clone)")
            {
                PostLogsToConsole("Bow Now In Hand");
                if (BuyUpgrades.currentBowDamageLevel == 0) { PostLogsToConsole("No Need for Updating, currentBowDamageLevel = 0"); return; }
                var item = LocalPlayer.Inventory.RightHandItem;
                if (item != null)
                {
                    PostLogsToConsole("CraftedBowHeld - Item != null");
                    var ranged = item.ItemObject.GetComponentInChildren<RangedWeapon>();
                    ranged._simulatedBulletInfo = ranged.GetAmmo()?._properties?.ProjectileInfo;
                    await Task.Run(BowProjectileUpdate);
                    string curretArrow = ranged.bulletPrefab.name;
                    if (curretArrow == null) { PostLogsToConsole("Prefab Name == null"); return; }
                    PostLogsToConsole($"Bullet Prefab Name = {ranged.bulletPrefab.name}");
                    switch (curretArrow)
                    {
                        case "CraftedArrowProjectile":
                            // Crafted Arrow
                            PostLogsToConsole($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = defaultBowDamage * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PostLogsToConsole($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                        case "3dPrintedArrowProjectile":
                            // 3dPrinted Arrow
                            PostLogsToConsole($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = 30f * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PostLogsToConsole($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                        case "TacticalBowAmmoProjectile":
                            // Found Arrow
                            PostLogsToConsole($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = 35f * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PostLogsToConsole($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                    }
                }
                else { PostLogsToConsole("CraftedBowHeld - item == null"); }
            }
        }
        public static async Task BowProjectileUpdate()
        {
            await Task.Delay(300);
        }
    }
}
