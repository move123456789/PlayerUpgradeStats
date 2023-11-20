using RedLoader;
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
using AssemblyCSharp;
using Sons.Gameplay.GPS;

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

        [HarmonyPatch(typeof(Vitals), "GainStrength", new Type[] { typeof(float) })]
        [HarmonyPostfix]
        public static void PostfixGainStrengthCheck(Vitals __instance)
        {
            PlayerStatsFunctions.PostMessage("Postfix PostfixGainStrengthCheck Loaded");
            DataHandler.GetStrengthLevelVitals();
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

    }
}
