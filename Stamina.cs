using Sons.Items.Core;
using Sons.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using UnityEngine;

namespace PlayerUpdadeStats
{
    internal class Stamina
    {
        public static void SetSetMeleeStamina(float currentMeleeStaminaLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetSetMeleeStamina returned, player not in world"); return; }
            if (currentMeleeStaminaLevel == 0) { PlayerStatsFunctions.PostMessage("SetSetMeleeStamina Returned: currentMeleeStaminaLevel == 0"); return; }

            SetMeleeStaminaCost(356, 12, currentMeleeStaminaLevel); // Modern Axe
            SetMeleeStaminaCost(431, 12, currentMeleeStaminaLevel); // Fire Axe
            SetMeleeStaminaCost(379, 7, currentMeleeStaminaLevel);  // Normal Axe
            SetMeleeStaminaCost(380, 4, currentMeleeStaminaLevel);  // Combat Knife
            SetMeleeStaminaCost(359, 6, currentMeleeStaminaLevel);  // Machete
            SetMeleeStaminaCost(396, 6, currentMeleeStaminaLevel);  // Taser Stick
            SetMeleeStaminaCost(474, 10, currentMeleeStaminaLevel); // Crafted Spear
            SetMeleeStaminaCost(503, 5, currentMeleeStaminaLevel);  // Torch
            SetMeleeStaminaCost(477, 8, currentMeleeStaminaLevel);  // Crafted Club
            SetMeleeStaminaCost(525, 5, currentMeleeStaminaLevel);  // Putter
            SetMeleeStaminaCost(392, 5, currentMeleeStaminaLevel);  // Stick
            SetMeleeStaminaCost(367, 8, currentMeleeStaminaLevel);  // Katana
            SetMeleeStaminaCost(663, 7, currentMeleeStaminaLevel);  // Pickaxe
            SetMeleeStaminaCost(405, 6, currentMeleeStaminaLevel);  // Bone
            SetMeleeStaminaCost(394, 12.5f, currentMeleeStaminaLevel); // Chainsaw
            SetMeleeStaminaCost(340, 8, currentMeleeStaminaLevel);  // Guitar

            // SHOVEL FIX LATER
            //ItemData shovel = ItemDatabaseManager.ItemById(485);
            //shovel.MeleeWeaponData.StaminaCost = Config.testStamina.Value;
            //float shovel_default = 0;  // NEED TO BE DONE IN ANOTHER WAY
        }

        private static void SetMeleeStaminaCost(int itemId, float defaultStamina, float currentMeleeStaminaLevel)
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            item.MeleeWeaponData.StaminaCost = defaultStamina * (float)Math.Pow(Config.MeleeStaminaPercentageReduction, currentMeleeStaminaLevel);
        }

        public static void SetTreeSwingStamina(float currentMeleeStaminaLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetTreeSwingStamina returned, player not in world"); return; }
            if (currentMeleeStaminaLevel == 0) { PlayerStatsFunctions.PostMessage("SetSetTreeSwingStamina Returned: currentMeleeStaminaLevel == 0"); return; }

            SetTreeSwingStaminaCost<ModernAxeWeaponController>(356, 7, currentMeleeStaminaLevel); // Modern Axe
            SetTreeSwingStaminaCost<FireAxeWeaponController>(431, 7, currentMeleeStaminaLevel); // Fire Axe
            SetTreeSwingStaminaCost<TacticalAxeWeaponController>(379, 7, currentMeleeStaminaLevel); // Normal Axe
            SetTreeSwingStaminaCost<ChainsawWeaponController>(394, 8, currentMeleeStaminaLevel); // Chainsaw

        }

        private static void SetTreeSwingStaminaCost<T>(int itemId, float defaultStamina, float currentMeleeStaminaLevel) where T : MonoBehaviour
        {
            ItemData item = ItemDatabaseManager.ItemById(itemId);
            T controller = item.HeldPrefab.gameObject.GetComponent<T>();

            if (controller != null)
            {
                typeof(T).GetField("_treeSwingStaminaCost").SetValue(controller, defaultStamina * (float)Math.Pow(Config.TreeSwingStaminaPercentageReduction, currentMeleeStaminaLevel));
            }
        }

        public static void SetPlayerStamina(float currentLevel = 0)
        {
            if (!LocalPlayer.IsInWorld) { PlayerStatsFunctions.PostMessage("SetPlayerStamina returned, player not in world"); return; }
            if (currentLevel == 0) { PlayerStatsFunctions.PostMessage("SetPlayerStamina Returned: currentLevel == 0"); return; }
            // Default Value == 10
            LocalPlayer.Stats._staminaRecoverRate = (float)(10 * Math.Pow(Config.StaminaBarRecoverRate, currentLevel));

            // Default Value == 1.5f
            // Reduce _runStaminaCostPerSec by X% for each level
            LocalPlayer.FpCharacter._runStaminaCostPerSec = (float)(1.5f * Math.Pow(Config.RunStaminaCostPerSec, currentLevel));

            // Default Value == 0.4f
            // Reduce timeToRecoverFromRun by X% for each level
            LocalPlayer.FpCharacter.timeToRecoverFromRun = (float)(0.4f * Math.Pow(Config.TimeToRecoverFromRun, currentLevel));
        }
    }
}
