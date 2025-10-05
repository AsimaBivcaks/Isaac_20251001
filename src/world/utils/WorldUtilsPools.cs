using Godot;
using System.Collections.Generic;

public static class WorldUtilsPools
{
    public readonly static Dictionary<string, string> resourcePaths = new Dictionary<string, string>()
    {
        {"item_obj", "res://scenes/triggers/item_obj.tscn"},
        {"itembase_obj", "res://scenes/triggers/item_base_obj.tscn"},
        {"bomb_obj", "res://scenes/triggers/active_bomb.tscn"},

        {"proj_tear", "res://scenes/projectiles/tear.tscn"},
        {"proj_e_bloodtear", "res://scenes/projectiles/e_bloodtear.tscn"},
        {"proj_e_aired_bloodtear", "res://scenes/projectiles/e_aired_bloodtear.tscn"},
        {"proj_explosion", "res://scenes/projectiles/explosion.tscn"},

        {"img_shadow", "res://graphics/shadow.png"},

        {"item_heart", "res://data/items/heart_item.tres"},
        {"item_bomb", "res://data/items/bomb_item.tres"},
        {"item_coin", "res://data/items/coin_item.tres"},
        {"item_key", "res://data/items/key_item.tres"},
        {"item_battery", "res://data/items/battery_item.tres"},

        {"eff_innereye", "res://data/items/innereye_eff.tres"},
        {"eff_distant_admiration", "res://data/items/distant_admiration_eff.tres"},

        {"pref_distant_admiration", "res://scenes/characters/distant_admiration.tscn"},
        {"pref_knight", "res://scenes/characters/knight.tscn"},
        {"pref_monstro", "res://scenes/characters/monstro.tscn"},
        {"pref_nerveending", "res://scenes/characters/nerveending.tscn"},
        {"pref_red_fly", "res://scenes/characters/red_fly.tscn"},
        {"pref_selfless_knight", "res://scenes/characters/selfless_knight.tscn"},

        {"player", "res://scenes/characters/player.tscn"},
        
        {"room_test", "res://scenes/rooms/basement_0.tscn"},
    };

    public static T GetResource<T>(string key) where T : class
    {
        if (resourcePaths.ContainsKey(key))
        {
            return GD.Load<T>(resourcePaths[key]);
        }
        return null;
    }

    static WorldUtilsPools()
    {

    }
}