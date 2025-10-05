using Godot;
using System;

public partial class UIHpStat : Control
{
    [Export] private Texture2D textureHearts;
    [Export] private Vector2I frames = new Vector2I(7, 4);

    [ExportGroup("FrameCoords")]
    [Export] private Vector2I fullHeartFrame = new Vector2I(0, 0);
    [Export] private Vector2I halfHeartFrame = new Vector2I(1, 0);
    [Export] private Vector2I emptyHeartFrame = new Vector2I(2, 0);

    private Sprite2D[] hearts = new Sprite2D[12];

    private PlayerHPBehavior hp;

    private void SetFrame(int index, Vector2I frame)
    {
        hearts[index].Frame = frame.Y * frames.X + frame.X;
    }

    public override void _Ready()
    {
        for (int i = 0; i < 12; i++)
        {
            hearts[i] = new Sprite2D();
            hearts[i].Position = new Vector2(14 * (i % 6), 14 * (i / 6));
            hearts[i].Texture = textureHearts;
            hearts[i].Hframes = frames.X;
            hearts[i].Vframes = frames.Y;
            hearts[i].Centered = false;
            SetFrame(i, emptyHeartFrame);
            hearts[i].Visible = false;
            AddChild(hearts[i]);
        }
        Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (hp == null)
        {
            var player = WorldUtilsBlackboard.Get<Player>("player_instance");
            if (player != null)
            {
                hp = player.GetBehavior<PlayerHPBehavior>(BehaviorType.HP);
                Visible = true;
            }
            else return;
        }

        for (int i = 0; i < 12; i++)
        {
            if (i*2 + 1 < hp.HP)
            {
                SetFrame(i, fullHeartFrame);
                hearts[i].Visible = true;
            }
            else if (i*2 + 1 == hp.HP)
            {
                SetFrame(i, halfHeartFrame);
                hearts[i].Visible = true;
            }
            else if (i*2 < hp.MaxHP)
            {
                SetFrame(i, emptyHeartFrame);
                hearts[i].Visible = true;
            }
            else
            {
                hearts[i].Visible = false;
            }
        }
    }
}
