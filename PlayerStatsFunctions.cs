using RedLoader;
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
            RLog.Msg($"[Player Upgrade Stats] {message}");
        }
        internal static void PostError(string message)
        {
            RLog.Warning($"[Player Upgrade Stats] {message}");
        }

        public static void LoadStats()
        {
            PlayerUpdadeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
            PlayerUpdadeStatsUi.DisplayedPoints_megaPanel.Text($"Points: {PlayerStatsFunctions.currentPointsMega}");
            GetCurrentPoints(PlayerUpdadeStatsPatches.currentStrengthLevel, pointsUsed);
            GetCurrentMegaPoints(PlayerUpdadeStatsPatches.currentStrengthLevel, pointsUsedMega);
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
    }

    internal class ChainSawModifications
    {
        private static float defaultChainsawHitFrequency = 0.25f;

        internal static ChainsawWeaponController GetChainSawComponent()
        {
            try
            {
                if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("GetChainSawComponent, Player Not In World"); return null; }
                if (LocalPlayer.Inventory.RightHandItem == null || LocalPlayer.Inventory.RightHandItem.ItemObject == null || LocalPlayer.Inventory.RightHandItem.ItemObject.name != "TacticalChainsawHeld") { PlayerStatsFunctions.PostMessage("Chainsaw name != TacticalChainsawHeld, so chainsaw is not in hand from GetChainSawComponent"); return null; }
                GameObject chainsawObject = GameObject.Find("TacticalChainsawHeld");
                if (chainsawObject == null) { PlayerStatsFunctions.PostError("In GetChainSawComponent, Could not find 'TacticalChainsawHeld' object"); return null; }
                ChainsawWeaponController ComponentChainsawWeaponController = chainsawObject.GetComponent<ChainsawWeaponController>();
                if (ComponentChainsawWeaponController == null) { PlayerStatsFunctions.PostError("In GetChainSawComponent, Found Object == null"); return null; } else { PlayerStatsFunctions.PostError("Found ComponentChainsawWeaponController"); }
                return ComponentChainsawWeaponController;
            }
            catch (Exception e) { PlayerStatsFunctions.PostError("Could not run GetChainSawComponent Error: " + e); return null; }
        }

        internal static float ChainSawHitFrequency
        {
            get
            {
                if (GetChainSawComponent() == null) { PlayerStatsFunctions.PostMessage("In Get ChainSawHitFrequency, GetChainSawComponent == null"); return ChainSawHitFrequency = 0; }
                return GetChainSawComponent()._treeHitFrequency;
            }
            set
            {
                if (GetChainSawComponent() == null) { PlayerStatsFunctions.PostMessage("In Set ChainSawHitFrequency, GetChainSawComponent == null"); return; }
                try
                {
                    PlayerStatsFunctions.PostMessage("Setting _treeHitFrequency = value");
                    GetChainSawComponent()._treeHitFrequency = value;
                }
                catch (Exception e) { PlayerStatsFunctions.PostError("Could not set _treeHitFrequency Error: " + e); }
            }
        }

        internal static void UpgradeChainsawHitFrequency(float currentChainsawSpeedLevel)
        {
            PlayerStatsFunctions.PostMessage("In UpgradeChainsawHitFrequency");
            try
            {
                if (currentChainsawSpeedLevel == 0) { PlayerStatsFunctions.PostMessage("No Need To UpgradeChainsawHitFrequency, currentChainsawSpeedLevel == 0"); return; }
                if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("GetChainSawComponent, Player Not In World"); return; }
                if (LocalPlayer.Inventory.RightHandItem != null && LocalPlayer.Inventory.RightHandItem.ItemObject != null && LocalPlayer.Inventory.RightHandItem.ItemObject.name != "TacticalChainsawHeld") { PlayerStatsFunctions.PostMessage("Chainsaw name != TacticalChainsawHeld, so chainsaw is not in hand, from UpgradeChainsawHitFrequency"); return; }
                ChainSawHitFrequency = defaultChainsawHitFrequency * (1 - currentChainsawSpeedLevel * 19 / 100);
            }
            catch (Exception e) { PlayerStatsFunctions.PostError("Something went wrong in UpgradeChainsawHitFrequency, Error: " + e); }
            PlayerStatsFunctions.PostMessage("Current Chainsaw Speed = " + ChainSawHitFrequency);
        }
    }
}
