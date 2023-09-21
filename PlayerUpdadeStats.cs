using SonsSdk;
using RedLoader;
using TheForest.Utils;
using Sons.Gui;
using UnityEngine;

namespace PlayerUpdadeStats;

public class PlayerUpdadeStats : SonsMod
{
    public PlayerUpdadeStats()
    {
        // Don't register any update callbacks here. Manually register them instead.
        // Removing this will call OnUpdate, OnFixedUpdate etc. even if you don't use them.
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
    }

    protected override void OnGameStart()
    {
        // This is called once the player spawns in the world and gains control.
        DataHandler.GetStrengthLevelVitals();
        if (doUpdateSpeeds)
        {
            if (!hasGottenOriginalValues)
            {
                hasGottenOriginalValues = true;
                originalWalkSpeed = LocalPlayer._FpCharacter_k__BackingField._walkSpeed;
                originalSprintSpeed = LocalPlayer._FpCharacter_k__BackingField._runSpeed;
                originalJumpHeight = LocalPlayer._FpCharacter_k__BackingField._jumpHeight;
                originalSwimSpeed = LocalPlayer._FpCharacter_k__BackingField._swimSpeed;
            }
            PlayerStatsFunctions.PostMessage("Updating Speeds");
            doUpdateSpeeds = false;
            // For Walk Speed
            LocalPlayer._FpCharacter_k__BackingField._walkSpeed = originalWalkSpeed * (BuyUpgrades.currentWalkSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Walk Speed = " + LocalPlayer._FpCharacter_k__BackingField._walkSpeed);
            // For Sprint Speed
            LocalPlayer._FpCharacter_k__BackingField._runSpeed = originalSprintSpeed * (BuyUpgrades.currentSprintSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Sprint Speed = " + LocalPlayer._FpCharacter_k__BackingField._runSpeed);
            // For Jump Height
            LocalPlayer._FpCharacter_k__BackingField._jumpHeight = originalJumpHeight * (BuyUpgrades.currentJumpHeightLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Jump Height = " + LocalPlayer._FpCharacter_k__BackingField._jumpHeight);
            // For Swin Speed
            LocalPlayer._FpCharacter_k__BackingField._swimSpeed = originalSwimSpeed * (BuyUpgrades.currentSwimSpeedLevel * 20 / 100 + 1);
            PlayerStatsFunctions.PostMessage("Current Swim Speed = " + LocalPlayer._FpCharacter_k__BackingField._swimSpeed);
        }
        if (!isQuitEventAdded)
        {
            PlayerStatsFunctions.PostMessage("Adding Quit Event");
            isQuitEventAdded = true;
            PauseMenu.add_OnQuitEvent((Il2CppSystem.Action)Quitting);

        }
    }

    // This is called every frame.
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayerUpdadeStatsUi.Close(); // THIS TOGGELS ALSO
            if (!LocalPlayer.IsInWorld || TheForest.Utils.LocalPlayer.IsInInventory || LocalPlayer.Inventory.Logs.HasLogs) { return; }
            if (!PauseMenu.IsActive && PauseMenu._instance.CanBeOpened())
            {
                PauseMenu._instance.Open();
            }
        }
        
    }

    internal static bool showMenu = false;
    internal static float MaxVelocity = 50;
    private bool hasGottenOriginalValues = false;
    private float originalWalkSpeed;
    private float originalSprintSpeed;
    private float originalJumpHeight;
    private float originalSwimSpeed;
    private bool isQuitEventAdded;
    internal static bool doUpdateSpeeds;


    private void Quitting()
    {
        PlayerStatsFunctions.PostMessage("Quit Button Pressed");
        DataHandler.SaveData();
        hasGottenOriginalValues = false;
        isQuitEventAdded = false;
        PlayerStatsFunctions.pointsUsed = 0;
        BuyUpgrades.currentWalkSpeedLevel = 0;
        BuyUpgrades.currentSprintSpeedLevel = 0;
        BuyUpgrades.currentJumpHeightLevel = 0;
        BuyUpgrades.currentSwimSpeedLevel = 0;
        BuyUpgrades.currentChainsawSpeedLevel = 0;
        BuyUpgrades.currentKnightVSpeedLevel = 0;
        BuyUpgrades.currentBowDamageLevel = 0;
        PlayerStatsFunctions.currentPoints = 0;
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
        PlayerUpdadeStatsUi.BowDamageBonus.Text(": +0%");
        PlayerUpdadeStatsPatches.postfixSaveID = 0;
    }

}