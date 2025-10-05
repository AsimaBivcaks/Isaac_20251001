using Godot;
using System;

[GlobalClass]
public partial class GnawedLeafEffect : EffectItem
{
    public override void OnActive(Player player)
    {
        player.AddBehavior(new GnawedLeafBehavior(player), BehaviorType.GLEff);
    }

    public override void OnRemove(Player player)
    {
        player.RemoveBehavior(BehaviorType.GLEff);
    }
}