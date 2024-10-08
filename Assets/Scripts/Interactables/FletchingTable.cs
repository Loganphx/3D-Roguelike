using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
internal class FletchingTable : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes =
    {
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ROPE,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.WHEAT,
            IngredientAmount1 = 10
        },
        
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ARROW_STONE,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
            IngredientItemId2 = ITEM_TYPE.ROCK,
            IngredientAmount2 = 5
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ARROW_IRON,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
            IngredientItemId2 = ITEM_TYPE.INGOT_IRON,
            IngredientAmount2 = 5
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ARROW_MYTHRIL,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
            IngredientItemId2 = ITEM_TYPE.INGOT_MYTHRIL,
            IngredientAmount2 = 5
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ARROW_ADAMANTITE,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
            IngredientItemId2 = ITEM_TYPE.INGOT_ADAMANTITE,
            IngredientAmount2 = 5
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.ARROW_ADAMANTITE,
            ProductAmount = 5,
            
            IngredientItemId1 = ITEM_TYPE.WOOD,
            IngredientAmount1 = 5,
            IngredientItemId2 = ITEM_TYPE.INGOT_ADAMANTITE,
            IngredientAmount2 = 5
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.WEAPON_BOW_WOODEN,
            ProductAmount = 1,
            
            IngredientItemId1 = ITEM_TYPE.ROPE,
            IngredientAmount1 = 1,
            IngredientItemId2 = ITEM_TYPE.WOOD_BIRCH,
            IngredientAmount2 = 10,
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
                ItemType = ITEM_TYPE.DEPLOYABLE_FLETCHING_TABLE,
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
        return INTERACTABLE_TYPE.FLETCHING_TABLE;
    }

    public void Interact(IPlayer player)
    {
    }
    
    public Transform Transform => transform;
    public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        Debug.Log($"OnHit: {damager}, {hitDirection}, {hitPosition}, {toolType}, {damage}");
        GetComponent<Damagable>().OnHit(damager, hitDirection, hitPosition, toolType, damage);
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