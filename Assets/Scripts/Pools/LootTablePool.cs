using System.Collections.Generic;
using Interactables.Trees;

public static class LootTablePool
{
    public static readonly Dictionary<TREE_TYPE, LootTable> TreeLootTables = new()
    {
        {
            TREE_TYPE.Tree, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.WOOD,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            TREE_TYPE.Birch, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.WOOD_BIRCH,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            TREE_TYPE.Fir, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.WOOD_FIR,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            TREE_TYPE.Oak, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.WOOD_OAK,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            TREE_TYPE.DarkOak, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.WOOD_DARKOAK,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
    };
    public static readonly Dictionary<NODE_TYPE, LootTable> NodeLootTables = new()
    {
        {
            NODE_TYPE.Stone, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            NODE_TYPE.Iron, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ORE_IRON,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                },
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 50,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            NODE_TYPE.Coal, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ORE_COAL,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                },
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 50,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            NODE_TYPE.Gold, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ORE_GOLD,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                },
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 50,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            NODE_TYPE.Mythril, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ORE_MYTHRIL,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                },
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 50,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
        {
            NODE_TYPE.Adamantite, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.ORE_ADAMANTITE,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                },
                new()
                {
                    ItemType = ITEM_TYPE.ROCK,
                    dropChance = 50,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
    };
    public static readonly Dictionary<MOB_TYPE, LootTable> MobLootTables = new()
    {
        {
            MOB_TYPE.SHEEP, new LootTable(new List<LootTableItem>()
            {
                new()
                {
                    ItemType = ITEM_TYPE.COIN,
                    dropChance = 100,
                    minAmount = 1,
                    maxAmount = 5,
                }
            })
        },
    };
}