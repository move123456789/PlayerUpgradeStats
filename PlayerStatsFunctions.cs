using RedLoader;
using Sons.Gui;
using Sons.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using UnityEngine;

namespace PlayerUpdadeStats
{
    internal class PlayerStatsFunctions
    {
        public static int currentPoints;
        public static int pointsUsed;

        // Level 10 Points
        public static int currentPointsMega;
        public static int pointsUsedMega;

        internal static void PostMessage(string message)
        {
            if (Config.DebugLogging.Value != true) { return; }
            RLog.Msg(message);
        }
        internal static void PostError(string message)
        {
            RLog.Warning($"[Player Upgrade Stats] {message}");
        }

        public static void LoadStats()
        {
            PlayerUpdadeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
            PlayerUpdadeStatsUi.DisplayedPoints_megaPanel.Text($"Special Points: {PlayerStatsFunctions.currentPointsMega}");
            GetCurrentPoints(PlayerUpdadeStatsPatches.currentStrengthLevel, pointsUsed);
            GetCurrentMegaPoints(PlayerUpdadeStatsPatches.currentStrengthLevel, pointsUsedMega);
            Stamina.SetTreeSwingStamina(MegaPoints.currentMeleeAndTreeHitStaminaLevel);
            Stamina.SetSetMeleeStamina(MegaPoints.currentMeleeAndTreeHitStaminaLevel);
            Stamina.SetPlayerStamina(MegaPoints.currentPlayerStaminaLevel);
            Arrows.SetBowDamage(BuyUpgrades.currentBowDamageLevel);
            ChainsawMods.SetChainSawSpeed(BuyUpgrades.currentChainsawSpeedLevel);
        }

        public static int GetCurrentPoints(int currentStrengthLevel, int pointsUsed)
        {
            if (currentStrengthLevel == 0)
            {
                return currentPoints = 0;
            }
            currentPoints = currentStrengthLevel - pointsUsed;
            return currentPoints;
        }

        public static int GetPointsUsed()
        {
            return pointsUsed;
        }

        // Level 10 Points
        public static int GetCurrentMegaPoints(int currentStrengthLevel, int pointsUsedMega)
        {
            if (currentStrengthLevel < 10)
            {
                return currentPointsMega = 0;
            }

            int earnedPoints = currentStrengthLevel / 10;
            currentPointsMega = earnedPoints - pointsUsedMega;
            return currentPointsMega;
        }

        public static int GetMegaPointsUsed()
        {
            return pointsUsedMega;
        }


        public static void UpdateSpeed()
        {
            PlayerStatsFunctions.PostMessage("Updating Speeds");
            // For Walk Speed
            LocalPlayer._FpCharacter_k__BackingField._walkSpeed = PlayerUpdadeStats.originalWalkSpeed * (BuyUpgrades.currentWalkSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Walk Speed = " + LocalPlayer._FpCharacter_k__BackingField._walkSpeed);
            // For Sprint Speed
            LocalPlayer._FpCharacter_k__BackingField._runSpeed = PlayerUpdadeStats.originalSprintSpeed * (BuyUpgrades.currentSprintSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Sprint Speed = " + LocalPlayer._FpCharacter_k__BackingField._runSpeed);
            // For Jump Height
            LocalPlayer._FpCharacter_k__BackingField._jumpHeight = PlayerUpdadeStats.originalJumpHeight * (BuyUpgrades.currentJumpHeightLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Jump Height = " + LocalPlayer._FpCharacter_k__BackingField._jumpHeight);
            // For Swin Speed
            LocalPlayer._FpCharacter_k__BackingField._swimSpeed = PlayerUpdadeStats.originalSwimSpeed * (BuyUpgrades.currentSwimSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Swim Speed = " + LocalPlayer._FpCharacter_k__BackingField._swimSpeed);
        }

        public static void DeleteSavedPointsData(string fileName = null)
        {
            string dir = @"PlayerUpgradeStatsData";
            if (fileName == null)
            {
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    PostError("Could not delete file, because directory does not exist");
                }
                else
                {
                    // If directory exists, delete all files within it
                    string[] files = Directory.GetFiles(dir, "*.json");
                    foreach (string file in files)
                    {
                        File.Delete(file);
                        PostError($"Deleted File: {file}");
                    }
                }
            }
            else
            {
                string fileName_And_dir = Path.Combine(dir, fileName);
                if (Path.GetExtension(fileName_And_dir).ToLower() == ".json")
                {
                    File.Delete(fileName_And_dir);
                }
                else
                {
                    PostError($"File {fileName} is not a .json file and will not be deleted.");
                }
            }
        }



        public static void OnMenuKeyPressed()
        {
            if (Config.UiTesting.Value == true)
            {
                PlayerUpdadeStatsUi.ToggleMainPanel(); // THIS TOGGELS ALSO
                PlayerUpdadeStatsUi.OpenMegaPointsButton.Visible(true);  // Makes The Button Visible Still When You Are Under Lvl 10
                return;
            }
            if (!LocalPlayer.IsInWorld || TheForest.Utils.LocalPlayer.IsInInventory || LocalPlayer.Inventory.Logs.HasLogs) { return; }

            if (!PauseMenu.IsActive && PauseMenu._instance.CanBeOpened())
            {
                PauseMenu._instance.Open();
                PlayerUpdadeStatsUi.ToggleMainPanel();
            }
        }


    }

}
