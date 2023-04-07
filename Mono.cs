using Sons.Gameplay.GameSetup;
using Sons.Gui;
using Sons.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Tools;
using TheForest.Utils;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using static PlayerUpgradeStats.Plugin;

namespace PlayerUpgradeStats
{
    public partial class Plugin
    {
        
        public class PlayerStatsMono : MonoBehaviour
        {
            internal bool showMenu = false;
            private bool hasGottenOriginalValues = false;
            private float originalWalkSpeed;
            private float originalSprintSpeed;
            private float originalJumpHeight;
            private float originalSwimSpeed;
            private bool isQuitEventAdded;
            private void Update()
            {
                if (smokyaceDeactivate.Value || !LocalPlayer.IsInWorld || TheForest.Utils.LocalPlayer.IsInInventory || LocalPlayer.Inventory.Logs.HasLogs) { return; }
                if (Input.GetKeyDown(smokyaceMenurKey.Value))
                {
                    if (PlayerStatsPatcher.myUIBase == null) { return; }
                    if (!showMenu)
                    {
                        showMenu = true;
                        PlayerStatsPatcher.myPanel.SetActive(true);
                        PostLogsToConsole("myPanel == Active");
                    } else if (showMenu)
                    {
                        showMenu = false;
                        PlayerStatsPatcher.myPanel.SetActive(false);
                        PostLogsToConsole("myPanel == False");
                    }
                    
                }
                if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    PostLogsToConsole("_defaultGameNameText = " + SavedSlot._defaultGameNameText);
                    PostLogsToConsole("SaveGameManager.SaveFileName = " + SaveGameManager.SaveFileName);

                }
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
                    PostLogsToConsole("Updating Speeds");
                    doUpdateSpeeds = false;
                    // For Walk Speed
                    LocalPlayer._FpCharacter_k__BackingField._walkSpeed = originalWalkSpeed * (BuyUpgrades.currentWalkSpeedLevel*20/100 + 1);
                    PostLogsToConsole("Current Walk Speed = " + LocalPlayer._FpCharacter_k__BackingField._walkSpeed);
                    // For Sprint Speed
                    LocalPlayer._FpCharacter_k__BackingField._runSpeed = originalSprintSpeed * (BuyUpgrades.currentSprintSpeedLevel * 20 / 100 + 1);
                    PostLogsToConsole("Current Sprint Speed = " + LocalPlayer._FpCharacter_k__BackingField._runSpeed);
                    // For Jump Height
                    LocalPlayer._FpCharacter_k__BackingField._jumpHeight = originalJumpHeight * (BuyUpgrades.currentJumpHeightLevel * 20 / 100 + 1);
                    PostLogsToConsole("Current Jump Height = " + LocalPlayer._FpCharacter_k__BackingField._jumpHeight);
                    // For Swin Speed
                    LocalPlayer._FpCharacter_k__BackingField._swimSpeed = originalSwimSpeed * (BuyUpgrades.currentSwimSpeedLevel * 20 / 100 + 1);
                    PostLogsToConsole("Current Swim Speed = " + LocalPlayer._FpCharacter_k__BackingField._swimSpeed);

                }
                if (!isQuitEventAdded)
                {
                    PostLogsToConsole("Adding Quit Event");
                    isQuitEventAdded = true;
                    PauseMenu.add_OnQuitEvent((Il2CppSystem.Action)Quitting);
                }
            }
            private void Quitting()
            {
                PostLogsToConsole("Quit Button Pressed");
                hasGottenOriginalValues = false;
                isQuitEventAdded = false;
                pointsUsed = 0;
                BuyUpgrades.currentWalkSpeedLevel = 0;
                BuyUpgrades.currentSprintSpeedLevel = 0;
                BuyUpgrades.currentJumpHeightLevel = 0;
                BuyUpgrades.currentSwimSpeedLevel = 0;
                currentPoints = 0;
                MyPanel.swimSpeedIncrease.text = $"Speed: +0%" + $"  Level 0/5";
                MyPanel.walkSpeedIncrease.text = $"Speed: +0%" + $"  Level 0/5";
                MyPanel.sprintSpeedIncrease.text = $"Speed: +0%" + $"  Level 0/5";
                MyPanel.jumpHeightIncrease.text = $"Height: +0%" + $"  Level 0/5";

            }
        }
    }
}
