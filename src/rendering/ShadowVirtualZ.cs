using Godot;
using System;

//Used by objects that don't really move along Y axis, but solely move their sprites
//ZPosition points upwards(<0)
public partial class ShadowVirtualZ : Sprite2D
{
    private const float SHADOW_VISIBLE_MAX_HEIGHT = 100.0f;

    private IShadow parentObject;

    private float originalWidth;
    [Export] private float fullWidth = 16f;
    [Export] public Vector2 offset = new Vector2(0, 10f);
    
    public override void _Ready()
    {
        base._Ready();
        parentObject = GetParent<IShadow>();
        if(parentObject == null)
            GD.PrintErr("Shadow must be a child of an IShadow object.");
        
        Modulate = new Color(0, 0, 0, 0.2f);
        Position = offset;

        Texture = GD.Load<Texture2D>("res://graphics/shadow.png");
        originalWidth = Texture.GetSize().X;

        ZIndex = -100;

        Update();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Update();
    }

    private void Update()
    {
        float destWidth = fullWidth * (1 + MathF.Min(0, parentObject.ZPosition) / SHADOW_VISIBLE_MAX_HEIGHT);
        if (destWidth < 10)
        {
            destWidth = 10;
        } else if (destWidth > fullWidth)
        {
            destWidth = fullWidth;
        }
        Modulate = new Color(0, 0, 0, MathF.Max(0.2f, 0.35f * (destWidth / fullWidth)) );
        Scale = new Vector2(destWidth / originalWidth, destWidth / originalWidth);
    }
}