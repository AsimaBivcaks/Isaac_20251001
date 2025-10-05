using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DistantAdmirationEffect : EffectItem
{
    PackedScene scene = GD.Load<PackedScene>(WorldUtilsPools.resourcePaths["pref_distant_admiration"]);
    DistantAdmiration distantAdmirationInstance = null;

    public override void OnActive(Player player)
    {
        distantAdmirationInstance = scene.Instantiate<DistantAdmiration>();
        distantAdmirationInstance.GlobalPosition = player.GlobalPosition;
        player.Mount.CallDeferred("add_child", distantAdmirationInstance);
    }

    public override void OnRemove(Player player)
    {
        if (distantAdmirationInstance != null)
        {
            distantAdmirationInstance.QueueFree();
            distantAdmirationInstance = null;
        }
    }
}