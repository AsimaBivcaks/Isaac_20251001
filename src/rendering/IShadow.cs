using Godot;

public interface IShadow
{
    public float ZPosition { get; }
    public Vector2 XYPosition { get; }
    //don't really need this for shadowing, but xypos should be implemented in any object that has a shadow
}