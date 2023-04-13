using Sons.Gameplay.GameSetup;
using Sons.Save;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UnityEngine;
using static PlayerUpgradeStats.Plugin;

namespace PlayerUpgradeStats
{
    public class DataHandler
    {
        public static void WriteDynamicJsonObject(JsonObject jsonObj, string fileName)
        {
            using var fileStream = File.Create("PlayerUpgradeStatsData/" + fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(fileStream);
            jsonObj.WriteTo(utf8JsonWriter);
        }
        public static void SaveData()
        {
            Plugin.PostLogsToConsole("Saving");
            if (PlayerStatsPatcher.postfixSaveID != 0)
            {
                var fileName = $"{PlayerStatsPatcher.postfixSaveID}.json";
                var jsonObj = new JsonObject
                {
                    ["WorldID"] = PlayerStatsPatcher.postfixSaveID,
                    ["pointsUsed"] = pointsUsed,
                    ["currentPoints"] = currentPoints,
                    ["currentWalkSpeedLevel"] = BuyUpgrades.currentWalkSpeedLevel,
                    ["currentSprintSpeedLevel"] = BuyUpgrades.currentSprintSpeedLevel,
                    ["currentJumpHeightLevel"] = BuyUpgrades.currentJumpHeightLevel,
                    ["currentSwimSpeedLevel"] = BuyUpgrades.currentSwimSpeedLevel
                };
                DataHandler.WriteDynamicJsonObject(jsonObj, fileName);
            }
        }
        public static void GetData()
        {
            if (PlayerStatsPatcher.postfixSaveID != 0)
            {
                if (File.Exists("PlayerUpgradeStatsData/" + $"{PlayerStatsPatcher.postfixSaveID}.json"))
                {
                    PostLogsToConsole("Save File For World Found");
                    string text = File.ReadAllText($"PlayerUpgradeStatsData/{PlayerStatsPatcher.postfixSaveID}.json");
                    var saveInfo = JsonSerializer.Deserialize<SavedInfo>(text);
                    PostLogsToConsole("Updating Values");
                    PostLogsToConsole("postfixSaveID = " + PlayerStatsPatcher.postfixSaveID + "  FromFileWorldID = " + saveInfo.WorldID);
                    if (PlayerStatsPatcher.postfixSaveID == saveInfo.WorldID)
                    {
                        pointsUsed = saveInfo.pointsUsed;
                        currentPoints = saveInfo.currentPoints;
                        BuyUpgrades.currentWalkSpeedLevel = saveInfo.currentWalkSpeedLevel;
                        BuyUpgrades.currentSprintSpeedLevel = saveInfo.currentSprintSpeedLevel;
                        BuyUpgrades.currentJumpHeightLevel = saveInfo.currentJumpHeightLevel;
                        BuyUpgrades.currentSwimSpeedLevel = saveInfo.currentSwimSpeedLevel;
                        UpdateDisplayedData();

                    }

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
        }
    }
}
