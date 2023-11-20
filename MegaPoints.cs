


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
        internal const int MaxMegaUpgradeLevel = 1;

        public enum MegaUpgradeType
        {
            PlayerStamina,
            ToolStamina,
        }

        public async static void BuyMegaUpgrade(MegaUpgradeType megaUpgrade)
        {
            PlayerStatsFunctions.LoadStats();
            PlayerStatsFunctions.PostMessage($"Trying To Buy Upgrade: {megaUpgrade}");

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
                    Stamina.SetTreeSwingStamina(MegaPoints.currentMeleeAndTreeHitStaminaLevel);
                    Stamina.SetSetMeleeStamina(MegaPoints.currentMeleeAndTreeHitStaminaLevel);
                    Stamina.SetPlayerStamina(MegaPoints.currentPlayerStaminaLevel);
                    SetMegaUpgradeLevel(megaUpgrade, newLevel);
                    UpdateMegaUI(megaUpgrade);
                    PlayerStatsFunctions.PostMessage($"Bougth Upgrade: {megaUpgrade}");
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
                PlayerUpgradeStatsUi.PlayerStaminaCost.Text(costInfo);
                PlayerUpgradeStatsUi.PlayerStaminaBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.PlayerStaminaLvl.Text(lvlInfo);
            }
            else if (megaUpgradeType == MegaUpgradeType.ToolStamina)
            {
                PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaCost.Text(costInfo);
                PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaBonus.Text(speedInfo);
                PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaLvl.Text(lvlInfo);
            }

            PlayerUpgradeStatsUi.DisplayedPoints_megaPanel.Text($"Special Points: {PlayerStatsFunctions.currentPointsMega}");
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
            PlayerUpgradeStatsUi.displayMessage_megaPanel.Visible(true);
            if (maxORpoints)
            {
                PlayerUpgradeStatsUi.displayMessage_megaPanel.Text("You Have Maximim Lvl");
            }
            else
            {
                PlayerUpgradeStatsUi.displayMessage_megaPanel.Text("You Do Not Have Enogth Points");
            }
            isRunning = true;
            await Task.Run(WarningTimer);
            PlayerUpgradeStatsUi.displayMessage_megaPanel.Visible(false);
        }
    }
}
