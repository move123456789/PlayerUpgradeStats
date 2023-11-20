using SonsSdk;
using RedLoader;
using TheForest.Utils;
using Sons.Gui;
using UnityEngine;
using SUI;
using Sons.Items.Core;

namespace PlayerUpdadeStats;

public class PlayerUpgradeStats : SonsMod
{
    public PlayerUpgradeStats()
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
        PlayerStatsFunctions.PostMessage("PlayerUpdadeStats Loaded");
        PlayerUpgradeStatsUi.Create();
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
            if (PlayerUpgradeStatsUi.IsMainPanelActive) { PlayerUpgradeStatsUi.CloseMainPanel(); }
            else if (PlayerUpgradeStatsUi.IsMegaPanelActive) { PlayerUpgradeStatsUi.CloseMegaPanel(); }
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
        PlayerUpgradeStatsUi.WalkSpeedLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.WalkSpeedBonus.Text("Speed: +0%");
        PlayerUpgradeStatsUi.SprintSpeedLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.SprintSpeedBonus.Text("Speed: +0%");
        PlayerUpgradeStatsUi.SwimSpeedLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.SwimSpeedBonus.Text("Speed: +0%");
        PlayerUpgradeStatsUi.JumpHeighLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.JumpHeightBonus.Text(": +0%");
        PlayerUpgradeStatsUi.ChainSawSpeedLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.ChainSawSpeedBonus.Text("Speed: +0%");
        PlayerUpgradeStatsUi.KnightVSpeedLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.KnightVSpeedBonus.Text("Speed: +0%");
        PlayerUpgradeStatsUi.BowDamageLvl.Text("Lvl: 0/5");
        PlayerUpgradeStatsUi.BowDamageBonus.Text("+0%");
        PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaLvl.Text($"Lvl: 0/{MegaPoints.MaxMegaUpgradeLevel}");
        PlayerUpgradeStatsUi.MeleeAndTreeHitStaminaBonus.Text("+0%");
        PlayerUpgradeStatsUi.PlayerStaminaLvl.Text($"Lvl: 0/{MegaPoints.MaxMegaUpgradeLevel}");
        PlayerUpgradeStatsUi.PlayerStaminaBonus.Text("+0%");
        PlayerUpgradeStatsPatches.postfixSaveID = 0;
    }

}