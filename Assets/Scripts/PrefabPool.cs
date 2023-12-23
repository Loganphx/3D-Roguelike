using System.Collections.Generic;
using UnityEngine;

public static class PrefabPool
{
  public static Dictionary<string, GameObject> Prefabs;

  public static void LoadPrefabs()
  {
    Prefabs = new Dictionary<string, GameObject>()
    {
      { "Prefabs/Player", Resources.Load<GameObject>("Prefabs/Player") },
      { "Prefabs/Entities/Animal", Resources.Load<GameObject>("Prefabs/Entities/Animal") },
      
      { "Prefabs/Items/item_template", Resources.Load<GameObject>("Prefabs/Items/item_template") },

      { "Prefabs/Models/model_ore_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_ore_adamantite") },
      { "Prefabs/Models/model_coin", Resources.Load<GameObject>("Prefabs/Models/model_coin") },
      { "Prefabs/Models/model_ore_coal", Resources.Load<GameObject>("Prefabs/Models/model_ore_coal") },
      { "Prefabs/Models/model_ore_gold", Resources.Load<GameObject>("Prefabs/Models/model_ore_gold") },
      { "Prefabs/Models/model_ore_iron", Resources.Load<GameObject>("Prefabs/Models/model_ore_iron") },
      { "Prefabs/Models/model_ore_mythril", Resources.Load<GameObject>("Prefabs/Models/model_ore_mythril") },
      { "Prefabs/Models/model_rock", Resources.Load<GameObject>("Prefabs/Models/model_rock") },
      { "Prefabs/Models/model_wood_tree", Resources.Load<GameObject>("Prefabs/Models/model_wood_tree") },
      { "Prefabs/Models/model_wood_birch", Resources.Load<GameObject>("Prefabs/Models/model_wood_birch") },
      { "Prefabs/Models/model_wood_fir", Resources.Load<GameObject>("Prefabs/Models/model_wood_fir") },
      { "Prefabs/Models/model_wood_oak", Resources.Load<GameObject>("Prefabs/Models/model_wood_oak") },
      { "Prefabs/Models/model_wood_darkoak", Resources.Load<GameObject>("Prefabs/Models/model_wood_darkoak") },
      
      { "Prefabs/Models/model_mushroom_healshroom", Resources.Load<GameObject>("Prefabs/Models/model_mushroom_healshroom") },
      { "Prefabs/Models/model_mushroom_munchshroom", Resources.Load<GameObject>("Prefabs/Models/model_mushroom_munchshroom") },
      { "Prefabs/Models/model_mushroom_zoomshroom", Resources.Load<GameObject>("Prefabs/Models/model_mushroom_zoomshroom") },

      { "Prefabs/Models/model_seed_wheat", Resources.Load<GameObject>("Prefabs/Models/model_seed_wheat") },
      { "Prefabs/Models/model_seed_flax", Resources.Load<GameObject>("Prefabs/Models/model_seed_flax") },
      
      { "Prefabs/Models/model_building_foundation", Resources.Load<GameObject>("Prefabs/Models/model_building_foundation") },
      { "Prefabs/Models/model_building_wall", Resources.Load<GameObject>("Prefabs/Models/model_building_wall") },
      
      { "Prefabs/Models/model_pickaxe_wooden", Resources.Load<GameObject>("Prefabs/Models/model_pickaxe_wooden") },
      { "Prefabs/Models/model_pickaxe_iron", Resources.Load<GameObject>("Prefabs/Models/model_pickaxe_iron") },
      { "Prefabs/Models/model_pickaxe_mythril", Resources.Load<GameObject>("Prefabs/Models/model_pickaxe_mythril") },
      { "Prefabs/Models/model_pickaxe_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_pickaxe_adamantite") },
    
      { "Prefabs/Models/model_axe_wooden", Resources.Load<GameObject>("Prefabs/Models/model_axe_wooden") },
      { "Prefabs/Models/model_axe_iron", Resources.Load<GameObject>("Prefabs/Models/model_axe_iron") },
      { "Prefabs/Models/model_axe_mythril", Resources.Load<GameObject>("Prefabs/Models/model_axe_mythril") },
      { "Prefabs/Models/model_axe_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_axe_adamantite") },
      
      { "Prefabs/Buildings/blueprint_building_foundation", Resources.Load<GameObject>("Prefabs/Buildings/blueprint_building_foundation") },
      { "Prefabs/Buildings/building_foundation", Resources.Load<GameObject>("Prefabs/Buildings/building_foundation") },
      { "Prefabs/Buildings/building_wall", Resources.Load<GameObject>("Prefabs/Buildings/building_wall") },
      
      { "Prefabs/Crops/crop_wheat", Resources.Load<GameObject>("Prefabs/Crops/crop_wheat") },
      { "Prefabs/Crops/crop_flax", Resources.Load<GameObject>("Prefabs/Crops/crop_flax") },
    
      { "Prefabs/Powerups/powerup_common", Resources.Load<GameObject>("Prefabs/Powerups/powerup_common") },
      { "Prefabs/Powerups/powerup_uncommon", Resources.Load<GameObject>("Prefabs/Powerups/powerup_uncommon") },
      { "Prefabs/Powerups/powerup_rare", Resources.Load<GameObject>("Prefabs/Powerups/powerup_rare") },
      { "Prefabs/Powerups/powerup_epic", Resources.Load<GameObject>("Prefabs/Powerups/powerup_epic") },
      { "Prefabs/Powerups/powerup_legendary", Resources.Load<GameObject>("Prefabs/Powerups/powerup_legendary") },
    };
    SpawnedPrefabs = new Dictionary<string, GameObject>()
    {
      {
        "Prefabs/Models/model_seed_wheat",
        GameObject.Instantiate(Prefabs["Prefabs/Models/model_seed_wheat"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Models/model_seed_flax",
        GameObject.Instantiate(Prefabs["Prefabs/Models/model_seed_flax"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Buildings/building_foundation",
        GameObject.Instantiate(Prefabs["Prefabs/Buildings/blueprint_building_foundation"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Buildings/building_wall",
        GameObject.Instantiate(Prefabs["Prefabs/Buildings/building_wall"], Vector3.zero, Quaternion.identity)
      }
    };
  }

  public static Dictionary<string, GameObject> SpawnedPrefabs = new Dictionary<string, GameObject>();
}

public static class MaterialPool
{
  public static Dictionary<string, Material> Materials;

  public static void LoadMaterials()
  {
    Materials = new Dictionary<string, Material>()
    {
      { "Materials/InvalidBuildHover", Resources.Load<Material>("Materials/InvalidBuildHover") },
      { "Materials/BuildHover", Resources.Load<Material>("Materials/BuildHover") }
    };
  }
}