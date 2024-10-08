using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
public class CraftingStation : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static readonly Recipe[] Recipes = {
        new Recipe(ITEM_TYPE.BARK, 5, ITEM_TYPE.WOOD, 5),
        new Recipe(ITEM_TYPE.BOWL, 1, ITEM_TYPE.WOOD, 5),
        new Recipe(ITEM_TYPE.TOOL_AXE_WOODEN, 1, ITEM_TYPE.WOOD, 10, ITEM_TYPE.BARK, 10),
        new Recipe(ITEM_TYPE.TOOL_PICKAXE_WOODEN, 1, ITEM_TYPE.WOOD, 10, ITEM_TYPE.BARK, 10),
        new Recipe(ITEM_TYPE.WEAPON_SWORD_WOODEN, 1, ITEM_TYPE.WOOD, 10, ITEM_TYPE.BARK, 10),
        new Recipe(ITEM_TYPE.DEPLOYABLE_FURNACE, 1, ITEM_TYPE.ROCK, 25),
        new Recipe(ITEM_TYPE.DEPLOYABLE_ANVIL, 1, ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(ITEM_TYPE.DEPLOYABLE_CAULDRON, 1, ITEM_TYPE.ROCK, 10, ITEM_TYPE.WOOD, 10),
        new Recipe(ITEM_TYPE.DEPLOYABLE_FLETCHING_TABLE, 1, ITEM_TYPE.WOOD, 25),
        new Recipe(ITEM_TYPE.DEPLOYABLE_CHEST, 1, ITEM_TYPE.WOOD, 25),
        new Recipe(ITEM_TYPE.DEPLOYABLE_FARM_PLANTER, 1, ITEM_TYPE.WOOD, 25),
        new Recipe(ITEM_TYPE.BUILDING_FOUNDATION, 1, ITEM_TYPE.WOOD, 20),
        new Recipe(ITEM_TYPE.BUILDING_FOUNDATION_TRIANGLE, 1, ITEM_TYPE.WOOD, 10),
        new Recipe(ITEM_TYPE.BUILDING_WALL, 1, ITEM_TYPE.WOOD, 10),
    };

    private Outline _outline;
    private void Awake()
    {
        ResourceManager.Builds.Add(GetHashCode(), this);
        
        GetComponent<Damagable>().Initialize(new LootTable(new List<LootTableItem>()
        {
            new()
            {
                ItemType = ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION,
                maxAmount = 1,
                minAmount = 1,
                dropChance = 100,
                useStats = false
            }
        }), 100);
        
        _outline = transform.GetChild(0).GetComponent<Outline>();
        _outline.enabled = false;
    }
    
    public void OnEnable()
    {
        ResourceManager.Builds.TryAdd(GetHashCode(), this);
    }

    private void OnDestroy()
    {
        ResourceManager.Builds.Remove(GetHashCode());
    }

    private void OnDisable()
    {
        ResourceManager.Builds.Remove(GetHashCode());
    }
    
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.CRAFTING_STATION;
    }

    public void Interact(IPlayer player)
    {
        //
    }

    public Transform Transform => transform;

    public void OnHit(IDamager player, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        GetComponent<Damagable>().OnHit(player, hitDirection, hitPosition, toolType, damage);
    }

    public void OnHoverEnter(IPlayer player)
    {
        _outline.enabled = true;
    }

    public void OnHoverExit(IPlayer player)
    {
        _outline.enabled = false;
    }
}