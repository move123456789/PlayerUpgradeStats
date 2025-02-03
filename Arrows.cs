using RedLoader;
using Sons.Items.Core;
using System;
using TheForest.Utils;

namespace PlayerUpgradeStats
{
    public class Arrows
    {
        public static void SetBowDamage(float currentBowDamageLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetBowDamage returned, player not in world"); return; }
            if (currentBowDamageLevel == 0) { PlayerStatsFunctions.PostMessage("SetBowDamage Returned: currentBowDamageLevel == 0"); return; }

            SetArrowDamage(618, 30, currentBowDamageLevel); // 3dPrintedArrow
            SetArrowDamage(373, 35, currentBowDamageLevel); // TacticalBowAmmo
            SetArrowDamage(507, 20, currentBowDamageLevel); // CraftedArrow

        }

        private static void SetArrowDamage(int itemId, float defaultDamage, float currentBowDamageLevel)
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            try
            {
                if (item != null)
                {
                    item.Ammo.ProjectileInfo.muzzleDamage = defaultDamage * (currentBowDamageLevel * 20 / 100 + 1);
                }
                else { RLog.Error($"Failed To find item by id: {itemId}, In SetArrowDamage"); return; }
            } catch (Exception ex)
            {
                RLog.Error($"Failed To Set ArrowDamage for itemID: {itemId}! ERROR: {ex}");
            }
            
        }
    }
}
