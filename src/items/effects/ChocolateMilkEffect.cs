using Godot;
using System;

[GlobalClass]
public partial class ChocolateMilkEffect : EffectItem
{
    private CharacterBehavior originalBehavior;
    private CharacterBehavior thisBehavior;

    private PackedScene originalScene;
    private PackedScene projectileScene;

    public override void OnActive(Player player)
    {
        originalBehavior = player.GetBehavior(BehaviorType.Attack);
        player.RemoveBehavior(BehaviorType.Attack);
        if (projectileScene == null)
        {
            projectileScene = WorldUtilsPools.GetResource<PackedScene>("proj_chocolate_milk_tear");
        }
        if (thisBehavior == null)
        {
            thisBehavior = new ChocolateMilkBehavior(player, player.EmitCDRefValue);
        }
        player.AddBehavior(thisBehavior, BehaviorType.Attack);
        originalScene = ((ProjectileFactoryComposite)player.projectileFactory).baseFactory.projectileScene;
        ((ProjectileFactoryComposite)player.projectileFactory).baseFactory.projectileScene = projectileScene;
    }

    public override void OnRemove(Player player)
    {
        if(player.GetBehavior(BehaviorType.Attack) != thisBehavior)
            return;
        player.RemoveBehavior(BehaviorType.Attack);
        if(((ProjectileFactoryComposite)player.projectileFactory).baseFactory.projectileScene == projectileScene)
        {
            ((ProjectileFactoryComposite)player.projectileFactory).baseFactory.projectileScene = originalScene;
        }
        player.AddBehavior(originalBehavior, BehaviorType.Attack);
    }
}