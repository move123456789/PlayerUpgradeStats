using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerUpdadeStats
{
    public class BuyUpgrades
    {
        // Max Level Of All Upgrades
        private const int MaxUpgradeLevel = 5;

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

        // Upgrade types
        public enum UpgradeType
        {
            WalkSpeed,
            SprintSpeed,
            JumpHeight,
            SwimSpeed,
            ChainSawSpeed,
            KnightVSpeed,
            BowDamage,
        }

        // Prices
        internal const int BasePrice = 2;
        internal const int HigherPrice = 4;

        public async static void BuyUpgrade(UpgradeType upgradeType)
        {
            PlayerStatsFunctions.LoadStats();

            float currentLevel = GetCurrentUpgradeLevel(upgradeType);

            if (PlayerStatsFunctions.currentPoints > 0 || currentLevel == MaxUpgradeLevel)
            {
                if (currentLevel < MaxUpgradeLevel)
                {
                    float newLevel = currentLevel + 1;
                    int cost = newLevel > 2 ? HigherPrice : BasePrice;

                    if (PlayerStatsFunctions.currentPoints < cost)
                    {
                        await DisplayWarning(false);
                        return;
                    }

                    PlayerStatsFunctions.currentPoints -= cost;
                    PlayerStatsFunctions.pointsUsed += cost;
                    PlayerStatsFunctions.UpdateSpeed();
                    SetUpgradeLevel(upgradeType, newLevel);
                    UpdateUI(upgradeType);
                    DataHandler.SaveData();
                }
                else
                {
                    await DisplayWarning(true);
                }
            }
            else
            {
                await DisplayWarning(false);
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
                case UpgradeType.SwimSpeed:
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
                case UpgradeType.SwimSpeed:
                    currentSwimSpeedLevel = newLevel;
                    break;
                case UpgradeType.ChainSawSpeed:
                    currentChainsawSpeedLevel = newLevel;
                    ChainsawMods.SetChainSawSpeed(newLevel);
                    break;
                case UpgradeType.KnightVSpeed:
                    currentKnightVSpeedLevel = newLevel;
                    break;
                case UpgradeType.BowDamage:
                    currentBowDamageLevel = newLevel;
                    Arrows.SetBowDamage(newLevel);
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
            //string levelInfo = $"Speed: +{totalSpeedIncrease}%  Level {currentLevel}/5";  # OLD SYSTEM
            string lvlInfo = $"Lvl: {currentLevel}/5";
            string speedInfo = $"Speed: +{totalSpeedIncrease}%";
            string costInfo = $"Cost: {cost}";

            if (upgradeType == UpgradeType.WalkSpeed)
            {
                PlayerUpgradeStatsUi.WalkSpeedCost.Text(costInfo);
                PlayerUpgradeStatsUi.WalkSpeedBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.WalkSpeedLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.SprintSpeed)
            {
                PlayerUpgradeStatsUi.SprintSpeedCost.Text(costInfo);
                PlayerUpgradeStatsUi.SprintSpeedBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.SprintSpeedLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.JumpHeight)
            {
                PlayerUpgradeStatsUi.JumpHeighCost.Text(costInfo);
                PlayerUpgradeStatsUi.JumpHeightBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.JumpHeighLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.SwimSpeed)
            {
                PlayerUpgradeStatsUi.SwimSpeedCost.Text(costInfo);
                PlayerUpgradeStatsUi.SwimSpeedBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.SwimSpeedLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.ChainSawSpeed)
            {
                PlayerUpgradeStatsUi.ChainSawSpeedCost.Text(costInfo);
                PlayerUpgradeStatsUi.ChainSawSpeedBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.ChainSawSpeedLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.KnightVSpeed)
            {
                PlayerUpgradeStatsUi.KnightVSpeedCost.Text(costInfo);
                PlayerUpgradeStatsUi.KnightVSpeedBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.KnightVSpeedLvl.Text(lvlInfo);
            }
            else if (upgradeType == UpgradeType.BowDamage)
            {
                PlayerUpgradeStatsUi.BowDamageCost.Text(costInfo);
                PlayerUpgradeStatsUi.BowDamageBonus.Text($"Damage: +{totalSpeedIncrease}%");
                PlayerUpgradeStatsUi.BowDamageLvl.Text(lvlInfo);
            }

            PlayerUpgradeStatsUi.DisplayedPoints.Text($"Points: {PlayerStatsFunctions.currentPoints}");
        }

        // A Little Async Function that adds a interval time when the UI Text will show
        public static async Task WarningTimer()
        {
            await Task.Delay(2500);
            isRunning = false;
        }


        public static async Task DisplayWarning(bool maxORpoints)
        {
            if (isRunning) { return; }
            PlayerUpgradeStatsUi.displayMessage.Visible(true);
            if (maxORpoints)
            {
                PlayerUpgradeStatsUi.displayMessage.Text("You Have Maximim Lvl");
            }
            else
            {
                PlayerUpgradeStatsUi.displayMessage.Text("You Do Not Have Enogth Points");
            }
            isRunning = true;
            await Task.Run(WarningTimer);
            PlayerUpgradeStatsUi.displayMessage.Visible(false);
        }
    }
}
