using SonsSdk;
using RedLoader;
using TheForest.Utils;
using Sons.Gui;
using UnityEngine;
using SUI;
using Sons.Items.Core;

namespace PlayerUpdadeStats;

public class PlayerUpdadeStats : SonsMod
{
    public PlayerUpdadeStats()
    {
        // Don't register any update callbacks here. Manually register them instead.
        // Removing this will call OnUpdate, OnFixedUpdate etc. even if you don't use them.
        HarmonyPatchAll = true;
        OnUpdateCallback = OnUpdate;
    }

    protected override void OnInitializeMod()
    {
        // Do your early mod initialization which doesn't involve game or sdk references here
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        // Do your mod initialization which involves game or sdk references here
        // This is for stuff like UI creation, event registration etc.
        RLog.Msg("PlayerUpdadeStats Loaded");
        PlayerUpdadeStatsUi.Create();
        // Data Folder
        string dir = @"PlayerUpgradeStatsData";
        // If directory does not exist, create it
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // Adding Ingame CFG
        SettingsRegistry.CreateSettings(this, null, typeof(Config));
    }

    protected override void OnGameStart()
    {
        // This is called once the player spawns in the world and gains control.
        DataHandler.GetStrengthLevelVitals();
        if (!hasGottenOriginalValues)
        {
            hasGottenOriginalValues = true;
            originalWalkSpeed = LocalPlayer._FpCharacter_k__BackingField._walkSpeed;
            originalSprintSpeed = LocalPlayer._FpCharacter_k__BackingField._runSpeed;
            originalJumpHeight = LocalPlayer._FpCharacter_k__BackingField._jumpHeight;
            originalSwimSpeed = LocalPlayer._FpCharacter_k__BackingField._swimSpeed;
        }
        if (!isQuitEventAdded)
        {
            PlayerStatsFunctions.PostMessage("Adding Quit Event");
            isQuitEventAdded = true;
            PauseMenu.add_OnQuitEvent((Il2CppSystem.Action)Quitting);
        }
        PlayerStatsFunctions.UpdateSpeed();
    }

    // This is called every frame.
    protected void OnUpdate()
    {
        //if (Input.GetKeyDown(Config.ToggleMenuKey.Value))
        //{
            
        //}
        if (Input.GetKeyDown(KeyCode.Escape) && Config.UiTesting.Value == false)
        {
            if (PlayerUpdadeStatsUi.IsMainPanelActive) { PlayerUpdadeStatsUi.CloseMainPanel(); }
            else if (PlayerUpdadeStatsUi.IsMegaPanelActive) { PlayerUpdadeStatsUi.CloseMegaPanel(); }
        }


    }
    

    internal static bool showMenu = false;
    internal static float MaxVelocity = 50;
    private bool hasGottenOriginalValues = false;
    internal static float originalWalkSpeed;
    internal static float originalSprintSpeed;
    internal static float originalJumpHeight;
    internal static float originalSwimSpeed;
    private bool isQuitEventAdded;



    private void Quitting()
    {
        PlayerStatsFunctions.PostMessage("Quit Button Pressed");
        DataHandler.SaveData();
        hasGottenOriginalValues = false;
        isQuitEventAdded = false;
        PlayerStatsFunctions.pointsUsed = 0;
        PlayerStatsFunctions.currentPoints = 0;
        PlayerStatsFunctions.pointsUsedMega = 0;
        PlayerStatsFunctions.currentPointsMega = 0;
        BuyUpgrades.currentWalkSpeedLevel = 0;
        BuyUpgrades.currentSprintSpeedLevel = 0;
        BuyUpgrades.currentJumpHeightLevel = 0;
        BuyUpgrades.currentSwimSpeedLevel = 0;
        BuyUpgrades.currentChainsawSpeedLevel = 0;
        BuyUpgrades.currentKnightVSpeedLevel = 0;
        BuyUpgrades.currentBowDamageLevel = 0;
        MegaPoints.currentMeleeAndTreeHitStaminaLevel = 0;
        MegaPoints.currentPlayerStaminaLevel = 0;
        PlayerUpdadeStatsUi.WalkSpeedLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.WalkSpeedBonus.Text("Speed: +0%");
        PlayerUpdadeStatsUi.SprintSpeedLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.SprintSpeedBonus.Text("Speed: +0%");
        PlayerUpdadeStatsUi.SwimSpeedLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.SwimSpeedBonus.Text("Speed: +0%");
        PlayerUpdadeStatsUi.JumpHeighLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.JumpHeightBonus.Text(": +0%");
        PlayerUpdadeStatsUi.ChainSawSpeedLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.ChainSawSpeedBonus.Text("Speed: +0%");
        PlayerUpdadeStatsUi.KnightVSpeedLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.KnightVSpeedBonus.Text("Speed: +0%");
        PlayerUpdadeStatsUi.BowDamageLvl.Text("Lvl: 0/5");
        PlayerUpdadeStatsUi.BowDamageBonus.Text("+0%");
        PlayerUpdadeStatsUi.MeleeAndTreeHitStaminaLvl.Text($"Lvl: 0/{MegaPoints.MaxMegaUpgradeLevel}");
        PlayerUpdadeStatsUi.MeleeAndTreeHitStaminaBonus.Text("+0%");
        PlayerUpdadeStatsUi.PlayerStaminaLvl.Text($"Lvl: 0/{MegaPoints.MaxMegaUpgradeLevel}");
        PlayerUpdadeStatsUi.PlayerStaminaBonus.Text("+0%");
        PlayerUpdadeStatsPatches.postfixSaveID = 0;
    }

}