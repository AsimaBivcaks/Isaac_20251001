using Godot;
using System;

[GlobalClass]
public partial class BombItem : Item
{
    [Export] public int BombAmount = 1;

    public override void OnPlayerGet(Player player)
    {
        var bombBehavior = player.GetBehavior<PlayerBombBehavior>(BehaviorType.PlayerBomb);
        if (bombBehavior != null)
        {
            bombBehavior.AddBomb(BombAmount);
        }
    }

    public override bool IsPickable(Player player)
    {
        var bombBehavior = player.GetBehavior<PlayerBombBehavior>(BehaviorType.PlayerBomb);
        return bombBehavior != null && bombBehavior.CanAddBomb();
    }
}