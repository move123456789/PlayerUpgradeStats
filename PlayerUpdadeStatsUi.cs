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



    internal static void Test()
    {
        PlayerStatsFunctions.PostMessage("CLICK");
        WalkSpeedBonus.Text("+ 50%");
    }


    public static void Create()
    {
        var panel = RegisterNewPanel(MOD_LIST_ID)
           .Dock(EDockType.Fill).OverrideSorting(100);

        Close();

        var mainContainer = SContainer
            .Dock(EDockType.Fill)
            .Background(MainBgBlack).Margin(200);
        panel.Add(mainContainer);

        // TESTING SCROLL TO MAIN CONTAINER
        //var mainContainer = SScrollContainer
        //    .Dock(EDockType.Fill)
        //    .Background(MainBgBlack).Margin(200)
        //    .As<SScrollContainerOptions>();
        //mainContainer.ContainerObject.Spacing(4);
        //panel.Add(mainContainer);


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
            .Size(60, 60).Ppu(1.7f).Notify(Close);
        exitButton.SetParent(mainContainer);

        // Colum Container
        //var CoulumContainer = SContainer
        //    .Dock(EDockType.Fill)
        //                   .Vertical(10, "EX").Padding(56, 56, 200, 80);
        //CoulumContainer.SetParent(mainContainer);

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
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        WalkSpeedRowContainer.SetParent(CoulumContainer);

        var WalkSpeedText = SLabel.Text("Walk Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedText.SetParent(WalkSpeedRowContainer);

        WalkSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedLvl.SetParent(WalkSpeedRowContainer);

        WalkSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedBonus.SetParent(WalkSpeedRowContainer);

        WalkSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        WalkSpeedCost.SetParent(WalkSpeedRowContainer);

        var WalkSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        WalkSpeedBtn.SetParent(WalkSpeedRowContainer);

        // Sprint SPEED
        var SprintSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        SprintSpeedRowContainer.SetParent(CoulumContainer);

        var SprintSpeedText = SLabel.Text("Sprint Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedText.SetParent(SprintSpeedRowContainer);

        SprintSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedLvl.SetParent(SprintSpeedRowContainer);

        SprintSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedBonus.SetParent(SprintSpeedRowContainer);

        SprintSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SprintSpeedCost.SetParent(SprintSpeedRowContainer);

        var SprintSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        SprintSpeedBtn.SetParent(SprintSpeedRowContainer);

        // Swim SPEED

        var SwimSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        SwimSpeedRowContainer.SetParent(CoulumContainer);

        var SwimSpeedText = SLabel.Text("Swim Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedText.SetParent(SwimSpeedRowContainer);

        SwimSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedLvl.SetParent(SwimSpeedRowContainer);

        SwimSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedBonus.SetParent(SwimSpeedRowContainer);

        SwimSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        SwimSpeedCost.SetParent(SwimSpeedRowContainer);

        var SwimSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        SwimSpeedBtn.SetParent(SwimSpeedRowContainer);


        // ChainSaw SPEED

        var ChainSawSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        ChainSawSpeedRowContainer.SetParent(CoulumContainer);

        var ChainSawSpeedText = SLabel.Text("ChainSaw Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedText.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedLvl.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedBonus.SetParent(ChainSawSpeedRowContainer);

        ChainSawSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        ChainSawSpeedCost.SetParent(ChainSawSpeedRowContainer);

        var ChainSawSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        ChainSawSpeedBtn.SetParent(ChainSawSpeedRowContainer);


        // KnightV SPEED
        var KnightVSpeedRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        KnightVSpeedRowContainer.SetParent(CoulumContainer);

        var KnightVSpeedText = SLabel.Text("KnightV Speed").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedText.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedLvl.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedBonus = SLabel.Text("Current Speed: + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedBonus.SetParent(KnightVSpeedRowContainer);

        KnightVSpeedCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        KnightVSpeedCost.SetParent(KnightVSpeedRowContainer);

        var KnightVSpeedBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        KnightVSpeedBtn.SetParent(KnightVSpeedRowContainer);


        // BowDamage Damage
        var BowDamageRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        BowDamageRowContainer.SetParent(CoulumContainer);

        var BowDamageText = SLabel.Text("Bow Damage").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageText.SetParent(BowDamageRowContainer);

        BowDamageLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageLvl.SetParent(BowDamageRowContainer);

        BowDamageBonus = SLabel.Text("Current : + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageBonus.SetParent(BowDamageRowContainer);

        BowDamageCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        BowDamageCost.SetParent(BowDamageRowContainer);

        var BowDamageBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        BowDamageBtn.SetParent(BowDamageRowContainer);

        // JumpHeight Damage
        var JumpHeightRowContainer = SContainer
            .Dock(EDockType.Fill)
                           .Horizontal(0, "EE").Padding(0, 0, 50, 200).Background(ComponentBlack).Height(70);

        JumpHeightRowContainer.SetParent(CoulumContainer);

        var JumpHeightText = SLabel.Text("Jump Height").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeightText.SetParent(JumpHeightRowContainer);

        JumpHeighLvl = SLabel.Text("Lvl: 0/5").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeighLvl.SetParent(JumpHeightRowContainer);

        JumpHeightBonus = SLabel.Text("Current : + 0%").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeightBonus.SetParent(JumpHeightRowContainer);

        JumpHeighCost = SLabel.Text("Cost: 2").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10);
        JumpHeighCost.SetParent(JumpHeightRowContainer);

        var JumpHeightBtn = SLabel.Text("Upgrade").FontColor(Color.white).Font(EFont.RobotoRegular).FontSize(32).PHeight(10).OnClick(Test);
        JumpHeightBtn.SetParent(JumpHeightRowContainer);



        // SG BUTTON, I USE LABEL BUTTON CURRENTLY
        //var WalkSpeedBtn = SBgButton.Background(EBackground.None).Text("Upgrade").FontColor(Color.white).FontSize(32).PHeight(50)
        //    .Notify(Test);
        //WalkSpeedBtn.SetParent(WalkSpeedRowContainer);

        // TEST CONTAINERS
        //var WalkSpeedContainer2 = SContainer.Background(ComponentBlack).Height(100).Horizontal();
        //WalkSpeedContainer2.SetParent(CoulumContainer);
        //var WalkSpeedContainer3 = SContainer.Background(ComponentBlack).Height(100).Horizontal();
        //WalkSpeedContainer3.SetParent(CoulumContainer);
    }

    internal static void Close()
    {
        TogglePanel(MOD_LIST_ID);
    }
}