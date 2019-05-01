using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums //
public enum Province {
    // Remove From Level Gen
    Guangxi,
    Guangdong,
    Fujian,
    Jiangxi,
    Hunan,
    Hubei,
    Anhui,
    Zhejiang,
    Jiangsu,
    Shandong,
    Henan,
    Shanxi,
    Hebei,
    Beijing,
    Mongolia,
    Liaoning,
    Jilin,
    Heilongjiang
}

public enum RegionType {
    urban,
    rural,
    jungle,
    plains,
    flooded,
    camp,
    citadel,
    palace,
    shrine
}

public enum WeatherType {
    normal,
    raining,
    snowing,
    heat
}

public enum ObjectiveType {
    assassination,
    key_item,
    seek_and_destroy,
    rescue,
    eavesdropping,
    check_point,
    tutorial
}

public enum NPCType {
    none = 0x00,
    imperialists = 0x01,
    nationalists = 0x02,
    warlord_clique = 0x04,
    monarchists = 0x08,
    communists = 0x010,
    westerners = 0x20,
    bandits = 0x40,
    villagers = 0x80,
    civilians = 0x100,
    militias = 0x200
}

[CreateAssetMenu(fileName = "Level File", menuName = "LevelGeneration/LevelFile", order = 0)]
public class LevelGen : ScriptableObject
{
    
    // Data //
    [Header("Debug")]
    public bool debug = false;
    public bool custom_level = false;
    public GameObject custom_level_prefab = null;

    [Header("Level Traits")]
    public Province province = Province.Guangdong;
    public RegionType region = RegionType.rural;
    public WeatherType weather = WeatherType.normal;
    public NPCType enemy = NPCType.imperialists;
    public NPCType npcs = NPCType.villagers;

    public ObjectiveType objective = ObjectiveType.assassination;

    [Header("Generated Level Traits")]
    public bool random_weather = false;
    [Range(0f, 1f)]
    public float rain_chance = 0f;
    [Range(0f, 1f)]
    public float snow_chance = 0f;
    [Range(0f, 1f)]
    public float heat_chance = 0f;

    public bool random_enemy = false;
    public NPCType[] possible_enemies = { NPCType.imperialists, NPCType.warlord_clique, NPCType.nationalists };
    public bool random_npc = false;
    public NPCType[] possible_npcs = { NPCType.villagers, NPCType.militias };

    public bool random_conflict = false;
    [Range(0f, 1f)]
    public float conflict_chance = 0f;

    public bool random_objective = false;
    public ObjectiveType[] possible_objectives = { ObjectiveType.assassination, ObjectiveType.key_item, ObjectiveType.rescue };
    public bool random_objective_chances = false;
    public float[] objective_chances = { 1f, 0f, 0f };

    [Header("Colors")]
    public ColorPalette level_colors;

    public bool random_colors = false;
    public ColorPalette[] random_palette = null;

    [Header("Generation Settings")]
    [Range(10, 250)]
    public int level_size = 10;
    [Range(1, 20)]
    public int buildings = 1;

    public bool random_settings = false;
    [Range(10, 250)]
    public int min_level_size = 20;
    [Range(10, 250)]
    public int max_level_size = 20;
    [Range(1, 20)]
    public int min_buildings = 1;
    [Range(1, 20)]
    public int max_buildings = 1;

    [Header("Generators")]
    public Generator[] generator_file;

    public void generateLevel() {
        if (generator_file.Length > 0) {
            generator_file[Random.Range(0, generator_file.Length - 1)].generate(this);
        }
    }

}
