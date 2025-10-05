using Godot;
using System;

public interface IBlastable
{
    public void OnBlast(Vector2 sourcePosition, float strength);
}