using Sons.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static PlayerUpgradeStats.Plugin;

namespace PlayerUpgradeStats
{
    public class BuyUpgrades
    {
        // Max Level Of All Upgrades
        private const int maxWalkSpeedLevel = 5;

        private static bool isRunning;
        // Current Upgrade Level of Each Stat
        public static float currentWalkSpeedLevel;
        public static float currentSprintSpeedLevel;
        public static float currentJumpHeightLevel;
        public static float currentSwimSpeedLevel;
        public static float currentChainsawSpeedLevel;
        public static float currentKnightVSpeedLevel;
        public static float currentBowDamageLevel;
        // Price of Upgrades for UI
        internal const string pointPriceText2 = "2";
        internal const string pointPriceText4 = "4";

        // FOR TESTING
        private const int MaxUpgradeLevel = 5;

        // Upgrade types
        public enum UpgradeType
        {
            WalkSpeed,
            SprintSpeed,
            JumpHeight,
            SwinSpeed,
            ChainSawSpeed,
            KnightVSpeed,
            BowDamage,
        }

        // Prices
        internal const int BasePrice = 2;
        internal const int HigherPrice = 4;

        public async static void BuyUpgrade(UpgradeType upgradeType)
        {
            Plugin.LoadStats();

            float currentLevel = GetCurrentUpgradeLevel(upgradeType);

            if (Plugin.currentPoints > 0 || currentLevel == MaxUpgradeLevel)
            {
                if (currentLevel < MaxUpgradeLevel)
                {
                    float newLevel = currentLevel + 1;
                    int cost = newLevel > 2 ? HigherPrice : BasePrice;

                    if (currentPoints < cost)
                    {
                        await DisplayWarning(GetNotEnoughPointsWarning(upgradeType));
                        return;
                    }

                    currentPoints -= cost;
                    pointsUsed += cost;
                    doUpdateSpeeds = true;
                    SetUpgradeLevel(upgradeType, newLevel);
                    UpdateUI(upgradeType);
                    DataHandler.SaveData();
                }
                else
                {
                    await DisplayWarning(GetMaxLevelWarning(upgradeType));
                }
            }
            else
            {
                await DisplayWarning(GetNotEnoughPointsWarning(upgradeType));
            }
        }

        private static Text GetNotEnoughPointsWarning(UpgradeType upgradeType)
        {
            switch (upgradeType)
            {
                case UpgradeType.WalkSpeed:
                    return MyPanel.walkNotEnogthPoints;
                case UpgradeType.SprintSpeed:
                    return MyPanel.sprintNotEnogthPoints;
                case UpgradeType.JumpHeight:
                    return MyPanel.jumpNotEnogthPoints;
                case UpgradeType.SwinSpeed:
                    return MyPanel.swimNotEnogthPoints;
                case UpgradeType.ChainSawSpeed:
                    return MyPanel.chainSawNotEnogthPoints;
                case UpgradeType.KnightVSpeed:
                    return MyPanel.knightVNotEnogthPoints;
                case UpgradeType.BowDamage:
                    return MyPanel.bowNotEnogthPoints;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }

        private static Text GetMaxLevelWarning(UpgradeType upgradeType)
        {
            switch (upgradeType)
            {
                case UpgradeType.WalkSpeed:
                    return MyPanel.walkMaxLevel;
                case UpgradeType.SprintSpeed:
                    return MyPanel.sprintMaxLevel;
                case UpgradeType.JumpHeight:
                    return MyPanel.jumpMaxLevel;
                case UpgradeType.SwinSpeed:
                    return MyPanel.swimMaxLevel;
                case UpgradeType.ChainSawSpeed:
                    return MyPanel.chainSawMaxLevel;
                case UpgradeType.KnightVSpeed:
                    return MyPanel.knightVMaxLevel;
                case UpgradeType.BowDamage:
                    return MyPanel.bowMaxLevel;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }


        private static float GetCurrentUpgradeLevel(UpgradeType upgradeType)
        {
            switch (upgradeType)
            {
                case UpgradeType.WalkSpeed:
                    return currentWalkSpeedLevel;
                case UpgradeType.SprintSpeed:
                    return currentSprintSpeedLevel;
                case UpgradeType.JumpHeight:
                    return currentJumpHeightLevel;
                case UpgradeType.SwinSpeed:
                    return currentSwimSpeedLevel;
                case UpgradeType.ChainSawSpeed:
                    return currentChainsawSpeedLevel;
                case UpgradeType.KnightVSpeed:
                    return currentKnightVSpeedLevel;
                case UpgradeType.BowDamage:
                    return currentBowDamageLevel;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }

        private static void SetUpgradeLevel(UpgradeType upgradeType, float newLevel)
        {
            switch (upgradeType)
            {
                case UpgradeType.WalkSpeed:
                    currentWalkSpeedLevel = newLevel;
                    break;
                case UpgradeType.SprintSpeed:
                    currentSprintSpeedLevel = newLevel;
                    break;
                case UpgradeType.JumpHeight:
                    currentJumpHeightLevel = newLevel;
                    break;
                case UpgradeType.SwinSpeed:
                    currentSwimSpeedLevel = newLevel;
                    break;
                case UpgradeType.ChainSawSpeed:
                    currentChainsawSpeedLevel = newLevel;
                    ChainSawModifications.UpgradeChainsawHitFrequency(currentChainsawSpeedLevel);
                    break;
                case UpgradeType.KnightVSpeed:
                    currentKnightVSpeedLevel = newLevel;
                    break;
                case UpgradeType.BowDamage:
                    currentBowDamageLevel = newLevel;
                    break;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }
        private static void UpdateUI(UpgradeType upgradeType)
        {
            float currentLevel = GetCurrentUpgradeLevel(upgradeType);
            int cost = currentLevel > 1 ? HigherPrice : BasePrice;
            float totalSpeedIncrease = currentLevel * 20;
            string levelInfo = $"Speed: +{totalSpeedIncrease}%  Level {currentLevel}/5";
            string costInfo = $"Cost: {cost}";

            if (upgradeType == UpgradeType.WalkSpeed)
            {
                MyPanel.walkSpeedCost.text = costInfo;
                MyPanel.walkSpeedIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.SprintSpeed)
            {
                MyPanel.sprintSpeedCost.text = costInfo;
                MyPanel.sprintSpeedIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.JumpHeight)
            {
                MyPanel.jumpHeightCost.text = costInfo;
                MyPanel.jumpHeightIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.SwinSpeed)
            {
                MyPanel.swimSpeedCost.text = costInfo;
                MyPanel.swimSpeedIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.ChainSawSpeed)
            {
                MyPanel.chainSawSpeedCost.text = costInfo;
                MyPanel.chainSawSpeedIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.KnightVSpeed)
            {
                MyPanel.knightVSpeedCost.text = costInfo;
                MyPanel.knightVSpeedIncrease.text = levelInfo;
            }
            else if (upgradeType == UpgradeType.BowDamage)
            {
                MyPanel.bowDamageCost.text = costInfo;
                MyPanel.bowDamageIncrease.text = $"Damage: +{totalSpeedIncrease}%" + $"  Level {currentBowDamageLevel}/5"; ;
            }

            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
        }

        // A Little Async Function that adds a interval time when the UI Text will show
        public static async Task WarningTimer()
        {
            await Task.Delay(2500);
            isRunning = false;
        }


        public static async Task DisplayWarning(Text WarningText)
        {
            if (isRunning) { return; }
            WarningText.enabled = true;
            isRunning = true;
            await Task.Run(WarningTimer);
            WarningText.enabled = false;
        }
    }
}
