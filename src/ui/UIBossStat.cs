using Godot;
using System;
using System.Collections.Generic;

public partial class UIBossStat : Control
{
    [Export] private NodePath barPath;

    private Node2D bar;
    private List<Character> bosses;

    private int currentMaxHP = 0;

    public override void _Ready()
    {
        Visible = false;
        bar = GetNode<Node2D>(barPath);
        bosses = WorldUtilsBlackboard.Get<List<Character>>("current_bosses");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (bosses.Count == 0)
        {
            Visible = false;
            return;
        }
        else
        {
            if( Visible == false )
            {
                Visible = true;
                currentMaxHP = 0;
                foreach (Character boss in bosses)
                {
                    currentMaxHP += boss.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).MaxHP;
                }
            }
            int currentHP = 0;
            foreach (Character boss in bosses)
            {
                currentHP += boss.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).HP;
            }
            float ratio = (float)currentHP / currentMaxHP;
            bar.Position = new Vector2( -107 * (1 - ratio), 0);
        }
    }
}
