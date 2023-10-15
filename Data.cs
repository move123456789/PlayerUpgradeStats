using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using TheForest.Utils;
using static PlayerUpdadeStats.DataHandler;

namespace PlayerUpdadeStats
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
            PlayerStatsFunctions.PostMessage("Trying To Save");
            if (PlayerUpdadeStatsPatches.postfixSaveID != 0 && PlayerStatsFunctions.currentPoints >= 0)
            {
                PlayerStatsFunctions.PostMessage("Saving");
                if (PlayerStatsFunctions.currentPoints > 50) { PlayerStatsFunctions.PostError("From Savedata() CurrentPoints can't be over 50"); return; }
                var fileName = $"{PlayerUpdadeStatsPatches.postfixSaveID}.json";
                var jsonObj = new JsonObject
                {
                    ["WorldID"] = PlayerUpdadeStatsPatches.postfixSaveID,
                    ["pointsUsed"] = PlayerStatsFunctions.pointsUsed,
                    ["currentPoints"] = PlayerStatsFunctions.currentPoints,
                    ["currentWalkSpeedLevel"] = BuyUpgrades.currentWalkSpeedLevel,
                    ["currentSprintSpeedLevel"] = BuyUpgrades.currentSprintSpeedLevel,
                    ["currentJumpHeightLevel"] = BuyUpgrades.currentJumpHeightLevel,
                    ["currentSwimSpeedLevel"] = BuyUpgrades.currentSwimSpeedLevel,
                    ["currentChainsawSpeedLevel"] = BuyUpgrades.currentChainsawSpeedLevel,
                    ["currentKnightVSpeedLevel"] = BuyUpgrades.currentKnightVSpeedLevel,
                    ["currentBowDamageLevel"] = BuyUpgrades.currentBowDamageLevel
                };
                DataHandler.WriteDynamicJsonObject(jsonObj, fileName);
            }
        }
        internal static void GetData()
        {
            if (PlayerUpdadeStatsPatches.postfixSaveID != 0)
            {
                if (File.Exists("PlayerUpgradeStatsData/" + $"{PlayerUpdadeStatsPatches.postfixSaveID}.json"))
                {
                    PlayerStatsFunctions.PostMessage("Save File For World Found");
                    try
                    {
                        string text = File.ReadAllText($"PlayerUpgradeStatsData/{PlayerUpdadeStatsPatches.postfixSaveID}.json");
                        var saveInfo = JsonSerializer.Deserialize<SavedInfo>(text);
                        PlayerStatsFunctions.PostMessage("Updating Values");
                        PlayerStatsFunctions.PostMessage("postfixSaveID = " + PlayerUpdadeStatsPatches.postfixSaveID + "  FromFileWorldID = " + saveInfo.WorldID);
                        if (PlayerUpdadeStatsPatches.postfixSaveID == saveInfo.WorldID)
                        {
                            if (saveInfo.currentPoints > 50) { PlayerStatsFunctions.PostError("CurrentPoints can't be over 50"); return; }
                            PlayerStatsFunctions.pointsUsed = saveInfo.pointsUsed;
                            PlayerStatsFunctions.currentPoints = saveInfo.currentPoints;
                            BuyUpgrades.currentWalkSpeedLevel = saveInfo.currentWalkSpeedLevel;
                            BuyUpgrades.currentSprintSpeedLevel = saveInfo.currentSprintSpeedLevel;
                            BuyUpgrades.currentJumpHeightLevel = saveInfo.currentJumpHeightLevel;
                            BuyUpgrades.currentSwimSpeedLevel = saveInfo.currentSwimSpeedLevel;
                            BuyUpgrades.currentChainsawSpeedLevel = saveInfo.currentChainsawSpeedLevel;
                            BuyUpgrades.currentKnightVSpeedLevel = saveInfo.currentKnightVSpeedLevel;
                            BuyUpgrades.currentBowDamageLevel = saveInfo.currentBowDamageLevel;
                            UpdateDisplayedData();
                            UpdateDisplayedCost();
                        }
                    }
                    catch (System.Exception e) { PlayerStatsFunctions.PostError("Something went wrong trying to load saved data from file. Error: " + e); }

                }
                else { PlayerStatsFunctions.PostMessage("File Not Found"); }
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
            public float currentKnightVSpeedLevel { get; set; }
            public float currentBowDamageLevel { get; set; }

        }
        public static void UpdateDisplayedData()
        {
            PlayerUpdadeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
            float totalwalkSpeedIncrease = BuyUpgrades.currentWalkSpeedLevel * 20;
            PlayerUpdadeStatsUi.WalkSpeedBonus.Text($"Speed: +{totalwalkSpeedIncrease}%");
            PlayerUpdadeStatsUi.WalkSpeedLvl.Text($"Lvl {BuyUpgrades.currentWalkSpeedLevel}/5");

            float totalSprintSpeedIncrease = BuyUpgrades.currentSprintSpeedLevel * 20;
            PlayerUpdadeStatsUi.SprintSpeedBonus.Text($"Speed: +{totalSprintSpeedIncrease}%");
            PlayerUpdadeStatsUi.SprintSpeedLvl.Text($"Lvl {BuyUpgrades.currentSprintSpeedLevel}/5");


            float totalJumpHeightIncrease = BuyUpgrades.currentJumpHeightLevel * 20;
            PlayerUpdadeStatsUi.JumpHeightBonus.Text($"Speed: +{totalJumpHeightIncrease}%");
            PlayerUpdadeStatsUi.JumpHeighLvl.Text($"Lvl {BuyUpgrades.currentJumpHeightLevel}/5");

            float totalSwimSpeedIncrease = BuyUpgrades.currentSwimSpeedLevel * 20;
            PlayerUpdadeStatsUi.SwimSpeedBonus.Text($"Speed: +{totalSwimSpeedIncrease}%");
            PlayerUpdadeStatsUi.SwimSpeedLvl.Text($"Lvl {BuyUpgrades.currentSwimSpeedLevel}/5");

            float totalChainsawSpeedIncrease = BuyUpgrades.currentChainsawSpeedLevel * 20;
            PlayerUpdadeStatsUi.ChainSawSpeedBonus.Text($"Speed: +{totalChainsawSpeedIncrease}%");
            PlayerUpdadeStatsUi.ChainSawSpeedLvl.Text($"Lvl {BuyUpgrades.currentChainsawSpeedLevel}/5");

            float totalKnightVSpeedIncrease = BuyUpgrades.currentKnightVSpeedLevel * 20;
            PlayerUpdadeStatsUi.KnightVSpeedBonus.Text($"Speed: +{totalKnightVSpeedIncrease}%");
            PlayerUpdadeStatsUi.KnightVSpeedLvl.Text($"Lvl {BuyUpgrades.currentKnightVSpeedLevel}/5");

            float totalBowDamageIncrease = BuyUpgrades.currentBowDamageLevel * 20;
            PlayerUpdadeStatsUi.BowDamageBonus.Text($"Speed: +{totalBowDamageIncrease}%");
            PlayerUpdadeStatsUi.BowDamageLvl.Text($"Lvl {BuyUpgrades.currentBowDamageLevel}/5");
        }
        public static void UpdateDisplayedCost()
        {
            if (BuyUpgrades.currentWalkSpeedLevel >= 2) { PlayerUpdadeStatsUi.WalkSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentSprintSpeedLevel >= 2) { PlayerUpdadeStatsUi.SprintSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentJumpHeightLevel >= 2) { PlayerUpdadeStatsUi.JumpHeighCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentSwimSpeedLevel >= 2) { PlayerUpdadeStatsUi.SwimSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentChainsawSpeedLevel >= 2) { PlayerUpdadeStatsUi.ChainSawSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentKnightVSpeedLevel >= 2) { PlayerUpdadeStatsUi.KnightVSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentBowDamageLevel >= 2) { PlayerUpdadeStatsUi.BowDamageCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
        }

        public static async void GetStrengthLevelVitals()
        {
            while (true)
            {
                PlayerStatsFunctions.PostMessage("Strength Level = " + LocalPlayer.Vitals.CurrentStrengthLevel);
                if (LocalPlayer.Vitals.CurrentStrengthLevel == 1)
                {
                    await Task.Delay(5000);
                }
                else
                {
                    PlayerUpdadeStatsPatches.currentStrengthLevel = LocalPlayer.Vitals.CurrentStrengthLevel;
                    //MyPanel.curStrengthLvl.text = $"Your Strength Level: {PlayerUpdadeStatsPatches.currentStrengthLevel}";
                    PlayerUpdadeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
                    PlayerStatsFunctions.LoadStats();
                    break;
                }

            }
        }
    }
}
