using System.Collections.Generic;

public static class ItemPool
{
  public static readonly Dictionary<ITEM_TYPE, string> ItemSprites = new Dictionary<ITEM_TYPE, string>()
  {
    { ITEM_TYPE.NULL, null },
    { ITEM_TYPE.ROCK, "Sprites/Items/rock" },
    { ITEM_TYPE.COIN, "Sprites/Items/coin" },

    { ITEM_TYPE.ORE_COAL, "Sprites/Items/ore_coal" },
    { ITEM_TYPE.ORE_IRON, "Sprites/Items/ore_iron" },
    { ITEM_TYPE.ORE_GOLD, "Sprites/Items/ore_gold" },
    { ITEM_TYPE.ORE_MYTHRIL, "Sprites/Items/ore_mythril" },
    { ITEM_TYPE.ORE_ADAMANTITE, "Sprites/Items/ore_adamantite" },

    { ITEM_TYPE.WOOD, "Sprites/Items/wood_tree" },
    { ITEM_TYPE.WOOD_BIRCH, "Sprites/Items/wood_birch" },
    { ITEM_TYPE.WOOD_FIR, "Sprites/Items/wood_fir" },
    { ITEM_TYPE.WOOD_OAK, "Sprites/Items/wood_oak" },
    { ITEM_TYPE.WOOD_DARKOAK, "Sprites/Items/wood_darkoak" },

    { ITEM_TYPE.MUSHROOM_ZOOMSHROOM, "Sprites/Items/mushroom_zoomshroom" },
    { ITEM_TYPE.MUSHROOM_HEALSHROOM, "Sprites/Items/mushroom_healshroom" },
    { ITEM_TYPE.MUSHROOM_MUNCHSHROOM, "Sprites/Items/mushroom_munchshroom" },

    { ITEM_TYPE.SEED_WHEAT, "Sprites/Items/seed_wheat" },
    { ITEM_TYPE.SEED_FLAX, "Sprites/Items/seed_flax" },
    
    { ITEM_TYPE.WHEAT, "Sprites/Items/wheat" },
    { ITEM_TYPE.FLAX, "Sprites/Items/flax" },

    { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, "Sprites/Items/deployable_farm_planter" },
    { ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, "Sprites/Items/deployable_crafting_station" },
    { ITEM_TYPE.DEPLOYABLE_FURNACE, "Sprites/Items/deployable_furnace" },
    { ITEM_TYPE.DEPLOYABLE_CAULDRON, "Sprites/Items/deployable_cauldron" },
    { ITEM_TYPE.DEPLOYABLE_CHEST, "Sprites/Items/deployable_chest" },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, "Sprites/Items/building_foundation" },
    { ITEM_TYPE.BUILDING_WALL, "Sprites/Items/building_wall" },
    { ITEM_TYPE.BUILDING_RAMP, "Sprites/Items/building_ramp" },

    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, "Sprites/Items/pickaxe_wooden" },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, "Sprites/Items/pickaxe_iron" },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, "Sprites/Items/pickaxe_mythril" },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, "Sprites/Items/pickaxe_adamantite" },
  
    { ITEM_TYPE.TOOL_AXE_WOODEN, "Sprites/Items/axe_wooden" },
    { ITEM_TYPE.TOOL_AXE_IRON, "Sprites/Items/axe_iron" },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, "Sprites/Items/axe_mythril" },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, "Sprites/Items/axe_adamantite" },
    
    { ITEM_TYPE.BARK, "Sprites/Items/bark" },
    { ITEM_TYPE.BOWL, "Sprites/Items/bowl" },
    
    { ITEM_TYPE.ARROW_STONE, "Sprites/Items/arrow_stone" },
    { ITEM_TYPE.ARROW_IRON, "Sprites/Items/arrow_iron" },
    { ITEM_TYPE.ARROW_MYTHRIL, "Sprites/Items/arrow_mythril" },
    { ITEM_TYPE.ARROW_ADAMANTITE, "Sprites/Items/arrow_adamantite" },
  };

  public static readonly Dictionary<ITEM_TYPE, string> ItemPrefabs = new Dictionary<ITEM_TYPE, string>()
  {
    { ITEM_TYPE.NULL, null },
    { ITEM_TYPE.ROCK, "Prefabs/Models/model_rock" },
    { ITEM_TYPE.COIN, "Prefabs/Models/model_coin" },

    { ITEM_TYPE.ORE_COAL, "Prefabs/Models/model_ore_coal" },
    { ITEM_TYPE.ORE_IRON, "Prefabs/Models/model_ore_iron" },
    { ITEM_TYPE.ORE_GOLD, "Prefabs/Models/model_ore_gold" },
    { ITEM_TYPE.ORE_MYTHRIL, "Prefabs/Models/model_ore_mythril" },
    { ITEM_TYPE.ORE_ADAMANTITE, "Prefabs/Models/model_ore_adamantite" },
          
    { ITEM_TYPE.WOOD, "Prefabs/Models/model_wood_tree" },
    { ITEM_TYPE.WOOD_BIRCH, "Prefabs/Models/model_wood_birch" },
    { ITEM_TYPE.WOOD_FIR, "Prefabs/Models/model_wood_fir" },
    { ITEM_TYPE.WOOD_OAK, "Prefabs/Models/model_wood_oak" },
    { ITEM_TYPE.WOOD_DARKOAK, "Prefabs/Models/model_wood_darkoak" },

    { ITEM_TYPE.MUSHROOM_HEALSHROOM, "Prefabs/Models/model_mushroom_healshroom" },
    { ITEM_TYPE.MUSHROOM_ZOOMSHROOM, "Prefabs/Models/model_mushroom_zoomshroom" },
    { ITEM_TYPE.MUSHROOM_MUNCHSHROOM, "Prefabs/Models/model_mushroom_munchshroom" },

    { ITEM_TYPE.SEED_WHEAT, "Prefabs/Models/model_seed_wheat" },
    { ITEM_TYPE.SEED_FLAX, "Prefabs/Models/model_seed_flax" },
    
    { ITEM_TYPE.WHEAT, "Prefabs/Models/model_wheat" },
    { ITEM_TYPE.FLAX, "Prefabs/Models/model_flax" },

    { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, "Prefabs/Deployables/deployable_farm_planter" },
    { ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, "Prefabs/Deployables/deployable_crafting_station" },
    { ITEM_TYPE.DEPLOYABLE_FURNACE, "Prefabs/Deployables/deployable_furnace" },
    { ITEM_TYPE.DEPLOYABLE_CAULDRON, "Prefabs/Deployables/deployable_cauldron" },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Models/model_building_foundation" },
    { ITEM_TYPE.BUILDING_WALL, "Prefabs/Models/model_building_wall" },
    { ITEM_TYPE.BUILDING_RAMP, "Prefabs/Models/model_building_foundation" },
    
    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, "Prefabs/Models/model_pickaxe_wooden" },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, "Prefabs/Models/model_pickaxe_iron" },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, "Prefabs/Models/model_pickaxe_mythril" },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, "Prefabs/Models/model_pickaxe_adamantite" },
    
    { ITEM_TYPE.TOOL_AXE_WOODEN, "Prefabs/Models/model_axe_wooden" },
    { ITEM_TYPE.TOOL_AXE_IRON, "Prefabs/Models/model_axe_iron" },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, "Prefabs/Models/model_axe_mythril" },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, "Prefabs/Models/model_axe_adamantite" },
  };

  
  // MAX STACK SIZES OF ITEMS
  public static readonly Dictionary<ITEM_TYPE, int> ItemMaxStacks = new Dictionary<ITEM_TYPE, int>()
  {
    { ITEM_TYPE.NULL, 0 },
    { ITEM_TYPE.ROCK, 69 },
    { ITEM_TYPE.COIN, 1000 },

    { ITEM_TYPE.ORE_COAL, 69 },
    { ITEM_TYPE.ORE_IRON, 69 },
    { ITEM_TYPE.ORE_GOLD, 69 },
    { ITEM_TYPE.ORE_MYTHRIL, 69 },
    { ITEM_TYPE.ORE_ADAMANTITE, 69 },
    { ITEM_TYPE.WOOD, 42 },
    { ITEM_TYPE.WOOD_BIRCH, 42 },
    { ITEM_TYPE.WOOD_FIR, 42 },
    { ITEM_TYPE.WOOD_OAK, 42 },
    { ITEM_TYPE.WOOD_DARKOAK, 42 },

    { ITEM_TYPE.SEED_WHEAT, 5 },
    { ITEM_TYPE.SEED_FLAX, 5 },
    
    { ITEM_TYPE.WHEAT, 100 },
    { ITEM_TYPE.FLAX, 100 },

    { ITEM_TYPE.MUSHROOM_HEALSHROOM, 5 },
    { ITEM_TYPE.MUSHROOM_ZOOMSHROOM, 5 },
    { ITEM_TYPE.MUSHROOM_MUNCHSHROOM, 5 },

    { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, 1 },
    { ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, 1 },
    { ITEM_TYPE.DEPLOYABLE_FURNACE, 1 },
    { ITEM_TYPE.DEPLOYABLE_CAULDRON, 1 },
    { ITEM_TYPE.DEPLOYABLE_CHEST, 1 },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, 5 },
    { ITEM_TYPE.BUILDING_WALL, 5 },
    { ITEM_TYPE.BUILDING_RAMP, 5 },

    { ITEM_TYPE.TOOL_AXE_WOODEN, 1 },
    { ITEM_TYPE.TOOL_AXE_IRON, 1 },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, 1 },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, 1 },

    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE , 1 },
    
    { ITEM_TYPE.BARK , 10 },
    { ITEM_TYPE.BOWL , 5 },
    
    { ITEM_TYPE.ARROW_STONE , 5 },
    { ITEM_TYPE.ARROW_IRON , 5 },
    { ITEM_TYPE.ARROW_MYTHRIL , 5 },
    { ITEM_TYPE.ARROW_ADAMANTITE , 5 },
  };

  // DAMAGE OF WEAPONS
  public static readonly Dictionary<ITEM_TYPE, int> ItemDamages = new Dictionary<ITEM_TYPE, int>()
  {
    { ITEM_TYPE.NULL, 0 },
    { ITEM_TYPE.ROCK, 5 },
    { ITEM_TYPE.COIN, 0 },

    { ITEM_TYPE.ORE_COAL, 0 },
    { ITEM_TYPE.ORE_IRON, 0 },
    { ITEM_TYPE.ORE_GOLD, 0 },
    { ITEM_TYPE.ORE_MYTHRIL, 0 },
    { ITEM_TYPE.ORE_ADAMANTITE, 0 },
    
    { ITEM_TYPE.WOOD, 0 },
    { ITEM_TYPE.WOOD_BIRCH, 0 },
    { ITEM_TYPE.WOOD_FIR, 0 },
    { ITEM_TYPE.WOOD_OAK, 0 },
    { ITEM_TYPE.WOOD_DARKOAK, 0 },

    { ITEM_TYPE.MUSHROOM_HEALSHROOM, 0 },
    { ITEM_TYPE.MUSHROOM_ZOOMSHROOM, 0 },
    { ITEM_TYPE.MUSHROOM_MUNCHSHROOM, 0 },

    { ITEM_TYPE.SEED_WHEAT, 0 },
    { ITEM_TYPE.SEED_FLAX, 0 },

    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, 10 },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, 25 },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, 50 },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, 100 },
    
    { ITEM_TYPE.TOOL_AXE_WOODEN, 10 },
    { ITEM_TYPE.TOOL_AXE_IRON, 25 },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, 50 },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, 100 },
    
    { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, 0 },
    { ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, 0 },
    { ITEM_TYPE.DEPLOYABLE_FURNACE, 0 },
    { ITEM_TYPE.DEPLOYABLE_CAULDRON, 0 },
    { ITEM_TYPE.DEPLOYABLE_CHEST, 0 },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, 0 },
    { ITEM_TYPE.BUILDING_WALL, 0 },
    { ITEM_TYPE.BUILDING_RAMP, 0 },
    
    { ITEM_TYPE.BARK, 0 },
    { ITEM_TYPE.BOWL, 0 },
    
    { ITEM_TYPE.ARROW_STONE, 0 },
    { ITEM_TYPE.ARROW_IRON, 0 },
    { ITEM_TYPE.ARROW_MYTHRIL, 0 },
    { ITEM_TYPE.ARROW_ADAMANTITE, 0 },
  };

  // PRICES OF ITEMS
  public static readonly Dictionary<ITEM_TYPE, int> ItemValues = new Dictionary<ITEM_TYPE, int>()
  {
    { ITEM_TYPE.WOOD, 1 },
    { ITEM_TYPE.WOOD_BIRCH, 1 },
    { ITEM_TYPE.WOOD_FIR, 1 },
    { ITEM_TYPE.WOOD_OAK, 1 },
    { ITEM_TYPE.WOOD_DARKOAK, 1 },
    { ITEM_TYPE.ROCK, 1 },
    { ITEM_TYPE.ORE_COAL, 1 },
    { ITEM_TYPE.ORE_IRON, 1 },
    { ITEM_TYPE.ORE_GOLD, 1 },
    { ITEM_TYPE.ORE_ADAMANTITE, 1 },
    { ITEM_TYPE.ORE_MYTHRIL, 1 },
    { ITEM_TYPE.COIN, 1 },
    { ITEM_TYPE.MEAT_RAW, 1 },
    { ITEM_TYPE.HIDE_WOLF, 1 },
    { ITEM_TYPE.SEED_WHEAT, 1 },
    { ITEM_TYPE.SEED_FLAX, 1 },
    { ITEM_TYPE.MUSHROOM_HEALSHROOM, 1 },
    { ITEM_TYPE.MUSHROOM_MUNCHSHROOM, 1 },
    { ITEM_TYPE.MUSHROOM_ZOOMSHROOM, 1 },
    { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, 1 },
    { ITEM_TYPE.BUILDING_FOUNDATION, 1 },
    { ITEM_TYPE.BUILDING_FOUNDATION_TRIANGLE, 1 },
    { ITEM_TYPE.BUILDING_WALL, 1 },
    { ITEM_TYPE.BUILDING_FLOOR, 1 },
    { ITEM_TYPE.BUILDING_RAMP, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, 1 },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, 1 },
    { ITEM_TYPE.TOOL_AXE_WOODEN, 1 },
    { ITEM_TYPE.TOOL_AXE_IRON, 1 },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, 1 },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, 1 },
    { ITEM_TYPE.BARK, 1 },
    { ITEM_TYPE.BOWL, 1 },
    
    { ITEM_TYPE.ARROW_STONE, 1 },
    { ITEM_TYPE.ARROW_IRON, 1 },
    { ITEM_TYPE.ARROW_MYTHRIL, 1 },
    { ITEM_TYPE.ARROW_ADAMANTITE, 1 },
  };
  
  public static readonly HashSet<ITEM_TYPE> ItemDeployables = new HashSet<ITEM_TYPE>()
  {
    ITEM_TYPE.SEED_WHEAT,
    ITEM_TYPE.SEED_FLAX,

    ITEM_TYPE.DEPLOYABLE_FARM_PLANTER,
    ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION,
    ITEM_TYPE.DEPLOYABLE_FURNACE,
    ITEM_TYPE.DEPLOYABLE_CAULDRON,
    ITEM_TYPE.DEPLOYABLE_CHEST,
    
    ITEM_TYPE.BUILDING_FOUNDATION,
    ITEM_TYPE.BUILDING_WALL,
    ITEM_TYPE.BUILDING_RAMP,
  };

  public static readonly Dictionary<ITEM_TYPE, TOOL_TYPE> ItemTools = new Dictionary<ITEM_TYPE, TOOL_TYPE>()
  {
    { ITEM_TYPE.NULL, TOOL_TYPE.NULL },
    { ITEM_TYPE.COIN, TOOL_TYPE.NULL },
    { ITEM_TYPE.ROCK, TOOL_TYPE.AXE },

    { ITEM_TYPE.WOOD, TOOL_TYPE.NULL },
    { ITEM_TYPE.WOOD_BIRCH, TOOL_TYPE.NULL },
    { ITEM_TYPE.WOOD_FIR, TOOL_TYPE.NULL },
    { ITEM_TYPE.WOOD_OAK, TOOL_TYPE.NULL },
    { ITEM_TYPE.WOOD_DARKOAK, TOOL_TYPE.NULL },
          
    { ITEM_TYPE.ORE_COAL, TOOL_TYPE.NULL },
    { ITEM_TYPE.ORE_IRON, TOOL_TYPE.NULL },
    { ITEM_TYPE.ORE_GOLD, TOOL_TYPE.NULL },
    { ITEM_TYPE.ORE_ADAMANTITE, TOOL_TYPE.NULL },
    { ITEM_TYPE.ORE_MYTHRIL, TOOL_TYPE.NULL },

    { ITEM_TYPE.TOOL_AXE_WOODEN, TOOL_TYPE.AXE },
    { ITEM_TYPE.TOOL_AXE_IRON, TOOL_TYPE.AXE },
    { ITEM_TYPE.TOOL_AXE_MYTHRIL, TOOL_TYPE.AXE },
    { ITEM_TYPE.TOOL_AXE_ADAMANTITE, TOOL_TYPE.AXE },
    
    { ITEM_TYPE.TOOL_PICKAXE_WOODEN, TOOL_TYPE.PICKAXE },
    { ITEM_TYPE.TOOL_PICKAXE_IRON, TOOL_TYPE.PICKAXE },
    { ITEM_TYPE.TOOL_PICKAXE_MYTHRIL, TOOL_TYPE.PICKAXE },
    { ITEM_TYPE.TOOL_PICKAXE_ADAMANTITE, TOOL_TYPE.PICKAXE },
    
    { ITEM_TYPE.ARROW_STONE, TOOL_TYPE.NULL },
    { ITEM_TYPE.ARROW_IRON, TOOL_TYPE.NULL },
    { ITEM_TYPE.ARROW_MYTHRIL, TOOL_TYPE.NULL },
    { ITEM_TYPE.ARROW_ADAMANTITE, TOOL_TYPE.NULL },
  };


  public static readonly HashSet<ITEM_TYPE> ItemConsumables = new HashSet<ITEM_TYPE>()
  {
    ITEM_TYPE.MUSHROOM_HEALSHROOM,
    ITEM_TYPE.MUSHROOM_MUNCHSHROOM,
    ITEM_TYPE.MUSHROOM_ZOOMSHROOM,
  };
}