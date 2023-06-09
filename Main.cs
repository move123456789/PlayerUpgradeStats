﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverseLib.UI;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using TheForest;
using Sons.Gui;
using Bolt;
using TheForest.Utils;
using Sons.Weapon;
using System.Xml.Linq;
using Sons.Gameplay;
using static PlayerUpgradeStats.BuyUpgrades;

namespace PlayerUpgradeStats
{
    public partial class Plugin
    {
        internal static void PostLogsToConsole(string message)
        {
            if (!smokyaceLogToConsole.Value) { return; }
            DLog.LogInfo(message);
        }
        internal static void PostErrorToConsole(string message)
        {
            DLog.LogError(message);
        }

        public static void LoadStats()
        {
            MyPanel.curPoints.text = $"Upgrade Points Left: {currentPoints}";
            GetCurrentPoints(PlayerStatsPatcher.currentStrengthLevel, pointsUsed);
        }

        public static int GetCurrentPoints(int currentStrengthLevel, int pointsUsed)
        {
            if (currentStrengthLevel == 0)
            {
                return currentPoints = 0;
            }
            currentPoints = currentStrengthLevel - pointsUsed;
            return currentPoints;
        }

        public static int GetPointsUsed()
        {
            return pointsUsed;
        }

        public class MyPanel : UniverseLib.UI.Panels.PanelBase
        {
            public MyPanel(UIBase owner) : base(owner) { }

            public override string Name => "Upgrade Player Stats";
            public override int MinWidth => 100;
            public override int MinHeight => 200;
            public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
            public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
            public override bool CanDragAndResize => true;
            private Color btnColor = new Color(0.5f, 0.5f, 0.5f, 1);
            private Color Color2 = new Color(0.1f, 0.1f, 0.1f);
            protected override void OnClosePanelClicked()
            {
                this.SetActive(false);
                PostLogsToConsole("Close Button Clicked");
                PlayerStatsMono.showMenu = false;
            }

            protected override void ConstructPanelContent()
            {
                // Top Bar
                GameObject topBarGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "TopBar", false, false, true, true, 4, new Vector4(3, 3, 3, 3),
                Color2);
                curStrengthLvl = UIFactory.CreateLabel(topBarGroup, "curStrengthLvl", "Your Strength Level: " + PlayerStatsPatcher.currentStrengthLevel);
                curPoints = UIFactory.CreateLabel(topBarGroup, "curPoints", "Upgrade Points Left: " + currentPoints);
                UIFactory.SetLayoutElement(curStrengthLvl.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(curPoints.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(topBarGroup, spacing: 3);

                // Walk Speed
                Text walkSpeedHeader = UIFactory.CreateLabel(ContentRoot, "walkSpeedHeader", "Walk Speed");
                walkNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "walkNotEnoughPoints", "You don't have Enough points!");
                walkNotEnoughPoints.enabled = false;
                walkMaxLevel = UIFactory.CreateLabel(ContentRoot, "walkMaxLevel", "You have reached the max level!");
                walkMaxLevel.enabled = false;
                GameObject walkSpeedGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "WalkSpeedHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeWalkSpeed = UIFactory.CreateButton(walkSpeedGroup, "upgradeWalkSpeed", "+20% Walk Speed", btnColor);
                upgradeWalkSpeed.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.WalkSpeed);
                };
                walkSpeedIncrease = UIFactory.CreateLabel(walkSpeedGroup, "walkSpeedIncrease", "Speed: +0%" + $"  Level {BuyUpgrades.currentWalkSpeedLevel}/5");
                walkSpeedCost = UIFactory.CreateLabel(walkSpeedGroup, "walkSpeedCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(walkSpeedGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(walkSpeedHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(walkSpeedGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(walkSpeedIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(walkSpeedCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeWalkSpeed.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(walkNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Sprint Speed
                Text sprintSpeedHeader = UIFactory.CreateLabel(ContentRoot, "sprintSpeedHeader", "Sprint Speed");
                sprintNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "sprintNotEnoughPoints", "You don't have Enough points!");
                sprintNotEnoughPoints.enabled = false;
                sprintMaxLevel = UIFactory.CreateLabel(ContentRoot, "sprintMaxLevel", "You have reached the max level!");
                sprintMaxLevel.enabled = false;
                GameObject sprintSpeedGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "sprintSpeedHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeSprintSpeed = UIFactory.CreateButton(sprintSpeedGroup, "upgradeSprintSpeed", "+20% Sprint Speed", btnColor);
                upgradeSprintSpeed.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.SprintSpeed);
                };
                sprintSpeedIncrease = UIFactory.CreateLabel(sprintSpeedGroup, "sprintSpeedIncrease", "Speed: +0%" + $"  Level {BuyUpgrades.currentSprintSpeedLevel}/5");
                sprintSpeedCost = UIFactory.CreateLabel(sprintSpeedGroup, "sprintSpeedCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(sprintSpeedGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(sprintSpeedHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(sprintSpeedGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(sprintSpeedIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(sprintSpeedCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeSprintSpeed.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(sprintNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Jump Height
                Text jumpHeightHeader = UIFactory.CreateLabel(ContentRoot, "jumpHeightHeader", "Jump Height");
                jumpNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "jumpNotEnoughPoints", "You don't have Enough points!");
                jumpNotEnoughPoints.enabled = false;
                jumpMaxLevel = UIFactory.CreateLabel(ContentRoot, "jumpMaxLevel", "You have reached the max level!");
                jumpMaxLevel.enabled = false;
                GameObject jumpHeightGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "jumpHeightHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeJumpHeight = UIFactory.CreateButton(jumpHeightGroup, "upgradeJumpHeight", "+20% Jump Height", btnColor);
                upgradeJumpHeight.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.JumpHeight);
                };
                jumpHeightIncrease = UIFactory.CreateLabel(jumpHeightGroup, "jumpHeightIncrease", "Height: +0%" + $"  Level {BuyUpgrades.currentJumpHeightLevel}/5");
                jumpHeightCost = UIFactory.CreateLabel(jumpHeightGroup, "jumpHeightCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(jumpHeightGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(jumpHeightHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(jumpHeightGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(jumpHeightIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(jumpHeightCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeJumpHeight.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(jumpNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Swim Speed
                Text swimSpeedHeader = UIFactory.CreateLabel(ContentRoot, "swimSpeedHeader", "Swim Speed");
                swimNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "swimNotEnoughPoints", "You don't have Enough points!");
                swimNotEnoughPoints.enabled = false;
                swimMaxLevel = UIFactory.CreateLabel(ContentRoot, "swimMaxLevel", "You have reached the max level!");
                swimMaxLevel.enabled = false;
                GameObject swimSpeedGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "swimSpeedHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeSwimSpeed = UIFactory.CreateButton(swimSpeedGroup, "upgradeSwimSpeed", "+20% Swim Speed", btnColor);
                upgradeSwimSpeed.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.SwimSpeed);
                };
                swimSpeedIncrease = UIFactory.CreateLabel(swimSpeedGroup, "swimSpeedIncrease", "Speed: +0%" + $"  Level {BuyUpgrades.currentSwimSpeedLevel}/5");
                swimSpeedCost = UIFactory.CreateLabel(swimSpeedGroup, "swimSpeedCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(swimSpeedGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(swimSpeedHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(swimSpeedGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(swimSpeedIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(swimSpeedCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeSwimSpeed.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(swimNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Chainsaw Speed
                Text chainSawSpeedHeader = UIFactory.CreateLabel(ContentRoot, "chanSawSpeedHeader", "Chainsaw Speed");
                chainSawNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "chainSawNotEnoughPoints", "You don't have Enough points!");
                chainSawNotEnoughPoints.enabled = false;
                chainSawMaxLevel = UIFactory.CreateLabel(ContentRoot, "chainSawMaxLevel", "You have reached the max level!");
                chainSawMaxLevel.enabled = false;
                GameObject chainSawSpeedGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "chainSawSpeedHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeChainsawSpeed = UIFactory.CreateButton(chainSawSpeedGroup, "upgradeChainsawSpeed", "+20% Chainsaw Speed", btnColor);
                upgradeChainsawSpeed.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.ChainSawSpeed);
                };
                chainSawSpeedIncrease = UIFactory.CreateLabel(chainSawSpeedGroup, "chainSawSpeedIncrease", "Speed: +0%" + $"  Level {BuyUpgrades.currentChainsawSpeedLevel}/5");
                chainSawSpeedCost = UIFactory.CreateLabel(chainSawSpeedGroup, "chainSawSpeedCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(chainSawSpeedGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(chainSawSpeedHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(chainSawSpeedGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(chainSawSpeedIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(chainSawSpeedCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeChainsawSpeed.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(chainSawNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // KnightV Speed
                Text knightVSpeedHeader = UIFactory.CreateLabel(ContentRoot, "knightVSpeedHeader", "KnightV Speed");
                knightVNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "knightVNotEnoughPoints", "You don't have Enough points!");
                knightVNotEnoughPoints.enabled = false;
                knightVMaxLevel = UIFactory.CreateLabel(ContentRoot, "knightVMaxLevel", "You have reached the max level!");
                knightVMaxLevel.enabled = false;
                GameObject knightVSpeedGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "knightVSpeedHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeKnightVSpeed = UIFactory.CreateButton(knightVSpeedGroup, "upgradeKnightVSpeed", "+20% KnightV Speed", btnColor);
                upgradeKnightVSpeed.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.KnightVSpeed);
                };
                knightVSpeedIncrease = UIFactory.CreateLabel(knightVSpeedGroup, "knightVSpeedIncrease", "Speed: +0%" + $"  Level {BuyUpgrades.currentKnightVSpeedLevel}/5");
                knightVSpeedCost = UIFactory.CreateLabel(knightVSpeedGroup, "knightVSpeedCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(knightVSpeedGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(knightVSpeedHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(knightVSpeedGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(knightVSpeedIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(knightVSpeedCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeKnightVSpeed.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(knightVNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Bow Damage
                Text bowDamageHeader = UIFactory.CreateLabel(ContentRoot, "bowDamageHeader", "Crafted Bow Damage");
                bowNotEnoughPoints = UIFactory.CreateLabel(ContentRoot, "bowNotEnoughPoints", "You don't have Enough points!");
                bowNotEnoughPoints.enabled = false;
                bowMaxLevel = UIFactory.CreateLabel(ContentRoot, "bowMaxLevel", "You have reached the max level!");
                bowMaxLevel.enabled = false;
                GameObject bowDamageGroup = UIFactory.CreateHorizontalGroup(ContentRoot, "bowDamageHor", false, false, true, true, 4, new Vector4(3, 5, 3, 3));
                ButtonRef upgradeBowDamage = UIFactory.CreateButton(bowDamageGroup, "upgradeBowDamage", "+20% Bow Damage", btnColor);
                upgradeBowDamage.OnClick += () =>
                {
                    BuyUpgrade(UpgradeType.BowDamage);
                };
                bowDamageIncrease = UIFactory.CreateLabel(bowDamageGroup, "bowDamageIncrease", "Damage: +0%" + $"  Level {BuyUpgrades.currentBowDamageLevel}/5");
                bowDamageCost = UIFactory.CreateLabel(bowDamageGroup, "bowDamageCost", "Cost: 2");
                UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(bowDamageGroup, padLeft: 2, padBottom: 0, padTop: 0, spacing: 5);
                UIFactory.SetLayoutElement(bowDamageHeader.gameObject, minWidth: 50, minHeight: 25);
                UIFactory.SetLayoutElement(bowDamageGroup.gameObject, minWidth: 200, minHeight: 25);
                UIFactory.SetLayoutElement(bowDamageIncrease.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(bowDamageCost.gameObject, flexibleWidth: 50, minHeight: 30, flexibleHeight: 0);
                UIFactory.SetLayoutElement(upgradeBowDamage.Component.gameObject, minHeight: 30, flexibleHeight: 0, minWidth: 250, preferredWidth: 250);
                UIFactory.SetLayoutElement(bowNotEnoughPoints.gameObject, flexibleWidth: 20, minHeight: 0, flexibleHeight: 0, minWidth: 20);

                // Ui Loaded
                isUiLoaded = true;
        }
            // For Walk Speed
            public static Text walkNotEnoughPoints;
            public static Text walkMaxLevel;
            public static Text walkSpeedIncrease;
            public static Text walkSpeedCost;

            // For Sprint Speed
            public static Text sprintNotEnoughPoints;
            public static Text sprintMaxLevel;
            public static Text sprintSpeedCost;
            public static Text sprintSpeedIncrease;

            // For Jump Height
            public static Text jumpNotEnoughPoints;
            public static Text jumpMaxLevel;
            public static Text jumpHeightCost;
            public static Text jumpHeightIncrease;

            // For Swin Speed
            public static Text swimNotEnoughPoints;
            public static Text swimMaxLevel;
            public static Text swimSpeedCost;
            public static Text swimSpeedIncrease;

            // For Chainsaw Speed
            public static Text chainSawNotEnoughPoints;
            public static Text chainSawMaxLevel;
            public static Text chainSawSpeedCost;
            public static Text chainSawSpeedIncrease;

            // For KnightV Speed
            public static Text knightVNotEnoughPoints;
            public static Text knightVMaxLevel;
            public static Text knightVSpeedCost;
            public static Text knightVSpeedIncrease;

            // For Bow Damage
            public static Text bowNotEnoughPoints;
            public static Text bowMaxLevel;
            public static Text bowDamageCost;
            public static Text bowDamageIncrease;

            // Publics
            public static Text curPoints;
            public static Text curStrengthLvl;



        }
        

        public static int currentPoints;
        public static int pointsUsed;
        public static bool doUpdateSpeeds;
        public static bool isUiLoaded;




        internal class ChainSawModifications
        {
            private static float defaultChainsawHitFrequency = 0.25f;

            internal static ChainsawWeaponController GetChainSawComponent()
            {
                try
                {
                    if (!LocalPlayer.IsInWorld) { PostLogsToConsole("GetChainSawComponent, Player Not In World"); return null; }
                    if (LocalPlayer.Inventory.RightHandItem == null || LocalPlayer.Inventory.RightHandItem.ItemObject == null || LocalPlayer.Inventory.RightHandItem.ItemObject.name != "TacticalChainsawHeld") { PostLogsToConsole("Chainsaw name != TacticalChainsawHeld, so chainsaw is not in hand from GetChainSawComponent"); return null; }
                    GameObject chainsawObject = GameObject.Find("TacticalChainsawHeld");
                    if (chainsawObject == null) { PostErrorToConsole("In GetChainSawComponent, Could not find 'TacticalChainsawHeld' object"); return null; }
                    ChainsawWeaponController ComponentChainsawWeaponController = chainsawObject.GetComponent<ChainsawWeaponController>();
                    if (ComponentChainsawWeaponController == null) { PostErrorToConsole("In GetChainSawComponent, Found Object == null"); return null; } else { PostErrorToConsole("Found ComponentChainsawWeaponController"); }
                    return ComponentChainsawWeaponController;
                }
                catch (Exception e) { PostErrorToConsole("Could not run GetChainSawComponent Error: " + e); return null; }
            }

            internal static float ChainSawHitFrequency
            {
                get
                {
                    if (GetChainSawComponent() == null) { PostLogsToConsole("In Get ChainSawHitFrequency, GetChainSawComponent == null"); return ChainSawHitFrequency = 0; }
                    return GetChainSawComponent()._treeHitFrequency;
                }
                set
                {
                    if (GetChainSawComponent() == null) { PostLogsToConsole("In Set ChainSawHitFrequency, GetChainSawComponent == null"); return; }
                    try
                    {
                        PostLogsToConsole("Setting _treeHitFrequency = value");
                        GetChainSawComponent()._treeHitFrequency = value;
                    }
                    catch (Exception e) { PostErrorToConsole("Could not set _treeHitFrequency Error: " + e); }
                }
            }

            internal static void UpgradeChainsawHitFrequency(float currentChainsawSpeedLevel)
            {
                PostLogsToConsole("In UpgradeChainsawHitFrequency");
                try
                {
                    if (currentChainsawSpeedLevel == 0) { PostLogsToConsole("No Need To UpgradeChainsawHitFrequency, currentChainsawSpeedLevel == 0"); return; }
                    if (!LocalPlayer.IsInWorld) { PostLogsToConsole("GetChainSawComponent, Player Not In World"); return; }
                    if (LocalPlayer.Inventory.RightHandItem != null && LocalPlayer.Inventory.RightHandItem.ItemObject != null && LocalPlayer.Inventory.RightHandItem.ItemObject.name != "TacticalChainsawHeld") { PostLogsToConsole("Chainsaw name != TacticalChainsawHeld, so chainsaw is not in hand, from UpgradeChainsawHitFrequency"); return; }
                    ChainSawHitFrequency = defaultChainsawHitFrequency * (1 - currentChainsawSpeedLevel * 19 / 100);
                }
                catch (Exception e) { PostErrorToConsole("Something went wrong in UpgradeChainsawHitFrequency, Error: " + e); }
                PostLogsToConsole("Current Chainsaw Speed = " + ChainSawHitFrequency);
            }
        }
    }
}
