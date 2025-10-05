using Godot;
using System;

public enum DamageType
{
    MELEE,
    NORMAL,
    FIRE,
    EXPLOSION,
}

public record DamageData(
    CharacterBody2D attacker,
    int damageAmount,
    DamageType damageType,
    Vector2 knockbackVector
);