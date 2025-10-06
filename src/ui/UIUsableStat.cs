using Godot;
using System;

public partial class UIUsableStat : Control
{
    private static readonly int[] slot_frames = new int[] {
        -1, 8, 7, 6, 5, 4, 3, -1, 9, -1, -1, -1, 2
    };

    private static readonly int[][] bar_heights = new int[][] {
        new int[] {},
        new int[] {0, 25},
        new int[] {0, 12, 25},
        new int[] {0, 8, 16, 25},
        new int[] {0, 6, 12, 18, 25},
        new int[] {0, 5, 10, 15, 19, 25},
        new int[] {0, 4, 8, 12, 16, 20, 25},
        new int[] {},
        new int[] {0, 3, 6, 9, 12, 15, 18, 20, 25},
        new int[] {},
        new int[] {},
        new int[] {},
        new int[] {0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 25},
    }; //I HATE THIS

    [Export] public NodePath IconPath;
    [Export] public NodePath BarPath;
    [Export] public NodePath SlotsPath;

    private Sprite2D icon;
    private Sprite2D bar;
    private Sprite2D slots;

    private PlayerUsableManagementBehavior usable;
    
    public override void _Ready()
    {
        icon = GetNode<Sprite2D>(IconPath);
        bar = GetNode<Sprite2D>(BarPath);
        slots = GetNode<Sprite2D>(SlotsPath);
        Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (usable == null)
        {
            var player = WorldUtilsBlackboard.Get<Player>("player_instance");
            if (player != null)
            {
                usable = player.GetBehavior<PlayerUsableManagementBehavior>(BehaviorType.PlayerUsableManagement);
            }
            else return;
            if (usable == null)
                return;
        }

        if (!Visible)
        {
            if (usable.MaxEnergy > 0)
            {
                Visible = true;
            }
            else return;
        }
        else if (usable.MaxEnergy <= 0 || usable.item == null)
        {
            Visible = false;
            return;
        }

        icon.Texture = usable.item.Icon;
        slots.Frame = slot_frames[Math.Clamp(usable.MaxEnergy, 0, slot_frames.Length - 1)];
        bar.Position = 
            new Vector2(0,
                bar_heights
                    [Math.Clamp(usable.MaxEnergy, 0, bar_heights.Length - 1)]
                    [Math.Clamp(usable.MaxEnergy - usable.Energy, 0, usable.MaxEnergy)]
            );
    }
}
