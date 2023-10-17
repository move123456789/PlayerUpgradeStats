


namespace PlayerUpdadeStats
{
    public class MegaPoints
    {
        // Current Upgrade Level of Each Stat
        public static float currentMeleeAndTreeHitStaminaLevel;
        public static float currentPlayerStaminaLevel;

        // Prices
        internal const int MegaUpgradePrice = 1;

        // Misc
        private static bool isRunning;
        private const int MaxMegaUpgradeLevel = 1;

        public enum MegaUpgradeType
        {
            PlayerStamina,
            ToolStamina,
        }

        public async static void BuyMegaUpgrade(MegaUpgradeType megaUpgrade)
        {
            PlayerStatsFunctions.LoadStats();

            float currentLevel = GetCurrentMegaUpgradeLevel(megaUpgrade);

            if (PlayerStatsFunctions.currentPointsMega > 0 || currentLevel == MaxMegaUpgradeLevel)
            {
                if (currentLevel < MaxMegaUpgradeLevel)
                {
                    float newLevel = currentLevel + 1;
                    int cost = MegaUpgradePrice;

                    if (PlayerStatsFunctions.currentPointsMega < cost)
                    {
                        await DisplayMegaWarning(false);
                        return;
                    }

                    PlayerStatsFunctions.currentPointsMega -= cost;
                    PlayerStatsFunctions.pointsUsedMega += cost;
                    // DO THE METHODS TO ADD THE UPGRADE
                    SetMegaUpgradeLevel(megaUpgrade, newLevel);
                    UpdateMegaUI(megaUpgrade);
                    DataHandler.SaveData();
                }
                else
                {
                    await DisplayMegaWarning(true);
                }
            }
            else
            {
                await DisplayMegaWarning(false);
            }
        }

        private static float GetCurrentMegaUpgradeLevel(MegaUpgradeType upgradeType)
        {
            switch (upgradeType)
            {
                case MegaUpgradeType.PlayerStamina:
                    return currentPlayerStaminaLevel;
                case MegaUpgradeType.ToolStamina:
                    return currentMeleeAndTreeHitStaminaLevel;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }

        private static void SetMegaUpgradeLevel(MegaUpgradeType upgradeType, float newLevel)
        {
            switch (upgradeType)
            {
                case MegaUpgradeType.PlayerStamina:
                    currentPlayerStaminaLevel = newLevel;
                    Stamina.SetPlayerStamina(newLevel);
                    break;
                case MegaUpgradeType.ToolStamina:
                    currentMeleeAndTreeHitStaminaLevel = newLevel;
                    Stamina.SetTreeSwingStamina(newLevel);
                    Stamina.SetSetMeleeStamina(newLevel);
                    break;
                default:
                    throw new ArgumentException("Invalid upgrade type");
            }
        }

        private static void UpdateMegaUI(MegaUpgradeType megaUpgradeType)
        {
            float currentLevel = GetCurrentMegaUpgradeLevel(megaUpgradeType);
            float totalIncrease = currentLevel * Config.UpdateMegaUIIncreace;
            string lvlInfo = $"Lvl: {currentLevel}/1";
            string speedInfo = $"Bonus: +{totalIncrease}%";
            string costInfo = $"Cost: {MegaUpgradePrice}";

            if (megaUpgradeType == MegaUpgradeType.PlayerStamina)
            {
                PlayerUpdadeStatsUi.PlayerStaminaCost.Text(costInfo);
                PlayerUpdadeStatsUi.PlayerStaminaBonus.Text(speedInfo);
                PlayerUpdadeStatsUi.PlayerStaminaLvl.Text(lvlInfo);
            }
            else if (megaUpgradeType == MegaUpgradeType.ToolStamina)
            {
                PlayerUpdadeStatsUi.MeleeAndTreeHitStaminaCost.Text(costInfo);
                PlayerUpdadeStatsUi.MeleeAndTreeHitStaminaBonus.Text(speedInfo);
                PlayerUpdadeStatsUi.MeleeAndTreeHitStaminaLvl.Text(lvlInfo);
            }

            PlayerUpdadeStatsUi.DisplayedPoints_megaPanel.Text($"Special Points: {PlayerStatsFunctions.currentPointsMega}");
        }

        // A Little Async Function that adds a interval time when the UI Text will show
        public static async Task WarningTimer()
        {
            await Task.Delay(2500);
            isRunning = false;
        }


        private static async Task DisplayMegaWarning(bool maxORpoints)
        {
            if (isRunning) { return; }
            PlayerUpdadeStatsUi.displayMessage_megaPanel.Visible(true);
            if (maxORpoints)
            {
                PlayerUpdadeStatsUi.displayMessage_megaPanel.Text("You Have Maximim Lvl");
            }
            else
            {
                PlayerUpdadeStatsUi.displayMessage_megaPanel.Text("You Do Not Have Enogth Points");
            }
            isRunning = true;
            await Task.Run(WarningTimer);
            PlayerUpdadeStatsUi.displayMessage_megaPanel.Visible(false);
        }
    }
}
