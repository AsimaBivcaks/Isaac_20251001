using Godot;
using System;
using System.Collections.Generic;

public partial class UIDemowin : Panel
{
    private bool bossHasLoaded = false;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (WorldUtilsBlackboard.Get<List<Character>>("current_bosses").Count > 0)
        {
            bossHasLoaded = true;
        }

        if (bossHasLoaded && WorldUtilsBlackboard.Get<List<Character>>("current_bosses").Count == 0)
        {
            Visible = true;
            Tween tween = CreateTween();
            tween.TweenCallback(Callable.From(() => GetTree().Quit())).SetDelay(3f);
        }
    }
}
