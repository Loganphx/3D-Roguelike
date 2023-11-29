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
    
    { ITEM_TYPE.SEED_WHEAT, "Sprites/Items/seed_wheat" },
    { ITEM_TYPE.SEED_FLAX, "Sprites/Items/seed_flax" },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, "Sprites/Items/building_foundation" },
    { ITEM_TYPE.BUILDING_WALL, "Sprites/Items/building_wall" },
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
    
    { ITEM_TYPE.SEED_WHEAT, "Prefabs/Models/model_seed_wheat" },
    { ITEM_TYPE.SEED_FLAX, "Prefabs/Models/model_seed_flax" },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Models/model_building_foundation" },
    { ITEM_TYPE.BUILDING_WALL, "Prefabs/Models/model_building_wall" },
  };

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

    { ITEM_TYPE.SEED_WHEAT, 0 },
    { ITEM_TYPE.SEED_FLAX, 0 },
    
    { ITEM_TYPE.BUILDING_FOUNDATION, 0 },
    { ITEM_TYPE.BUILDING_WALL, 0 },
  };
  
  public static readonly HashSet<ITEM_TYPE> ItemDeployables = new HashSet<ITEM_TYPE>()
  {
    ITEM_TYPE.SEED_WHEAT,
    ITEM_TYPE.SEED_FLAX,
    
    ITEM_TYPE.BUILDING_FOUNDATION,
    ITEM_TYPE.BUILDING_WALL,
  };
}