using Sons.Items.Core;
using Sons.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerUpdadeStats
{
    internal class Stamina
    {
        public static void SetSetMeleeStamina(float currentMeleeStaminaLevel = 0)
        {
            if (currentMeleeStaminaLevel == 0) { PlayerStatsFunctions.PostMessage("SetSetMeleeStamina Returned: currentMeleeStaminaLevel == 0"); return; }

            SetStaminaCost(356, 12, currentMeleeStaminaLevel); // Modern Axe
            SetStaminaCost(431, 12, currentMeleeStaminaLevel); // Fire Axe
            SetStaminaCost(379, 7, currentMeleeStaminaLevel);  // Normal Axe
            SetStaminaCost(380, 4, currentMeleeStaminaLevel);  // Combat Knife
            SetStaminaCost(359, 6, currentMeleeStaminaLevel);  // Machete
            SetStaminaCost(396, 6, currentMeleeStaminaLevel);  // Taser Stick
            SetStaminaCost(474, 10, currentMeleeStaminaLevel); // Crafted Spear
            SetStaminaCost(503, 5, currentMeleeStaminaLevel);  // Torch
            SetStaminaCost(477, 8, currentMeleeStaminaLevel);  // Crafted Club
            SetStaminaCost(525, 5, currentMeleeStaminaLevel);  // Putter
            SetStaminaCost(392, 5, currentMeleeStaminaLevel);  // Stick
            SetStaminaCost(367, 8, currentMeleeStaminaLevel);  // Katana
            SetStaminaCost(663, 7, currentMeleeStaminaLevel);  // Pickaxe
            SetStaminaCost(405, 6, currentMeleeStaminaLevel);  // Bone
            SetStaminaCost(394, 12.5f, currentMeleeStaminaLevel); // Chainsaw
            SetStaminaCost(340, 8, currentMeleeStaminaLevel);  // Guitar

            // SHOVEL FIX LATER
            //ItemData shovel = ItemDatabaseManager.ItemById(485);
            //shovel.MeleeWeaponData.StaminaCost = Config.testStamina.Value;
            //float shovel_default = 0;  // NEED TO BE DONE IN ANOTHER WAY
        }

        private static void SetStaminaCost(int itemId, float defaultStamina, float currentMeleeStaminaLevel)
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            item.MeleeWeaponData.StaminaCost = defaultStamina - (1.20f * currentMeleeStaminaLevel);
        }

        public static void SetTreeSwingStamina(float currentMeleeStaminaLevel = 0)
        {
            if (currentMeleeStaminaLevel == 0) { PlayerStatsFunctions.PostMessage("SetSetTreeSwingStamina Returned: currentMeleeStaminaLevel == 0"); return; }


        }

        private static void SetTreeSwingStaminaCost<T>(int itemId, float defaultStamina, float currentMeleeStaminaLevel) where T : MonoBehaviour
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            item.HeldPrefab.gameObject.GetComponent<ModernAxeWeaponController>()._treeSwingStaminaCost = defaultStamina - (1.20f * currentMeleeStaminaLevel);
        }
    }
}
