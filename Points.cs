using Sons.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PlayerUpgradeStats.Plugin;

namespace PlayerUpgradeStats
{
    public class BuyUpgrades
    {
        public static float currentWalkSpeedLevel;
        private static bool isRunning;
        private const int maxWalkSpeedLevel = 5;
        public static float currentSprintSpeedLevel;
        public static float currentJumpHeightLevel;
        public static float currentSwimSpeedLevel;
        public async static void BuyWalkSpeed()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentWalkSpeedLevel == maxWalkSpeedLevel)
            {
                if (currentWalkSpeedLevel < maxWalkSpeedLevel)
                {
                    currentWalkSpeedLevel++;
                    if (currentWalkSpeedLevel <= 2)
                    {
                        if (currentPoints < 2) { await Task.Run(DisplayWalkUpgradeWarning); return; }
                        currentPoints -=2;
                        pointsUsed += 2;
                    }
                    if (currentWalkSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await Task.Run(DisplayWalkUpgradeWarning); return; }
                        currentPoints -=4;
                        pointsUsed += 4;
                        MyPanel.walkSpeedCost.text = "Cost: 4";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentWalkSpeedLevel = " + currentWalkSpeedLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalwalkSpeedIncrease = currentWalkSpeedLevel * 20;
                    MyPanel.walkSpeedIncrease.text = $"Speed: +{totalwalkSpeedIncrease}%" + $"  Level {currentWalkSpeedLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
                    


                } else if (currentWalkSpeedLevel == maxWalkSpeedLevel)
                {
                    if (isRunning) { return; }
                    MyPanel.walkMaxLevel.enabled = true;
                    isRunning = true;
                    await Task.Run(WarningTimer);
                    MyPanel.walkMaxLevel.enabled = false;
                }
                
            }
            else
            {
                if (isRunning) { return; }
                MyPanel.walkNotEnogthPoints.enabled = true;
                isRunning = true;
                await Task.Run(WarningTimer);
                MyPanel.walkNotEnogthPoints.enabled = false;
            }
        }

        public async static void BuySprintSpeed()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentSprintSpeedLevel == maxWalkSpeedLevel)
            {
                if (currentSprintSpeedLevel < maxWalkSpeedLevel)
                {
                    currentSprintSpeedLevel++;
                    if (currentSprintSpeedLevel <= 2)
                    {
                        if (currentPoints < 2) { await Task.Run(DisplaySprintUpgradeWarning); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentSprintSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await Task.Run(DisplaySprintUpgradeWarning); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.sprintSpeedCost.text = "Cost: 4";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentSprintSpeedLevel = " + currentSprintSpeedLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalSprintSpeedIncrease = currentSprintSpeedLevel * 20;
                    MyPanel.sprintSpeedIncrease.text = $"Speed: +{totalSprintSpeedIncrease}%" + $"  Level {currentSprintSpeedLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";



                }
                else if (currentSprintSpeedLevel == maxWalkSpeedLevel)
                {
                    if (isRunning) { return; }
                    MyPanel.sprintMaxLevel.enabled = true;
                    isRunning = true;
                    await Task.Run(WarningTimer);
                    MyPanel.sprintMaxLevel.enabled = false;
                }

            }
            else
            {
                if (isRunning) { return; }
                MyPanel.sprintNotEnogthPoints.enabled = true;
                isRunning = true;
                await Task.Run(WarningTimer);
                MyPanel.sprintNotEnogthPoints.enabled = false;
            }
        }

        public async static void BuyJumpHeight()
        {
            Plugin.LoadStats();

            if (Plugin.currentPoints > 0 || currentJumpHeightLevel == maxWalkSpeedLevel)
            {
                if (currentJumpHeightLevel < maxWalkSpeedLevel)
                {
                    currentJumpHeightLevel++;
                    if (currentJumpHeightLevel <= 2)
                    {
                        if (currentPoints < 2) { await Task.Run(DisplayJumpUpgradeWarning); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentJumpHeightLevel > 2)
                    {
                        if (currentPoints < 4) { await Task.Run(DisplayJumpUpgradeWarning); return; }
                        currentPoints -= 4;
                        pointsUsed += 4;
                        MyPanel.jumpHeightCost.text = "Cost: 4";
                    }
                    doUpdateSpeeds = true;
                    PostLogsToConsole("currentJumpHeightLevel = " + currentJumpHeightLevel);
                    PostLogsToConsole("currentPoints = " + Plugin.currentPoints);
                    PostLogsToConsole("pointsUsed = " + pointsUsed);
                    float totalJumpHeightIncrease = currentJumpHeightLevel * 20;
                    MyPanel.jumpHeightIncrease.text = $"Height: +{totalJumpHeightIncrease}%" + $"  Level {currentJumpHeightLevel}/5";
                    MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";



                }
                else if (currentJumpHeightLevel == maxWalkSpeedLevel)
                {
                    if (isRunning) { return; }
                    MyPanel.jumpMaxLevel.enabled = true;
                    isRunning = true;
                    await Task.Run(WarningTimer);
                    MyPanel.jumpMaxLevel.enabled = false;
                }

            }
            else
            {
                if (isRunning) { return; }
                MyPanel.jumpNotEnogthPoints.enabled = true;
                isRunning = true;
                await Task.Run(WarningTimer);
                MyPanel.jumpNotEnogthPoints.enabled = false;
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
                    if (currentSwimSpeedLevel <= 2)
                    {
                        if (currentPoints < 2) { await Task.Run(DisplaySwimUpgradeWarning); return; }
                        currentPoints -= 2;
                        pointsUsed += 2;
                    }
                    if (currentSwimSpeedLevel > 2)
                    {
                        if (currentPoints < 4) { await Task.Run(DisplaySwimUpgradeWarning); return; }
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



                }
                else if (currentSwimSpeedLevel == maxWalkSpeedLevel)
                {
                    if (isRunning) { return; }
                    MyPanel.swimMaxLevel.enabled = true;
                    isRunning = true;
                    await Task.Run(WarningTimer);
                    MyPanel.swimMaxLevel.enabled = false;
                }

            }
            else
            {
                if (isRunning) { return; }
                MyPanel.swimNotEnogthPoints.enabled = true;
                isRunning = true;
                await Task.Run(WarningTimer);
                MyPanel.swimNotEnogthPoints.enabled = false;
            }
        }

        public static async Task WarningTimer()
        {
            await Task.Delay(2500);
            isRunning = false;
        }

        public static async Task DisplayWalkUpgradeWarning()
        {
            if (isRunning) { return; }
            MyPanel.walkNotEnogthPoints.enabled = true;
            isRunning = true;
            await Task.Run(WarningTimer);
            MyPanel.walkNotEnogthPoints.enabled = false;
        }

        public static async Task DisplaySprintUpgradeWarning()
        {
            if (isRunning) { return; }
            MyPanel.sprintNotEnogthPoints.enabled = true;
            isRunning = true;
            await Task.Run(WarningTimer);
            MyPanel.sprintNotEnogthPoints.enabled = false;
        }

        public static async Task DisplayJumpUpgradeWarning()
        {
            if (isRunning) { return; }
            MyPanel.jumpNotEnogthPoints.enabled = true;
            isRunning = true;
            await Task.Run(WarningTimer);
            MyPanel.jumpNotEnogthPoints.enabled = false;
        }

        public static async Task DisplaySwimUpgradeWarning()
        {
            if (isRunning) { return; }
            MyPanel.swimNotEnogthPoints.enabled = true;
            isRunning = true;
            await Task.Run(WarningTimer);
            MyPanel.swimNotEnogthPoints.enabled = false;
        }
    }
}
