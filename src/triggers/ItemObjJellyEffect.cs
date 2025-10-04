using Godot;
using System;

public partial class ItemObjJellyEffect
{
    private const float K = 800f;
    private const float MU = 20f;
    private const float POISSON = .7f;

    private Sprite2D sprite;

    private float squeezeRatio = 0;
    private float dSqueezeRatio = 0;


    public ItemObjJellyEffect(Sprite2D _sprite)
    {
        sprite = _sprite;
    }

    public void _Process(double delta)
    {
        float d2SqueezeRatio = -squeezeRatio * K;
        dSqueezeRatio += d2SqueezeRatio * (float)delta;
        dSqueezeRatio -= dSqueezeRatio * MathF.Min(1f ,MU * (float)delta);
        squeezeRatio += dSqueezeRatio * (float)delta;
        ApplySqueezeRatio();
    }

    //+Sqeeze X, -Squeeze Y
    public void Squeeze(float kineticX)
    {
        dSqueezeRatio += kineticX;
        dSqueezeRatio = Math.Clamp(dSqueezeRatio, -5f, 5f);
    }

    private void ApplySqueezeRatio()
    {
        sprite.Scale = new Vector2(1 - squeezeRatio * POISSON, 1 + squeezeRatio);
    }
}