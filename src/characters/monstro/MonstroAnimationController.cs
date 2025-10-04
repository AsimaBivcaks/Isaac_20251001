using Godot;
using System;

public partial class MonstroAnimationController : AnimationPlayer
{
    [Export] public float K = 20f;
    [Export] public float Mu = 100f;
    [Export] public float Poisson = 0.5f;
    [Export] public NodePath spritePath;

    private Sprite2D sprite;

    private float squeezeRatio = 0;
    private float dSqueezeRatio = 0;

    private float spriteHeight;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite2D>(spritePath);
        spriteHeight = sprite.Texture.GetHeight();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        float d2SqueezeRatio = -squeezeRatio * K;
        dSqueezeRatio += d2SqueezeRatio * (float)delta;
        dSqueezeRatio -= dSqueezeRatio * MathF.Min(1f ,Mu * (float)delta);
        squeezeRatio += dSqueezeRatio * (float)delta;
        ApplySqueezeRatio();
    }

    public void Squeeze(float kinetic)
    {
        dSqueezeRatio += kinetic * 10;
    }

    private void ApplySqueezeRatio()
    {
        sprite.Scale = new Vector2(1 - squeezeRatio * Poisson, 1 + squeezeRatio);
        sprite.Position = new Vector2(0, -spriteHeight * squeezeRatio * 0.5f);
    }
}