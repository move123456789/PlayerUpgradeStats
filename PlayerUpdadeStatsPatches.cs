﻿using RedLoader;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using System.Collections;
using System.Reflection;
using Harmony;
using HarmonyLib;
using Sons.Gameplay.GameSetup;
using Sons.Weapon;
using Sons.Gameplay;
using TheForest.Utils;

namespace PlayerUpdadeStats
{
    [HarmonyPatch]
    public class PlayerUpdadeStatsPatches
    {
        public static uint postfixSaveID;
        public static int currentStrengthLevel;


        [HarmonyPatch(typeof(GameSetupManager), "GetSelectedSaveId")]
        [HarmonyPostfix]
        public static void PostfixGetLoadedSaveID(uint __result)
        {
            PlayerStatsFunctions.PostMessage("Postfix PostfixGetLoadedSaveID Loaded");
            uint? nullable = __result;
            if (nullable.HasValue)
            {
                PlayerStatsFunctions.PostMessage("Save Id = " + __result);
                if (__result != 0)
                {
                    postfixSaveID = __result;
                    DataHandler.GetData();
                }
            }
            else { PlayerStatsFunctions.PostMessage("SaveId Posfix __result Does Not Have A Value"); }

        }

        [HarmonyPatch(typeof(GenericMeleeWeaponController), "OnEnable")]
        [HarmonyPostfix]
        public static void PostfixGenericMeleeWeaponController(ref GenericMeleeWeaponController __instance)
        {
            PlayerStatsFunctions.PostMessage("OnEnable GenericMeleeWeaponController");
            string instanceName = __instance.name;
            if (instanceName == "TacticalChainsawHeld" || instanceName == "TacticalChainsawHeld(Clone)")
            {
                PlayerStatsFunctions.PostMessage("__instance.gameObject.name = " + __instance.gameObject.name);
                PlayerStatsFunctions.PostMessage("Trying to get component ChainsawWeaponController");
                ChainsawWeaponController chainsawWeaponController = __instance.gameObject.GetComponent<ChainsawWeaponController>();
                chainsawWeaponController._treeHitFrequency = 0.25f * (1 - BuyUpgrades.currentChainsawSpeedLevel * 19 / 100);
                PlayerStatsFunctions.PostMessage("_treeHitFrequency updated, new value = " + 0.25f * (1 - BuyUpgrades.currentChainsawSpeedLevel * 19 / 100));
            }

        }
        [HarmonyPatch(typeof(PlayerKnightVAction), nameof(PlayerKnightVAction.StartRiding))]
        [HarmonyPostfix]
        public static void PostfixKnightV(ref PlayerKnightVAction __instance)
        {
            PlayerStatsFunctions.PostMessage("OnEnable PlayerKnightVAction");
            if (BuyUpgrades.currentKnightVSpeedLevel == 0) { PlayerStatsFunctions.PostMessage("currentKnightVSpeedLevel == 0, no need for updating"); return; }
            try
            {
                float calculatedMaxVelocity = defaultMaxVelocity * (BuyUpgrades.currentKnightVSpeedLevel * 20 / 100 + 1);
                PlayerStatsFunctions.PostMessage("Calculated New Current KnightV Speed = " + calculatedMaxVelocity);
                if (__instance._controlDefinition.MaxVelocity == calculatedMaxVelocity) { PlayerStatsFunctions.PostMessage("KnightVControlDefinition does not need updating"); return; }
                __instance._controlDefinition.MaxVelocity = defaultMaxVelocity * (BuyUpgrades.currentKnightVSpeedLevel * 20 / 100 + 1);
                PlayerStatsFunctions.PostMessage("After Updated Veloicy, get result = " + __instance._controlDefinition.MaxVelocity);

            }
            catch (Exception e) { PlayerStatsFunctions.PostError("Something went wrong in PostfixKnightVControlDefinition, Error: " + e); }
        }
        private static float defaultMaxVelocity = 20f;


        [HarmonyPatch(typeof(RangedWeapon), "Start")]
        [HarmonyPostfix]
        public static void PostfixBowDamage(ref RangedWeapon __instance)
        {
            PlayerStatsFunctions.PostMessage("Awake RangedWeapon");
            if (__instance.name == "CraftedBowHeld" || __instance.name == "CraftedBowHeld(Clone)")
            {
                PlayerStatsFunctions.PostMessage("Bow Now In Hand");
                if (BuyUpgrades.currentBowDamageLevel == 0) { PlayerStatsFunctions.PostMessage("No Need for Updating, currentBowDamageLevel = 0"); return; }
                var item = LocalPlayer.Inventory.RightHandItem;
                if (item != null)
                {
                    PlayerStatsFunctions.PostMessage("CraftedBowHeld - Item != null");
                    var ranged = item.ItemObject.GetComponentInChildren<RangedWeapon>();
                    ranged._simulatedBulletInfo = ranged.GetAmmo()?._properties?.ProjectileInfo;
                    PlayerStatsFunctions.PostMessage($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                    ranged._simulatedBulletInfo.muzzleDamage = defaultBowDamage * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                    PlayerStatsFunctions.PostMessage($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                }
                else { PlayerStatsFunctions.PostMessage("CraftedBowHeld - item == null"); }
            }
        }
        private static float defaultBowDamage = 20f;
        [HarmonyPatch(typeof(RangedWeapon), "CycleAmmoType")]
        [HarmonyPostfix]
        public static async void PostfixBowDamageChangeAmmo(RangedWeapon __instance)
        {
            PlayerStatsFunctions.PostMessage("Awake RangedWeapon");
            if (__instance.name == "CraftedBowHeld" || __instance.name == "CraftedBowHeld(Clone)")
            {
                PlayerStatsFunctions.PostMessage("Bow Now In Hand");
                if (BuyUpgrades.currentBowDamageLevel == 0) { PlayerStatsFunctions.PostMessage("No Need for Updating, currentBowDamageLevel = 0"); return; }
                var item = LocalPlayer.Inventory.RightHandItem;
                if (item != null)
                {
                    PlayerStatsFunctions.PostMessage("CraftedBowHeld - Item != null");
                    var ranged = item.ItemObject.GetComponentInChildren<RangedWeapon>();
                    ranged._simulatedBulletInfo = ranged.GetAmmo()?._properties?.ProjectileInfo;
                    await Task.Run(BowProjectileUpdate);
                    string curretArrow = ranged.bulletPrefab.name;
                    if (curretArrow == null) { PlayerStatsFunctions.PostMessage("Prefab Name == null"); return; }
                    PlayerStatsFunctions.PostMessage($"Bullet Prefab Name = {ranged.bulletPrefab.name}");
                    switch (curretArrow)
                    {
                        case "CraftedArrowProjectile":
                            // Crafted Arrow
                            PlayerStatsFunctions.PostMessage($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = defaultBowDamage * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PlayerStatsFunctions.PostMessage($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                        case "3dPrintedArrowProjectile":
                            // 3dPrinted Arrow
                            PlayerStatsFunctions.PostMessage($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = 30f * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PlayerStatsFunctions.PostMessage($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                        case "TacticalBowAmmoProjectile":
                            // Found Arrow
                            PlayerStatsFunctions.PostMessage($"Current MuzzleDamage = {ranged._simulatedBulletInfo.muzzleDamage}");
                            ranged._simulatedBulletInfo.muzzleDamage = 35f * (BuyUpgrades.currentBowDamageLevel * 20 / 100 + 1);
                            PlayerStatsFunctions.PostMessage($"MuzzleDamage After Update= {ranged._simulatedBulletInfo.muzzleDamage}");
                            break;
                    }
                }
                else { PlayerStatsFunctions.PostMessage("CraftedBowHeld - item == null"); }
            }
        }
        public static async Task BowProjectileUpdate()
        {
            await Task.Delay(300);
        }

    }
}
