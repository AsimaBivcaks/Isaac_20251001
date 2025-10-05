
using System.Collections.Generic;

public static class WorldUtilsPools
{
    public readonly static Dictionary<string, string> resourcePaths = new Dictionary<string, string>()
    {
        {"item_obj", "res://scenes/triggers/item_obj.tscn"},
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

        {"eff_innereye", "res://data/items/innereye_eff.tres"},
        {"eff_distant_admiration", "res://data/items/distant_admiration_eff.tres"},

        {"pref_distant_admiration", "res://scenes/characters/distant_admiration.tscn"},
    };

    static WorldUtilsPools()
    {

    }
}