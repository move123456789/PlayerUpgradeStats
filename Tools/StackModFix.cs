

using UnityEngine;

namespace PlayerUpgradeStats.Tools
{
    internal class StackModFix
    {
        public static bool IsStackModInstalled()
        {
            string dataPath = Application.dataPath;

            // sotfPath Are 1 Level Up From The DataPath
            string sotfPath = Directory.GetParent(dataPath).FullName;

            // Mods Path
            string modsPath = Path.Combine(sotfPath, "Mods");

            // StackMod Path
            string stackModPath = Path.Combine(modsPath, "StackMod.dll");

            // CustomStacks Path
            string customStacksPath = Path.Combine(modsPath, "CustomStacks.dll");

            // Check If The File Exists
            if (File.Exists(stackModPath) || File.Exists(customStacksPath))
            {
                DeactiveMaxArrowUi();
                return true;
            }
            return false;
        }

        public static bool uiDeactivated = false;

        public static void DeactiveMaxArrowUi()
        {
            if (uiDeactivated) return;
            uiDeactivated = true;
            PlayerUpgradeStatsUi.MaxArrowsLvl.Text("Disabled");
            PlayerUpgradeStatsUi.MaxArrowsLvl.FontColor(Color.red);
            PlayerUpgradeStatsUi.MaxArrowsBtn.OnClick(() => PlayerStatsFunctions.PostMessage("StackMod Installed, Max Arrows Ui Deactivated"));
            PlayerUpgradeStatsUi.MaxArrowsBonus.Text("Disabled");
            PlayerUpgradeStatsUi.MaxArrowsBonus.FontColor(Color.red);
            PlayerUpgradeStatsUi.MaxArrowsCost.Text("Disabled");
            PlayerUpgradeStatsUi.MaxArrowsCost.FontColor(Color.red);
        }

    }
}
