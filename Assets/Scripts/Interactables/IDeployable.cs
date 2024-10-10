using System;
using System.Collections.Generic;
using Interactables;

public interface IDeployable
{
    public void Deploy(IPlayer player, ITEM_TYPE itemType);

    public static readonly Dictionary<Type, Dictionary<ITEM_TYPE, string>> ItemToDeployable =
        new()
        {
            {typeof(BuildingBlock), new Dictionary<ITEM_TYPE, string>()
            {
                // Buildings
                { ITEM_TYPE.BUILDING_FOUNDATION, "Prefabs/Buildings/building_foundation" },
                { ITEM_TYPE.BUILDING_WALL, "Prefabs/Buildings/building_wall" }, 
            }},
            {typeof(Crop), new Dictionary<ITEM_TYPE, string>()
            {
                // Seeds
                { ITEM_TYPE.SEED_WHEAT, "Prefabs/Crops/crop_wheat" },
                { ITEM_TYPE.SEED_FLAX, "Prefabs/Crops/crop_flax" },
            }},
            {typeof(CraftingStation), new Dictionary<ITEM_TYPE, string>()
            {
                // Seeds
                { ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION, "Prefabs/Deployables/deployable_crafting_station" },
            }},
            {typeof(FletchingTable), new Dictionary<ITEM_TYPE, string>()
            {
                { ITEM_TYPE.DEPLOYABLE_FLETCHING_TABLE, "Prefabs/Deployables/deployable_fletching_table" },
            }},
            {typeof(Anvil), new Dictionary<ITEM_TYPE, string>()
            {
                { ITEM_TYPE.DEPLOYABLE_ANVIL, "Prefabs/Deployables/deployable_anvil" },
            }},
            {typeof(Furnace), new Dictionary<ITEM_TYPE, string>()
            {
                { ITEM_TYPE.DEPLOYABLE_FURNACE, "Prefabs/Deployables/deployable_furnace" },
            }},
            {typeof(Cauldron), new Dictionary<ITEM_TYPE, string>()
            {
                { ITEM_TYPE.DEPLOYABLE_CAULDRON, "Prefabs/Deployables/deployable_cauldron" },
            }},
            {typeof(FarmPlot), new Dictionary<ITEM_TYPE, string>()
            {
                { ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, "Prefabs/Deployables/deployable_farm_planter" },
            }},
        };
}