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
            // other types...
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
                // other cases...
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
                // other cases...
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
                // other cases...
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
                // other cases...
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
            // Add other cases for other upgrade types

            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
        }


        // FOR TESTING








        //public async static void BuyWalkSpeed()
        //{
        //    Plugin.LoadStats();

        //    if (Plugin.currentPoints > 0 || currentWalkSpeedLevel == maxWalkSpeedLevel)
        //    {
        //        if (currentWalkSpeedLevel < maxWalkSpeedLevel)
        //        {
        //            currentWalkSpeedLevel++;
        //            if (currentWalkSpeedLevel < 2)
        //            {
        //                if (currentPoints < 2) { await DisplayWarning(MyPanel.walkNotEnogthPoints); return; }
        //                currentPoints -= 2;
        //                pointsUsed += 2;
        //            }
        //            if (currentWalkSpeedLevel == 2)
        //            {
        //                if (currentPoints < 2) { await DisplayWarning(MyPanel.walkNotEnogthPoints); return; }
        //                currentPoints -= 2;
        //                pointsUsed += 2;
        //                MyPanel.walkSpeedCost.text = $"Cost: {pointPriceText4}";
        //            }
        //            if (currentWalkSpeedLevel > 2)
        //            {
        //                if (currentPoints < 4) { await DisplayWarning(MyPanel.walkNotEnogthPoints); return; }
        //                currentPoints -= 4;
        //                pointsUsed += 4;
        //                MyPanel.walkSpeedCost.text = $"Cost: {pointPriceText4}";
        //            }
        //            doUpdateSpeeds = true;
        //            PostLogsToConsole("currentWalkSpeedLevel = " + currentWalkSpeedLevel);
        //            PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
        //            PostLogsToConsole("pointsUsed = " + pointsUsed);
        //            float totalwalkSpeedIncrease = currentWalkSpeedLevel * 20;
        //            MyPanel.walkSpeedIncrease.text = $"Speed: +{totalwalkSpeedIncrease}%" + $"  Level {currentWalkSpeedLevel}/5";
        //            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
        //            DataHandler.SaveData();


        //        }
        //        else if (currentWalkSpeedLevel == maxWalkSpeedLevel)
        //        {
        //            await DisplayWarning(MyPanel.walkMaxLevel);
        //        }

        //    }
        //    else
        //    {
        //        await DisplayWarning(MyPanel.walkNotEnogthPoints);
        //    }
        //}

        //public async static void BuySprintSpeed()
        //{
        //    Plugin.LoadStats();

        //    if (Plugin.currentPoints > 0 || currentSprintSpeedLevel == maxWalkSpeedLevel)
        //    {
        //        if (currentSprintSpeedLevel < maxWalkSpeedLevel)
        //        {
        //            currentSprintSpeedLevel++;
        //            if (currentSprintSpeedLevel < 2)
        //            {
        //                if (currentPoints < 2) { await DisplayWarning(MyPanel.sprintNotEnogthPoints); return; }
        //                currentPoints -= 2;
        //                pointsUsed += 2;
        //            }
        //            if (currentSprintSpeedLevel == 2)
        //            {
        //                if (currentPoints < 2) { await DisplayWarning(MyPanel.sprintNotEnogthPoints); return; }
        //                currentPoints -= 2;
        //                pointsUsed += 2;
        //                MyPanel.sprintSpeedCost.text = $"Cost: {pointPriceText4}";
        //            }
        //            if (currentSprintSpeedLevel > 2)
        //            {
        //                if (currentPoints < 4) { await DisplayWarning(MyPanel.sprintNotEnogthPoints); return; }
        //                currentPoints -= 4;
        //                pointsUsed += 4;
        //                MyPanel.sprintSpeedCost.text = $"Cost: {pointPriceText4}";
        //            }
        //            doUpdateSpeeds = true;
        //            PostLogsToConsole("currentSprintSpeedLevel = " + currentSprintSpeedLevel);
        //            PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
        //            PostLogsToConsole("pointsUsed = " + pointsUsed);
        //            float totalSprintSpeedIncrease = currentSprintSpeedLevel * 20;
        //            MyPanel.sprintSpeedIncrease.text = $"Speed: +{totalSprintSpeedIncrease}%" + $"  Level {currentSprintSpeedLevel}/5";
        //            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
        //            DataHandler.SaveData();


        //        }
        //        else if (currentSprintSpeedLevel == maxWalkSpeedLevel)
        //        {
        //            await DisplayWarning(MyPanel.sprintMaxLevel);
        //        }

        //    }
        //    else
        //    {
        //        await DisplayWarning(MyPanel.sprintNotEnogthPoints);
        //    }
        //}

        public async static void BuyJumpHeight()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentJumpHeightLevel == maxWalkSpeedLevel)
            {
                if (currentJumpHeightLevel < maxWalkSpeedLevel)
                {
                    currentJumpHeightLevel++;
                    if (currentJumpHeightLevel < 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.jumpNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentJumpHeightLevel == 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.jumpNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                        MyPanel.jumpHeightCost.text = $"Cost: {pointPriceText4}";
                    }
                    if (currentJumpHeightLevel > 2)
                    {
                        if (currentPoints < 4) { await DisplayWarning(MyPanel.jumpNotEnogthPoints); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.jumpHeightCost.text = $"Cost: {pointPriceText4}";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentJumpHeightLevel = " + currentJumpHeightLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalJumpHeightIncrease = currentJumpHeightLevel * 20;
                    MyPanel.jumpHeightIncrease.text = $"Height: +{totalJumpHeightIncrease}%" + $"  Level {currentJumpHeightLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    DataHandler.SaveData();


                }
                else if (currentJumpHeightLevel == maxWalkSpeedLevel)
                {
                    await DisplayWarning(MyPanel.jumpMaxLevel);
                }

            }
            else
            {
                await DisplayWarning(MyPanel.jumpNotEnogthPoints);
            }
        }

        public async static void BuySwimSpeed()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentSwimSpeedLevel == maxWalkSpeedLevel)
            {
                if (currentSwimSpeedLevel < maxWalkSpeedLevel)
                {
                    currentSwimSpeedLevel++;
                    if (currentSwimSpeedLevel < 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.swimNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentSwimSpeedLevel == 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.swimNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                        MyPanel.jumpHeightCost.text = $"Cost: {pointPriceText4}";
                    }
                    if (currentSwimSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await DisplayWarning(MyPanel.swimNotEnogthPoints); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.swimSpeedCost.text = "Cost: 4";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentSwimSpeedLevel = " + currentSwimSpeedLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalSwimSpeedIncrease = currentSwimSpeedLevel * 20;
                    MyPanel.swimSpeedIncrease.text = $"Speed: +{totalSwimSpeedIncrease}%" + $"  Level {currentSwimSpeedLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    DataHandler.SaveData();


                }
                else if (currentSwimSpeedLevel == maxWalkSpeedLevel)
                {
                    await DisplayWarning(MyPanel.swimMaxLevel);
                }

            }
            else
            {
                await DisplayWarning(MyPanel.swimNotEnogthPoints);
            }
        }
        public async static void BuyChainsawSpeed()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentChainsawSpeedLevel == maxWalkSpeedLevel)
            {
                if (currentChainsawSpeedLevel < maxWalkSpeedLevel)
                {
                    currentChainsawSpeedLevel++;
                    if (currentChainsawSpeedLevel < 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.chainSawNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentChainsawSpeedLevel == 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.chainSawNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                        MyPanel.chainSawSpeedCost.text = $"Cost: {pointPriceText4}";
                    }
                    if (currentChainsawSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await DisplayWarning(MyPanel.chainSawNotEnogthPoints); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.chainSawSpeedCost.text = $"Cost: {pointPriceText4}";
                    }
                    ChainSawModifications.UpgradeChainsawHitFrequency(currentChainsawSpeedLevel);
                    PostLogsToConsole("currentChainsawSpeedLevel = " + currentChainsawSpeedLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalchainSawSpeedIncrease = currentChainsawSpeedLevel * 20;
                    MyPanel.chainSawSpeedIncrease.text = $"Speed: +{totalchainSawSpeedIncrease}%" + $"  Level {currentChainsawSpeedLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    DataHandler.SaveData();


                }
                else if (currentChainsawSpeedLevel == maxWalkSpeedLevel)
                {
                    await DisplayWarning(MyPanel.chainSawMaxLevel);
                }

            }
            else
            {
                await DisplayWarning(MyPanel.chainSawNotEnogthPoints);
            }
        }

        public async static void BuyKnightVSpeed()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentKnightVSpeedLevel == maxWalkSpeedLevel)
            {
                if (currentKnightVSpeedLevel < maxWalkSpeedLevel)
                {
                    currentKnightVSpeedLevel++;
                    if (currentKnightVSpeedLevel < 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.knightVNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentKnightVSpeedLevel == 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.knightVNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                        MyPanel.jumpHeightCost.text = $"Cost: {pointPriceText4}";
                    }
                    if (currentKnightVSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await DisplayWarning(MyPanel.knightVNotEnogthPoints); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.knightVSpeedCost.text = $"Cost: {pointPriceText4}";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentKnightVSpeedLevel = " + currentKnightVSpeedLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalKnightVSpeedIncrease = currentKnightVSpeedLevel * 20;
                    MyPanel.knightVSpeedIncrease.text = $"Speed: +{totalKnightVSpeedIncrease}%" + $"  Level {currentKnightVSpeedLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    DataHandler.SaveData();


                }
                else if (currentKnightVSpeedLevel == maxWalkSpeedLevel)
                {
                    await DisplayWarning(MyPanel.knightVMaxLevel);
                }

            }
            else
            {
                await DisplayWarning(MyPanel.knightVNotEnogthPoints);
            }
        }

        public async static void BuyBowDamage()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentBowDamageLevel == maxWalkSpeedLevel)
            {
                if (currentBowDamageLevel < maxWalkSpeedLevel)
                {
                    currentBowDamageLevel++;
                    if (currentBowDamageLevel < 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.bowNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentBowDamageLevel == 2)
                    {
                        if (currentPoints < 2) { await DisplayWarning(MyPanel.bowNotEnogthPoints); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                        MyPanel.jumpHeightCost.text = $"Cost: {pointPriceText4}";
                    }
                    if (currentBowDamageLevel > 2)
                    {
                        if (currentPoints < 4) { await DisplayWarning(MyPanel.bowNotEnogthPoints); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.bowDamageCost.text = $"Cost: {pointPriceText4}";
                    }
                    PostLogsToConsole("currentBowDamageLevel = " + currentBowDamageLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalBowDamageIncrease = currentBowDamageLevel * 20;
                    MyPanel.bowDamageIncrease.text = $"Damage: +{totalBowDamageIncrease}%" + $"  Level {currentBowDamageLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    DataHandler.SaveData();


                }
                else if (currentBowDamageLevel == maxWalkSpeedLevel)
                {
                    await DisplayWarning(MyPanel.bowMaxLevel);
                }

            }
            else
            {
                await DisplayWarning(MyPanel.bowNotEnogthPoints);
            }
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
