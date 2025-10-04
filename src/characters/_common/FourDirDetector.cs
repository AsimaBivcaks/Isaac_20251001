using Godot;
using System;

public partial class FourDirDetector : Node2D
{
    private static readonly Vector2[] directions = new Vector2[4]{
        Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right,
    };
    private RayCast2D[] rays = new RayCast2D[4];

    [Export] private float detectRange = 200f;
    [Export(PropertyHint.Layers2DPhysics)] private uint collisionLayer = 1;

    public override void _Ready()
    {
        for(int i = 0; i < 4; i++)
        {
            rays[i] = new RayCast2D();
            rays[i].TargetPosition = directions[i] * detectRange;
            rays[i].Enabled = true;
            rays[i].ExcludeParent = true;
            rays[i].CollisionMask = collisionLayer;
            AddChild(rays[i]);
        }
    }

    public Vector2 Detect<T>() where T : Character
    {
        foreach (var ray in rays)
        {
            if (ray.IsColliding())
            {
                Object collider = ray.GetCollider();
                if (collider is T)
                {
                    return ((T)collider).GlobalPosition - GlobalPosition;
                }
            }
        }
        return Vector2.Zero;
    }
}
