using RedLoader;
using SonsSdk;
using SUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUpdadeStats;

using static SUI.SUI;

public class PlayerUpdadeStatsUi
{

    public const string MOD_LIST_ID = "PlayerUpgradeStats";
    public const string MegaUpgradesPanel = "MegaUpgrades";

    private static readonly Color MainBgBlack = new(0, 0, 0, 0.8f);
    private static readonly Color PanelBg = ColorFromString("#111111");
    private static readonly Color ComponentBlack = new(0, 0, 0, 0.6f);

    // Waring Or Info Messages
    internal static SUiElement<SLabelOptions> displayMessage;

    // Current Points
    internal static SUiElement<SLabelOptions> DisplayedPoints;

    // Walk Speed
    internal static SUiElement<SLabelOptions> WalkSpeedBonus;
    internal static SUiElement<SLabelOptions> WalkSpeedLvl;
    internal static SUiElement<SLabelOptions> WalkSpeedCost;

    // Sprint Speed
    internal static SUiElement<SLabelOptions> SprintSpeedBonus;
    internal static SUiElement<SLabelOptions> SprintSpeedLvl;
    internal static SUiElement<SLabelOptions> SprintSpeedCost;

    // Jump Height
    internal static SUiElement<SLabelOptions> JumpHeightBonus;
    internal static SUiElement<SLabelOptions> JumpHeighLvl;
    internal static SUiElement<SLabelOptions> JumpHeighCost;

    // Swim Speed
    internal static SUiElement<SLabelOptions> SwimSpeedBonus;
    internal static SUiElement<SLabelOptions> SwimSpeedLvl;
    internal static SUiElement<SLabelOptions> SwimSpeedCost;

    // ChainSaw Speed
    internal static SUiElement<SLabelOptions> ChainSawSpeedBonus;
    internal static SUiElement<SLabelOptions> ChainSawSpeedLvl;
    internal static SUiElement<SLabelOptions> ChainSawSpeedCost;

    // KnightV Speed
    internal static SUiElement<SLabelOptions> KnightVSpeedBonus;
    internal static SUiElement<SLabelOptions> KnightVSpeedLvl;
    internal static SUiElement<SLabelOptions> KnightVSpeedCost;

    // Bow Damage
    internal static SUiElement<SLabelOptions> BowDamageBonus;
    internal static SUiElement<SLabelOptions> BowDamageLvl;
    internal static SUiElement<SLabelOptions> BowDamageCost;

    // MEGA PANEL

    // Waring Or Info Messages
    internal static SUiElement<SLabelOptions> displayMessage_megaPanel;

    // Current Mega Points
    internal static SUiElement<SLabelOptions> DisplayedPoints_megaPanel;

    // Open Mega Points Button
    internal static SUiElement<SUI.SBgButtonOptions> OpenMegaPointsButton;

    // Melee Stamina And Tree Hit Stamina
    internal static SUiElement<SLabelOptions> MeleeAndTreeHitStaminaBonus;
    internal static SUiElement<SLabelOptions> MeleeAndTreeHitStaminaLvl;
    internal static SUiElement<SLabelOptions> MeleeAndTreeHitStaminaCost;

    // Player Stamina
    internal static SUiElement<SLabelOptions> PlayerStaminaBonus;
    internal static SUiElement<SLabelOptions> PlayerStaminaLvl;
    internal static SUiElement<SLabelOptions> PlayerStaminaCost;

    public static void Create()
    {
        var panel = RegisterNewPanel(MOD_LIST_ID)
           .Dock(EDockType.Fill).OverrideSorting(100);

        CloseMainPanel();

        var mainContainer = SContainer
            .Dock(EDockType.Fill)
            .Background(MainBgBlack).Margin(200);
        panel.Add(mainContainer);

        var title = SLabel.Text("PlayerUpgradeStats")
            .FontColor("#444").Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -95)
            .FontSpacing(10);
        title.SetParent(mainContainer);

        DisplayedPoints = SLabel.Text("Points: 0")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(40)
            .HFill().Position(null, -150)
            .FontSpacing(10);
        DisplayedPoints.SetParent(mainContainer);

        displayMessage = SLabel.Text("")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(40)
            .HFill().Position(null, -190)
            .FontSpacing(10);
        displayMessage.SetParent(mainContainer);
        displayMessage.Visible(false);

        var exitButton = SBgButton
            .Text("x").Background(GetBackgroundSprite(EBackground.Round28), Image.Type.Sliced).Color(ColorFromString("#FF234B"))
            .Pivot(1, 1).Anchor(AnchorType.TopRight).Position(-60, -60)
            .Size(60, 60).Ppu(1.7f).Notify(CloseMainPanel);
        exitButton.SetParent(mainContainer);


        var CoulumContainer = SScrollContainer
            .Dock(EDockType.Fill)
            .Vertical(10, "EX")
            .Padding(56, 56, 200, 80)
            .As<SScrollContainerOptions>();
        CoulumContainer.ContainerObject.Spacing(4);
        CoulumContainer.SetParent(mainContainer);

        // WALK SPEED

        var WalkSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        WalkSpeedRowContainer.SetParent(CoulumContainer);

        var WalkSpeedText = SLabel.Text("Walk Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        WalkSpeedText.SetParent(WalkSpeedRowContainer);

        WalkSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedLvl.SetParent(WalkSpeedRowContainer);

        WalkSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        WalkSpeedBonus.SetParent(WalkSpeedRowContainer);

        WalkSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedCost.SetParent(WalkSpeedRowContainer);

        var WalkSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        WalkSpeedBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.WalkSpeed);
        });


        WalkSpeedBtn.SetParent(WalkSpeedRowContainer);

        // Sprint SPEED
        var SprintSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        SprintSpeedRowContainer.SetParent(CoulumContainer);

        var SprintSpeedText = SLabel.Text("Sprint Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        SprintSpeedText.SetParent(SprintSpeedRowContainer);

        SprintSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedLvl.SetParent(SprintSpeedRowContainer);

        SprintSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        SprintSpeedBonus.SetParent(SprintSpeedRowContainer);

        SprintSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedCost.SetParent(SprintSpeedRowContainer);

        var SprintSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        SprintSpeedBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.SprintSpeed);
        });
        SprintSpeedBtn.SetParent(SprintSpeedRowContainer);

        // Swim SPEED

        var SwimSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        SwimSpeedRowContainer.SetParent(CoulumContainer);

        var SwimSpeedText = SLabel.Text("Swim Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        SwimSpeedText.SetParent(SwimSpeedRowContainer);

        SwimSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedLvl.SetParent(SwimSpeedRowContainer);

        SwimSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        SwimSpeedBonus.SetParent(SwimSpeedRowContainer);

        SwimSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedCost.SetParent(SwimSpeedRowContainer);

        var SwimSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        SwimSpeedBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.SwimSpeed);
        });
        SwimSpeedBtn.SetParent(SwimSpeedRowContainer);


        // ChainSaw SPEED

        var ChainSawSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        ChainSawSpeedRowContainer.SetParent(CoulumContainer);

        var ChainSawSpeedText = SLabel.Text("ChainSaw Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        ChainSawSpeedText.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedLvl.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        ChainSawSpeedBonus.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedCost.SetParent(ChainSawSpeedRowContainer);

        var ChainSawSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        ChainSawSpeedBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.ChainSawSpeed);
        });
        ChainSawSpeedBtn.SetParent(ChainSawSpeedRowContainer);


        // KnightV SPEED
        var KnightVSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        KnightVSpeedRowContainer.SetParent(CoulumContainer);

        var KnightVSpeedText = SLabel.Text("KnightV Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        KnightVSpeedText.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedLvl.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        KnightVSpeedBonus.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedCost.SetParent(KnightVSpeedRowContainer);

        var KnightVSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        KnightVSpeedBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.KnightVSpeed);
        });
        KnightVSpeedBtn.SetParent(KnightVSpeedRowContainer);


        // BowDamage Damage
        var BowDamageRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        BowDamageRowContainer.SetParent(CoulumContainer);

        var BowDamageText = SLabel.Text("Bow Damage").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        BowDamageText.SetParent(BowDamageRowContainer);

        BowDamageLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageLvl.SetParent(BowDamageRowContainer);

        BowDamageBonus = SLabel.Text("Current DMG : + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        BowDamageBonus.SetParent(BowDamageRowContainer);

        BowDamageCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageCost.SetParent(BowDamageRowContainer);

        var BowDamageBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        BowDamageBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.BowDamage);
        });
        BowDamageBtn.SetParent(BowDamageRowContainer);

        // JumpHeight Damage
        var JumpHeightRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Background(ComponentBlack).Height(50);

        JumpHeightRowContainer.SetParent(CoulumContainer);

        var JumpHeightText = SLabel.Text("Jump Height").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50);
        JumpHeightText.SetParent(JumpHeightRowContainer);

        JumpHeighLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeighLvl.SetParent(JumpHeightRowContainer);

        JumpHeightBonus = SLabel.Text("Current JumpH: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        JumpHeightBonus.SetParent(JumpHeightRowContainer);

        JumpHeighCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeighCost.SetParent(JumpHeightRowContainer);

        var JumpHeightBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        JumpHeightBtn.OnClick(() =>
        {
            BuyUpgrades.BuyUpgrade(BuyUpgrades.UpgradeType.JumpHeight);
        });
        JumpHeightBtn.SetParent(JumpHeightRowContainer);


        // Button For Opening Special Points Menu
        OpenMegaPointsButton = SBgButton
            .Text("Special Points").Background(GetBackgroundSprite(EBackground.Sons), Image.Type.Tiled).Color(ColorFromString("#FF234B"))
            .Pivot(1, 1).Anchor(AnchorType.BottomCenter).Width(250)
            .Size(100, 60).Ppu(1.7f).Notify(() =>
            {
                CloseMainPanel();
                OpenMegaPanel();
            });
        OpenMegaPointsButton.SetParent(CoulumContainer);
        OpenMegaPointsButton.Visible(false);


        // SG BUTTON, I USE LABEL BUTTON CURRENTLY
        //var WalkSpeedBtn = SBgButton.Background(EBackground.None).Text("Upgrade").FontColor(Color.white).FontSize(32).PHeight(50)
        //    .Notify(Test);
        //WalkSpeedBtn.SetParent(WalkSpeedRowContainer);

        //if (PlayerStatsFunctions.currentPoints == 0)
        //{
        //    WalkSpeedBtn.FontColor("#f74639");
        //    SwimSpeedBtn.FontColor("#f74639");
        //    SprintSpeedBtn.FontColor("#f74639");
        //    JumpHeightBtn.FontColor("#f74639");
        //    KnightVSpeedBtn.FontColor("#f74639");
        //    ChainSawSpeedBtn.FontColor("#f74639");
        //    BowDamageBtn.FontColor("#f74639");
        //}

        // MEGA PANEL
        var mega_panel = RegisterNewPanel(MegaUpgradesPanel)
           .Dock(EDockType.Fill).OverrideSorting(100);

        CloseMegaPanel();

        var mainContainer_megaPanel = SContainer
            .Dock(EDockType.Fill)
            .Background(MainBgBlack).Margin(300, 250);
        mega_panel.Add(mainContainer_megaPanel);

        var title_megaPanel = SLabel.Text("PlayerUpgradeStats")
            .FontColor("#444").Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(32)
            .HFill().Position(null, -95)
            .FontSpacing(10);
        title_megaPanel.SetParent(mainContainer_megaPanel);

        DisplayedPoints_megaPanel = SLabel.Text("Special Points: 0")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(40)
            .HFill().Position(null, -150)
            .FontSpacing(10);
        DisplayedPoints_megaPanel.SetParent(mainContainer_megaPanel);

        displayMessage_megaPanel = SLabel.Text("")
            .FontColor(Color.white).Font(EFont.RobotoRegular)
            .PHeight(100).FontSize(40)
            .HFill().Position(null, -190)
            .FontSpacing(10);
        displayMessage_megaPanel.SetParent(mainContainer_megaPanel);
        displayMessage_megaPanel.Visible(false);

        var exitButton_megaPanel = SBgButton
            .Text("x").Background(GetBackgroundSprite(EBackground.Round28), Image.Type.Sliced).Color(ColorFromString("#FF234B"))
            .Pivot(1, 1).Anchor(AnchorType.TopRight).Position(-60, -60)
            .Size(60, 60).Ppu(1.7f).Notify(CloseMegaPanel);
        exitButton_megaPanel.SetParent(mainContainer_megaPanel);

        var backButton_megaPanel = SBgButton
            .Text("BACK").Background(GetBackgroundSprite(EBackground.Sons), Image.Type.Tiled).Color(ColorFromString("#FF234B"))
            .Pivot(1, 1).Anchor(AnchorType.TopLeft).Position(160, -60)
            .Size(100, 60).Ppu(1.7f).Notify(() =>
            {
                CloseMegaPanel();
                OpenMainPanel();
            });
        backButton_megaPanel.SetParent(mainContainer_megaPanel);


        var CoulumContainer_megaPanel = SScrollContainer
            .Dock(EDockType.Fill)
            .Margin(56, 56, 200, 80)
            //.Background(Color.green)
            .ChildExpand(true, true)
            .ChildControl(true, true)
            .As<SScrollContainerOptions>();
        //CoulumContainer_megaPanel.ContainerObject.Spacing(4);
        mainContainer_megaPanel.Add(CoulumContainer_megaPanel);

        // Melee Stamina And Tree Hit Stamina
        var weapon_stamina_Container = SContainer
            .Dock(EDockType.Fill)
            .Horizontal(0, "EE")
            .Background(ComponentBlack)
            //.Background(Color.blue)
            .PHeight(50)
            ;
        CoulumContainer_megaPanel.Add(weapon_stamina_Container);

        var weapon_stamina_Text = SLabel.Text("ToolStamina").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50).AutoSizeContainer(true);
        weapon_stamina_Container.Add(weapon_stamina_Text);

        MeleeAndTreeHitStaminaLvl = SLabel.Text("Lvl: 0/1").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        weapon_stamina_Container.Add(MeleeAndTreeHitStaminaLvl);

        MeleeAndTreeHitStaminaBonus = SLabel.Text("Current TEST: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        weapon_stamina_Container.Add(MeleeAndTreeHitStaminaBonus);

        MeleeAndTreeHitStaminaCost = SLabel.Text("Cost: 1").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        weapon_stamina_Container.Add(MeleeAndTreeHitStaminaCost);

        var weapon_stamina_Btn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        weapon_stamina_Btn.OnClick(() =>
        {
            MegaPoints.BuyMegaUpgrade(MegaPoints.MegaUpgradeType.ToolStamina);
        });
        weapon_stamina_Container.Add(weapon_stamina_Btn);



        // Player Stamina
        var player_stamina_Container = SContainer
            .Dock(EDockType.Fill)
            .Horizontal(0, "EE")
            .Background(ComponentBlack)
            //.Background(Color.red)
            .PHeight(50)
            ;
        CoulumContainer_megaPanel.Add(player_stamina_Container);

        var player_stamina_Text = SLabel.Text("PlayerStamina").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineLeft).Margin(50).AutoSizeContainer(true);
        player_stamina_Container.Add(player_stamina_Text);

        MeleeAndTreeHitStaminaLvl = SLabel.Text("Lvl: 0/1").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        player_stamina_Container.Add(MeleeAndTreeHitStaminaLvl);

        MeleeAndTreeHitStaminaBonus = SLabel.Text("Current Bonus: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.Midline);
        player_stamina_Container.Add(MeleeAndTreeHitStaminaBonus);

        MeleeAndTreeHitStaminaCost = SLabel.Text("Cost: 1").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        player_stamina_Container.Add(MeleeAndTreeHitStaminaCost);

        var player_stamina_Btn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).Alignment(TextAlignmentOptions.MidlineRight).Margin(50);
        player_stamina_Btn.OnClick(() =>
        {
            MegaPoints.BuyMegaUpgrade(MegaPoints.MegaUpgradeType.PlayerStamina);
        });
        player_stamina_Container.Add(player_stamina_Btn);
    }

    internal static void CloseMainPanel()
    {
        TogglePanel(MOD_LIST_ID, false);
    }

    internal static void ToggleMainPanel()
    {
        TogglePanel(MOD_LIST_ID);
    }

    internal static void OpenMainPanel()
    {
        TogglePanel(MOD_LIST_ID, true);
    }

    internal static void CloseMegaPanel()
    {
        TogglePanel(MegaUpgradesPanel, false);
    }

    internal static void ToggleMegaPanel()
    {
        TogglePanel(MegaUpgradesPanel);
    }

    internal static void OpenMegaPanel()
    {
        TogglePanel(MegaUpgradesPanel, true);
    }

    internal static void CustomPanels(string panelName, bool show)
    {
        TogglePanel(panelName, show);
    }

    internal static bool IsMainPanelActive
    {
        get
        {
            return GetPanel(MOD_LIST_ID).Root.activeSelf;
        }
    }

    internal static bool IsMegaPanelActive
    {
        get
        {
            return GetPanel(MegaUpgradesPanel).Root.activeSelf;
        }
    }

}