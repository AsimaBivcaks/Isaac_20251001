using Godot;
using System;

[GlobalClass]
public partial class RazorBladeUsable : UsableItem
{
    public override void OnUse(Player player)
    {
        if(usableManager.TryUseEnergy(0))
        {
            var originalRBB = player.GetBehavior<RazorBladeBehavior>(BehaviorType.RBEff);
            if(originalRBB != null)
            {
                originalRBB.PlugIn();
            }
            else
            {
                player.AddBehavior(new RazorBladeBehavior(player), BehaviorType.RBEff, false);
            }
            var damage = new DamageData(null, 2, DamageType.RAZOR, Vector2.Zero);
            player.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(damage);
        }
    }

}