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
        public static void SetSetMeleeStamina(float currentBowDamageLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetSetMeleeStamina returned, player not in world"); return; }
            if (currentBowDamageLevel == 0) { PlayerStatsFunctions.PostMessage("SetSetMeleeStamina Returned: currentMeleeStaminaLevel == 0"); return; }

            SetArrowDamage(618, 12, currentBowDamageLevel); // 3dPrintedArrow

        }

        private static void SetArrowDamage(int itemId, float defaultDamage, float currentBowDamageLevel)
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            item.Ammo.ProjectileInfo.muzzleDamage = defaultDamage * (float)Math.Pow(Config.MeleeStaminaPercentageReduction, currentBowDamageLevel);
        }
    }
}
