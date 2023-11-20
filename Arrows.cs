using Sons.Items.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;

namespace PlayerUpdadeStats
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
            item.Ammo.ProjectileInfo.muzzleDamage = defaultDamage * (currentBowDamageLevel * 20 / 100 + 1);
        }
    }
}
