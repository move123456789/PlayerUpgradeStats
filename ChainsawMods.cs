using Sons.Items.Core;
using Sons.Weapon;
using TheForest.Utils;

namespace PlayerUpdadeStats
{
    public class ChainsawMods
    {

        public static void SetChainSawSpeed(float currentChainsawSpeedLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetChainSawSpeed returned, player not in world"); return; }
            if (currentChainsawSpeedLevel == 0) { PlayerStatsFunctions.PostMessage("SetChainSawSpeed Returned: currentChainsawSpeedLevel == 0"); return; }

            SetChainSawSpeedLevel(394, 0.25f, currentChainsawSpeedLevel); // ChainSaw

        }

        private static void SetChainSawSpeedLevel(int itemId, float defaultDamage, float currentChainsawSpeedLevel)
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            item.HeldPrefab.gameObject.GetComponentInChildren<ChainsawWeaponController>()._treeHitFrequency = 0.25f * (1 - currentChainsawSpeedLevel * 19 / 100);
        }
    }
}
