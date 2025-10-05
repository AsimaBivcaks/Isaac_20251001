using Godot;
using System;

public partial class UIPickupStat : Control
{
    [Export] private NodePath coinLabPath;
    [Export] private NodePath keyLabPath;
    [Export] private NodePath bombLabPath;

    private Label coinLab;
    private Label keyLab;
    private Label bombLab;

    private Player player;
    private PlayerMoneyManagementBehavior moneyBehavior;
    private PlayerKeyManagementBehavior keyBehavior;
    private PlayerBombBehavior bombBehavior;

    public override void _Ready()
    {
        coinLab = GetNode<Label>(coinLabPath);
        keyLab = GetNode<Label>(keyLabPath);
        bombLab = GetNode<Label>(bombLabPath);

        WorldUtilsBlackboard.Set("ui_pickup_stat", this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (player == null)
        {
            player = WorldUtilsBlackboard.Get<Player>("player_instance");
            if (player != null)
            {
                moneyBehavior = player.GetBehavior<PlayerMoneyManagementBehavior>(BehaviorType.PlayerMoneyManagement);
                keyBehavior = player.GetBehavior<PlayerKeyManagementBehavior>(BehaviorType.PlayerKeyManagement);
                bombBehavior = player.GetBehavior<PlayerBombBehavior>(BehaviorType.PlayerBomb);
            }
        }
        else
        {
            coinLab.Text = Math.Min(moneyBehavior.Money,99).ToString("D2");
            keyLab.Text = Math.Min(keyBehavior.Keys,99).ToString("D2");
            bombLab.Text = Math.Min(bombBehavior.Bombs,99).ToString("D2");
        }
    }
}
