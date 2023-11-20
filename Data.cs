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
using Sons.Gui;

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
            if (PlayerUpgradeStatsPatches.postfixSaveID != 0 && PlayerStatsFunctions.currentPoints >= 0)
            {
                PlayerStatsFunctions.PostMessage("Saving");
                if (PlayerStatsFunctions.currentPoints > 50) { PlayerStatsFunctions.PostError("From Savedata() CurrentPoints can't be over 50"); return; }
                var fileName = $"{PlayerUpgradeStatsPatches.postfixSaveID}.json";
                var jsonObj = new JsonObject
                {
                    ["WorldID"] = PlayerUpgradeStatsPatches.postfixSaveID,
                    ["pointsUsed"] = PlayerStatsFunctions.pointsUsed,
                    ["currentPoints"] = PlayerStatsFunctions.currentPoints,
                    ["pointsUsedMega"] = PlayerStatsFunctions.pointsUsedMega,
                    ["currentPointsMega"] = PlayerStatsFunctions.currentPointsMega,
                    ["currentWalkSpeedLevel"] = BuyUpgrades.currentWalkSpeedLevel,
                    ["currentSprintSpeedLevel"] = BuyUpgrades.currentSprintSpeedLevel,
                    ["currentJumpHeightLevel"] = BuyUpgrades.currentJumpHeightLevel,
                    ["currentSwimSpeedLevel"] = BuyUpgrades.currentSwimSpeedLevel,
                    ["currentChainsawSpeedLevel"] = BuyUpgrades.currentChainsawSpeedLevel,
                    ["currentKnightVSpeedLevel"] = BuyUpgrades.currentKnightVSpeedLevel,
                    ["currentBowDamageLevel"] = BuyUpgrades.currentBowDamageLevel,
                    ["currentMeleeAndTreeHitStaminaLevel"] = MegaPoints.currentMeleeAndTreeHitStaminaLevel,
                    ["currentPlayerStaminaLevel"] = MegaPoints.currentPlayerStaminaLevel
                };
                DataHandler.WriteDynamicJsonObject(jsonObj, fileName);
            }
        }
        internal static void GetData()
        {
            if (PlayerUpgradeStatsPatches.postfixSaveID != 0)
            {
                if (File.Exists("PlayerUpgradeStatsData/" + $"{PlayerUpgradeStatsPatches.postfixSaveID}.json"))
                {
                    PlayerStatsFunctions.PostMessage("Save File For World Found");
                    try
                    {
                        string text = File.ReadAllText($"PlayerUpgradeStatsData/{PlayerUpgradeStatsPatches.postfixSaveID}.json");
                        var saveInfo = JsonSerializer.Deserialize<SavedInfo>(text);
                        PlayerStatsFunctions.PostMessage("Updating Values");
                        PlayerStatsFunctions.PostMessage("postfixSaveID = " + PlayerUpgradeStatsPatches.postfixSaveID + "  FromFileWorldID = " + saveInfo.WorldID);
                        if (PlayerUpgradeStatsPatches.postfixSaveID == saveInfo.WorldID)
                        {
                            if (saveInfo.currentPoints > 50) { PlayerStatsFunctions.PostError("CurrentPoints can't be over 50"); return; }
                            PlayerStatsFunctions.pointsUsed = saveInfo.pointsUsed;
                            PlayerStatsFunctions.currentPoints = saveInfo.currentPoints;
                            PlayerStatsFunctions.pointsUsedMega = saveInfo.pointsUsedMega;
                            PlayerStatsFunctions.currentPointsMega = saveInfo.currentPointsMega;
                            BuyUpgrades.currentWalkSpeedLevel = saveInfo.currentWalkSpeedLevel;
                            BuyUpgrades.currentSprintSpeedLevel = saveInfo.currentSprintSpeedLevel;
                            BuyUpgrades.currentJumpHeightLevel = saveInfo.currentJumpHeightLevel;
                            BuyUpgrades.currentSwimSpeedLevel = saveInfo.currentSwimSpeedLevel;
                            BuyUpgrades.currentChainsawSpeedLevel = saveInfo.currentChainsawSpeedLevel;
                            BuyUpgrades.currentKnightVSpeedLevel = saveInfo.currentKnightVSpeedLevel;
                            BuyUpgrades.currentBowDamageLevel = saveInfo.currentBowDamageLevel;
                            MegaPoints.currentMeleeAndTreeHitStaminaLevel = saveInfo.currentMeleeAndTreeHitStaminaLevel;
                            MegaPoints.currentPlayerStaminaLevel = saveInfo.currentPlayerStaminaLevel;

                            UpdateDisplayedData();
                            UpdateDisplayedCost();
                        }
                    }
                    catch (System.Exception e)
                    {
                        PlayerStatsFunctions.PostError("Something went wrong trying to load saved data from file. Error: " + e);
                        PlayerUpgradeStatsUi.displayMessage_errorPanel.Text("Something went wrong trying to load saved data from file, please delete mod save data to fix issue");
                        PlayerUpgradeStatsUi.CustomPanelsActions(PlayerUpgradeStatsUi.ErrorPanel, true);
                    }

                }
                else { PlayerStatsFunctions.PostMessage("File Not Found"); }
            }
        }

        
        public class SavedInfo
        {
            public uint WorldID { get; set; }
            public int pointsUsed { get; set; }
            public int currentPoints { get; set; }
            public int pointsUsedMega { get; set; }
            public int currentPointsMega { get; set; }
            public float currentWalkSpeedLevel { get; set; }
            public float currentSprintSpeedLevel { get; set; }
            public float currentJumpHeightLevel { get; set; }
            public float currentSwimSpeedLevel { get; set; }
            public float currentChainsawSpeedLevel { get; set; }
            public float currentKnightVSpeedLevel { get; set; }
            public float currentBowDamageLevel { get; set; }
            public float currentMeleeAndTreeHitStaminaLevel { get; set; }
            public float currentPlayerStaminaLevel { get; set; }

        }
        public static void UpdateDisplayedData()
        {
            PlayerUpgradeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
            float totalwalkSpeedIncrease = BuyUpgrades.currentWalkSpeedLevel * 20;
            PlayerUpgradeStatsUi.WalkSpeedBonus.Text($"Speed: +{totalwalkSpeedIncrease}%");
            PlayerUpgradeStatsUi.WalkSpeedLvl.Text($"Lvl {BuyUpgrades.currentWalkSpeedLevel}/5");

            float totalSprintSpeedIncrease = BuyUpgrades.currentSprintSpeedLevel * 20;
            PlayerUpgradeStatsUi.SprintSpeedBonus.Text($"Speed: +{totalSprintSpeedIncrease}%");
            PlayerUpgradeStatsUi.SprintSpeedLvl.Text($"Lvl {BuyUpgrades.currentSprintSpeedLevel}/5");


            float totalJumpHeightIncrease = BuyUpgrades.currentJumpHeightLevel * 20;
            PlayerUpgradeStatsUi.JumpHeightBonus.Text($"Speed: +{totalJumpHeightIncrease}%");
            PlayerUpgradeStatsUi.JumpHeighLvl.Text($"Lvl {BuyUpgrades.currentJumpHeightLevel}/5");

            float totalSwimSpeedIncrease = BuyUpgrades.currentSwimSpeedLevel * 20;
            PlayerUpgradeStatsUi.SwimSpeedBonus.Text($"Speed: +{totalSwimSpeedIncrease}%");
            PlayerUpgradeStatsUi.SwimSpeedLvl.Text($"Lvl {BuyUpgrades.currentSwimSpeedLevel}/5");

            float totalChainsawSpeedIncrease = BuyUpgrades.currentChainsawSpeedLevel * 20;
            PlayerUpgradeStatsUi.ChainSawSpeedBonus.Text($"Speed: +{totalChainsawSpeedIncrease}%");
            PlayerUpgradeStatsUi.ChainSawSpeedLvl.Text($"Lvl {BuyUpgrades.currentChainsawSpeedLevel}/5");

            float totalKnightVSpeedIncrease = BuyUpgrades.currentKnightVSpeedLevel * 20;
            PlayerUpgradeStatsUi.KnightVSpeedBonus.Text($"Speed: +{totalKnightVSpeedIncrease}%");
            PlayerUpgradeStatsUi.KnightVSpeedLvl.Text($"Lvl {BuyUpgrades.currentKnightVSpeedLevel}/5");

            float totalBowDamageIncrease = BuyUpgrades.currentBowDamageLevel * 20;
            PlayerUpgradeStatsUi.BowDamageBonus.Text($"Speed: +{totalBowDamageIncrease}%");
            PlayerUpgradeStatsUi.BowDamageLvl.Text($"Lvl {BuyUpgrades.currentBowDamageLevel}/5");

            float totalPlayerStaminaIncrease = MegaPoints.currentPlayerStaminaLevel * Config.UpdateMegaUIIncreace;
            PlayerUpgradeStatsUi.PlayerStaminaBonus.Text($"Bonus: +{totalPlayerStaminaIncrease}%");
            PlayerUpgradeStatsUi.PlayerStaminaLvl.Text($"Lvl: {MegaPoints.currentPlayerStaminaLevel}/1");

            float totalToolStaminaIncrease = MegaPoints.currentMeleeAndTreeHitStaminaLevel * Config.UpdateMegaUIIncreace;
            PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaBonus.Text($"Bonus: +{totalToolStaminaIncrease}%");
            PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaLvl.Text($"Lvl {MegaPoints.currentMeleeAndTreeHitStaminaLevel}/1");

            
        }
        public static void UpdateDisplayedCost()
        {
            if (BuyUpgrades.currentWalkSpeedLevel >= 2) { PlayerUpgradeStatsUi.WalkSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentSprintSpeedLevel >= 2) { PlayerUpgradeStatsUi.SprintSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentJumpHeightLevel >= 2) { PlayerUpgradeStatsUi.JumpHeighCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentSwimSpeedLevel >= 2) { PlayerUpgradeStatsUi.SwimSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentChainsawSpeedLevel >= 2) { PlayerUpgradeStatsUi.ChainSawSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentKnightVSpeedLevel >= 2) { PlayerUpgradeStatsUi.KnightVSpeedCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
            if (BuyUpgrades.currentBowDamageLevel >= 2) { PlayerUpgradeStatsUi.BowDamageCost.Text($"Cost: {BuyUpgrades.pointPriceText4}"); }
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
                    PlayerUpgradeStatsPatches.currentStrengthLevel = LocalPlayer.Vitals.CurrentStrengthLevel;
                    if (PlayerUpgradeStatsPatches.currentStrengthLevel >= 10)
                    {
                        PlayerUpgradeStatsUi.OpenMegaPointsButton.Visible(true);
                    }
                    PlayerUpgradeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
                    PlayerUpgradeStatsUi.DisplayedPoints_megaPanel.Text($"Special Points: {PlayerStatsFunctions.currentPointsMega}");
                    PlayerUpgradeStatsUi.CurrentStrengthLevel.Text($"Strength Level: {PlayerUpgradeStatsPatches.currentStrengthLevel}");
                    PlayerStatsFunctions.LoadStats();
                    break;
                }

            }
        }
    }
}
