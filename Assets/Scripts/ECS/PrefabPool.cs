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
      { "Prefabs/EventSystem", Resources.Load<GameObject>("Prefabs/EventSystem") },
      { "Prefabs/Entities/Animal", Resources.Load<GameObject>("Prefabs/Entities/Animal") },
      { "Prefabs/Entities/Mob", Resources.Load<GameObject>("Prefabs/Entities/Mob") },
      
      { "Prefabs/Items/item_template", Resources.Load<GameObject>("Prefabs/Items/item_template") },

      { "Prefabs/Models/model_coin", Resources.Load<GameObject>("Prefabs/Models/model_coin") },

      { "Prefabs/Models/model_rock", Resources.Load<GameObject>("Prefabs/Models/model_rock") },
      { "Prefabs/Models/model_ore_iron", Resources.Load<GameObject>("Prefabs/Models/model_ore_iron") },
      { "Prefabs/Models/model_ore_coal", Resources.Load<GameObject>("Prefabs/Models/model_ore_coal") },
      { "Prefabs/Models/model_ore_gold", Resources.Load<GameObject>("Prefabs/Models/model_ore_gold") },
      { "Prefabs/Models/model_ore_mythril", Resources.Load<GameObject>("Prefabs/Models/model_ore_mythril") },
      { "Prefabs/Models/model_ore_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_ore_adamantite") },

      { "Prefabs/Models/model_ingot_iron", Resources.Load<GameObject>("Prefabs/Models/model_ingot_iron") },
      { "Prefabs/Models/model_ingot_gold", Resources.Load<GameObject>("Prefabs/Models/model_ingot_gold") },
      { "Prefabs/Models/model_ingot_mythril", Resources.Load<GameObject>("Prefabs/Models/model_ingot_mythril") },
      { "Prefabs/Models/model_ingot_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_ingot_adamantite") },

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
      
      { "Prefabs/Models/model_wheat", Resources.Load<GameObject>("Prefabs/Models/model_wheat") },
      { "Prefabs/Models/model_flax", Resources.Load<GameObject>("Prefabs/Models/model_flax") },
      
      { "Prefabs/Models/model_farm_planter", Resources.Load<GameObject>("Prefabs/Models/model_farm_planter") },
      { "Prefabs/Models/model_crafting_station", Resources.Load<GameObject>("Prefabs/Models/model_crafting_station") },
      { "Prefabs/Models/model_fletching_table", Resources.Load<GameObject>("Prefabs/Models/model_fletching_table") },
      { "Prefabs/Models/model_anvil", Resources.Load<GameObject>("Prefabs/Models/model_anvil") },
      { "Prefabs/Models/model_furnace", Resources.Load<GameObject>("Prefabs/Models/model_furnace") },
      { "Prefabs/Models/model_cauldron", Resources.Load<GameObject>("Prefabs/Models/model_cauldron") },
      
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
    
      { "Prefabs/Models/model_sword_wooden", Resources.Load<GameObject>("Prefabs/Models/model_sword_wooden") },
      { "Prefabs/Models/model_sword_iron", Resources.Load<GameObject>("Prefabs/Models/model_sword_iron") },
      { "Prefabs/Models/model_sword_mythril", Resources.Load<GameObject>("Prefabs/Models/model_sword_mythril") },
      { "Prefabs/Models/model_sword_adamantite", Resources.Load<GameObject>("Prefabs/Models/model_sword_adamantite") },
      
      { "Prefabs/Models/model_meat_cooked", Resources.Load<GameObject>("Prefabs/Models/model_meat_cooked") },
      { "Prefabs/Models/model_bread", Resources.Load<GameObject>("Prefabs/Models/model_bread") },
      { "Prefabs/Models/model_pot_pie", Resources.Load<GameObject>("Prefabs/Models/model_pot_pie") },
      { "Prefabs/Models/model_beef_stew", Resources.Load<GameObject>("Prefabs/Models/model_beef_stew") },

      { "Prefabs/Deployables/deployable_crafting_station", Resources.Load<GameObject>("Prefabs/Deployables/deployable_crafting_station") },
      { "Prefabs/Deployables/deployable_fletching_table", Resources.Load<GameObject>("Prefabs/Deployables/deployable_fletching_table") },
      { "Prefabs/Deployables/deployable_anvil", Resources.Load<GameObject>("Prefabs/Deployables/deployable_anvil") },
      { "Prefabs/Deployables/deployable_furnace", Resources.Load<GameObject>("Prefabs/Deployables/deployable_furnace") },
      { "Prefabs/Deployables/deployable_cauldron", Resources.Load<GameObject>("Prefabs/Deployables/deployable_cauldron") },
      { "Prefabs/Deployables/deployable_farm_planter", Resources.Load<GameObject>("Prefabs/Deployables/deployable_farm_planter") },

      { "Prefabs/Deployables/blueprint_crafting_station", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_crafting_station") },
      { "Prefabs/Deployables/blueprint_fletching_table", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_fletching_table") },
      { "Prefabs/Deployables/blueprint_anvil", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_anvil") },
      { "Prefabs/Deployables/blueprint_furnace", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_furnace") },
      { "Prefabs/Deployables/blueprint_cauldron", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_cauldron") },
      { "Prefabs/Deployables/blueprint_farm_planter", Resources.Load<GameObject>("Prefabs/Deployables/blueprint_farm_planter") },

      { "Prefabs/Buildings/blueprint_building_foundation", Resources.Load<GameObject>("Prefabs/Buildings/blueprint_building_foundation") },
      { "Prefabs/Buildings/building_foundation", Resources.Load<GameObject>("Prefabs/Buildings/building_foundation") },
      { "Prefabs/Buildings/blueprint_building_wall", Resources.Load<GameObject>("Prefabs/Buildings/blueprint_building_wall") },
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
        Object.Instantiate(Prefabs["Prefabs/Models/model_seed_wheat"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Models/model_seed_flax",
        Object.Instantiate(Prefabs["Prefabs/Models/model_seed_flax"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Buildings/building_foundation",
        Object.Instantiate(Prefabs["Prefabs/Buildings/blueprint_building_foundation"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Buildings/building_wall",
        Object.Instantiate(Prefabs["Prefabs/Buildings/blueprint_building_wall"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_crafting_station",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_crafting_station"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_fletching_table",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_fletching_table"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_anvil",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_anvil"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_furnace",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_furnace"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_cauldron",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_cauldron"], Vector3.zero, Quaternion.identity)
      },
      {
        "Prefabs/Deployables/deployable_farm_planter",
        Object.Instantiate(Prefabs["Prefabs/Deployables/blueprint_farm_planter"], Vector3.zero, Quaternion.identity)
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

public static class AnimationPool
{
  public static Dictionary<string, AnimationClip> Animations;

  public static void LoadAnimations()
  {
    Animations = new Dictionary<string, AnimationClip>()
    {
      { "Animations/Totem/Totem-0-Progress", Resources.Load<AnimationClip>("Animations/Totem/Totem-0-Progress") },
      { "Animations/Totem/Totem-1-Progress", Resources.Load<AnimationClip>("Animations/Totem/Totem-1-Progress") },
      { "Animations/Totem/Totem-2-Progress", Resources.Load<AnimationClip>("Animations/Totem/Totem-2-Progress") },
      { "Animations/Totem/Totem-3-Progress", Resources.Load<AnimationClip>("Animations/Totem/Totem-3-Progress") },
    };
  }
}