using Sons.Save;
using Sons.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TheForest.Utils;
using TMPro;
using UnityEngine;
using static BoltDebugStartSettings;
using static PlayerUpgradeStats.Plugin;

namespace PlayerUpgradeStats
{
    public class DataHandler
    {
        internal static void WriteDynamicJsonObject(JsonObject jsonObj, string fileName)
        {
            using var fileStream = File.Create("PlayerUpgradeStatsData/" + fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(fileStream);
            jsonObj.WriteTo(utf8JsonWriter);
        }
        public static void SaveData()
        {
            Plugin.PostLogsToConsole("Saving");
            if (PlayerStatsPatcher.postfixSaveID != 0 && currentPoints != 0)
            {
                if (currentPoints > 50) { PostErrorToConsole("From Savedata() CurrentPoints can't be over 50"); return; }
                var fileName = $"{PlayerStatsPatcher.postfixSaveID}.json";
                var jsonObj = new JsonObject
                {
                    ["WorldID"] = PlayerStatsPatcher.postfixSaveID,
                    ["pointsUsed"] = pointsUsed,
                    ["currentPoints"] = currentPoints,
                    ["currentWalkSpeedLevel"] = BuyUpgrades.currentWalkSpeedLevel,
                    ["currentSprintSpeedLevel"] = BuyUpgrades.currentSprintSpeedLevel,
                    ["currentJumpHeightLevel"] = BuyUpgrades.currentJumpHeightLevel,
                    ["currentSwimSpeedLevel"] = BuyUpgrades.currentSwimSpeedLevel,
                    ["currentChainsawSpeedLevel"] = BuyUpgrades.currentChainsawSpeedLevel
                };
                DataHandler.WriteDynamicJsonObject(jsonObj, fileName);
            }
        }
        internal static void GetData()
        {
            if (PlayerStatsPatcher.postfixSaveID != 0 && isUiLoaded)
            {
                if (File.Exists("PlayerUpgradeStatsData/" + $"{PlayerStatsPatcher.postfixSaveID}.json"))
                {
                    PostLogsToConsole("Save File For World Found");
                    try
                    {
                        string text = File.ReadAllText($"PlayerUpgradeStatsData/{PlayerStatsPatcher.postfixSaveID}.json");
                        var saveInfo = JsonSerializer.Deserialize<SavedInfo>(text);
                        PostLogsToConsole("Updating Values");
                        PostLogsToConsole("postfixSaveID = " + PlayerStatsPatcher.postfixSaveID + "  FromFileWorldID = " + saveInfo.WorldID);
                        if (PlayerStatsPatcher.postfixSaveID == saveInfo.WorldID)
                        {
                            if (saveInfo.currentPoints > 50) { PostErrorToConsole("CurrentPoints can't be over 50"); return; }
                            pointsUsed = saveInfo.pointsUsed;
                            currentPoints = saveInfo.currentPoints;
                            BuyUpgrades.currentWalkSpeedLevel = saveInfo.currentWalkSpeedLevel;
                            BuyUpgrades.currentSprintSpeedLevel = saveInfo.currentSprintSpeedLevel;
                            BuyUpgrades.currentJumpHeightLevel = saveInfo.currentJumpHeightLevel;
                            BuyUpgrades.currentSwimSpeedLevel = saveInfo.currentSwimSpeedLevel;
                            BuyUpgrades.currentChainsawSpeedLevel = saveInfo.currentChainsawSpeedLevel;
                            UpdateDisplayedData();
                            UpdateDisplayedCost();
                        }
                    }
                    catch (Exception e) { PostErrorToConsole("Something went wrong trying to load saved data from file. Error: " + e); }

                }
                else { PostLogsToConsole("File Not Found"); }
            }
        }

        public class SavedInfo
        {
            public uint WorldID { get; set; }
            public int pointsUsed { get; set; }
            public int currentPoints { get; set; }
            public float currentWalkSpeedLevel { get; set; }
            public float currentSprintSpeedLevel { get; set; }
            public float currentJumpHeightLevel { get; set; }
            public float currentSwimSpeedLevel { get; set; }
            public float currentChainsawSpeedLevel { get; set; }

        }
        public static void UpdateDisplayedData()
        {
            doUpdateSpeeds = true;
            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
            float totalwalkSpeedIncrease = BuyUpgrades.currentWalkSpeedLevel * 20;
            MyPanel.walkSpeedIncrease.text = $"Speed: +{totalwalkSpeedIncrease}%" + $"  Level {BuyUpgrades.currentWalkSpeedLevel}/5";
            float totalSprintSpeedIncrease = BuyUpgrades.currentSprintSpeedLevel * 20;
            MyPanel.sprintSpeedIncrease.text = $"Speed: +{totalSprintSpeedIncrease}%" + $"  Level {BuyUpgrades.currentSprintSpeedLevel}/5";
            float totalJumpHeightIncrease = BuyUpgrades.currentJumpHeightLevel * 20;
            MyPanel.jumpHeightIncrease.text = $"Height: +{totalJumpHeightIncrease}%" + $"  Level {BuyUpgrades.currentJumpHeightLevel}/5";
            float totalSwimSpeedIncrease = BuyUpgrades.currentSwimSpeedLevel * 20;
            MyPanel.swimSpeedIncrease.text = $"Speed: +{totalSwimSpeedIncrease}%" + $"  Level {BuyUpgrades.currentSwimSpeedLevel}/5";
            float totalChainsawSpeedIncrease = BuyUpgrades.currentChainsawSpeedLevel * 20;
            MyPanel.chainSawSpeedIncrease.text = $"Speed: +{totalChainsawSpeedIncrease}%" + $"  Level {BuyUpgrades.currentChainsawSpeedLevel}/5";
        }
        public static void UpdateDisplayedCost()
        {
            if (BuyUpgrades.currentWalkSpeedLevel >= 2) { MyPanel.walkSpeedCost.text = $"Cost: {BuyUpgrades.pointPriceText4}"; }
            if (BuyUpgrades.currentSprintSpeedLevel >= 2) { MyPanel.sprintSpeedCost.text = $"Cost: {BuyUpgrades.pointPriceText4}"; }
            if (BuyUpgrades.currentJumpHeightLevel >= 2) { MyPanel.jumpHeightCost.text = $"Cost: {BuyUpgrades.pointPriceText4}"; }
            if (BuyUpgrades.currentSwimSpeedLevel >= 2) { MyPanel.swimSpeedCost.text = $"Cost: {BuyUpgrades.pointPriceText4}"; }
            if (BuyUpgrades.currentChainsawSpeedLevel >= 2) { MyPanel.chainSawSpeedCost.text = $"Cost: {BuyUpgrades.pointPriceText4}"; }
        }

        public static async void GetStrengthLevelVitals()
        {
            while (true)
            {
                PostLogsToConsole("Strength Level = " + LocalPlayer.Vitals.CurrentStrengthLevel);
                if (LocalPlayer.Vitals.CurrentStrengthLevel == 1)
                {
                    await Task.Delay(5000);
                }
                else
                {
                    PlayerStatsPatcher.currentStrengthLevel = LocalPlayer.Vitals.CurrentStrengthLevel;
                    MyPanel.curStrengthLvl.text = $"Your Strength Level: {PlayerStatsPatcher.currentStrengthLevel}";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    Plugin.LoadStats();
                    break;
                }
                
            }
        }
    }
}
