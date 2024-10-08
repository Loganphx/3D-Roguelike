using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
public class CraftingStation : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes = {
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.BARK,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.BOWL,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.TOOL_AXE_WOODEN,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 10,
            
            IngredientItemId2 = ITEM_TYPE.BARK,
            IngredientAmount2 = 10,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.TOOL_PICKAXE_WOODEN,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 10,
            
            IngredientItemId2 = ITEM_TYPE.BARK,
            IngredientAmount2 = 10,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.WEAPON_SWORD_WOODEN,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 10,
            
            IngredientItemId2 = ITEM_TYPE.BARK,
            IngredientAmount2 = 10,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_FURNACE,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.ROCK,
            IngredientAmount1 = 25,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_ANVIL,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.INGOT_IRON,
            IngredientAmount1 = 5,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_CAULDRON,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.ROCK,
            IngredientAmount1 = 10,
            
            IngredientItemId2 = ITEM_TYPE.WOOD,
            IngredientAmount2 = 10,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_FLETCHING_TABLE,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 25,
        },
        
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_CHEST,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 25,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.DEPLOYABLE_FARM_PLANTER,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 25,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.BUILDING_FOUNDATION,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 20,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.BUILDING_FOUNDATION_TRIANGLE,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 10,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.BUILDING_WALL,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 10,
        },
        
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