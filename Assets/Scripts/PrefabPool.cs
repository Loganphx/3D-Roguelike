using System.Collections.Generic;
using UnityEngine;

public static class PrefabPool
{
  public static Dictionary<string, GameObject> Prefabs;

  public static void LoadPrefabs()
  {
    Prefabs = new Dictionary<string, GameObject>()
    {
      {"Prefabs/Items/Item_Adamantite", Resources.Load<GameObject>("Prefabs/Items/Item_Adamantite")},
      {"Prefabs/Items/Item_Coin", Resources.Load<GameObject>("Prefabs/Items/Item_Coin")},
      {"Prefabs/Items/Item_Coal", Resources.Load<GameObject>("Prefabs/Items/Item_Coal")},
      {"Prefabs/Items/Item_Gold", Resources.Load<GameObject>("Prefabs/Items/Item_Gold")},
      {"Prefabs/Items/Item_Iron", Resources.Load<GameObject>("Prefabs/Items/Item_Iron")},
      {"Prefabs/Items/Item_Mythril", Resources.Load<GameObject>("Prefabs/Items/Item_Mythril")},
      {"Prefabs/Items/Item_Stone", Resources.Load<GameObject>("Prefabs/Items/Item_Stone")},
      {"Prefabs/Items/Item_Wood_Tree", Resources.Load<GameObject>("Prefabs/Items/Item_Wood_Tree")},
      {"Prefabs/Items/Item_Wood_Birch", Resources.Load<GameObject>("Prefabs/Items/Item_Wood_Birch")},
      {"Prefabs/Items/Item_Wood_Fir", Resources.Load<GameObject>("Prefabs/Items/Item_Wood_Fir")},
      {"Prefabs/Items/Item_Wood_Oak", Resources.Load<GameObject>("Prefabs/Items/Item_Wood_Oak")},
      {"Prefabs/Items/Item_Wood_DarkOak", Resources.Load<GameObject>("Prefabs/Items/Item_Wood_DarkOak")},
    };
  }
}

public static class ItemPool
{
  public static Dictionary<ITEM_TYPE, Sprite> ItemSprites;
  
  public static void LoadItems()
  {
    ItemSprites = new Dictionary<ITEM_TYPE, Sprite>()
    {
      { ITEM_TYPE.NULL, null},
      { ITEM_TYPE.ORE_STONE, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.ORE_COAL, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.ORE_IRON, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.ORE_GOLD, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.ORE_MYTHRIL, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.ORE_ADAMANTITE, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.WOOD, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.WOOD_BIRCH, Resources.Load<Sprite>("Sprites/birch")},
      { ITEM_TYPE.WOOD_FIR, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.WOOD_OAK, Resources.Load<Sprite>("Sprites/Untitled")},
      { ITEM_TYPE.WOOD_DARKOAK, Resources.Load<Sprite>("Sprites/Untitled")},
    };
  }
}