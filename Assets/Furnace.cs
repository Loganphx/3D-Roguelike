using System;
using System.Collections.Generic;
using System.Linq;
using Interactables;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
public class Furnace : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes = new[]
    {
        new Recipe(ITEM_TYPE.INGOT_IRON, 1, ITEM_TYPE.ORE_IRON, 1),
        new Recipe(ITEM_TYPE.INGOT_GOLD, 1, ITEM_TYPE.ORE_GOLD, 1),
        new Recipe(ITEM_TYPE.INGOT_MYTHRIL, 1, ITEM_TYPE.ORE_MYTHRIL, 1),
        new Recipe(ITEM_TYPE.INGOT_ADAMANTITE, 1, ITEM_TYPE.ORE_ADAMANTITE, 1),
    };

    public const int SmeltTime = 10; // 10 seconds to smelt any ore

    private bool IsSmelting;
    private float StartSmeltTime;
    private float EndSmeltTime;
    [SerializeField, Min(0)] private float SmeltProgress;

    private float FuelEndTime;
    
    [SerializeField] private InventoryItem fuelItem;
    [SerializeField] private InventoryItem ingredientItem;
    [SerializeField] private InventoryItem productItem;

    private Outline _outline;
    private void Awake()
    {
        ResourceManager.Builds.Add(GetHashCode(), this);

        GetComponent<Damagable>().Initialize(new LootTable(new List<LootTableItem>()
        {
            new()
            {
                ItemType = ITEM_TYPE.DEPLOYABLE_FURNACE,
                maxAmount = 1,
                minAmount = 1,
                dropChance = 100,
                useStats = false
            }
        }), 100);
        
        _outline = transform.GetChild(0).GetComponent<Outline>();
        _outline.enabled = false;
        
        fuelItem = new InventoryItem()
        {
            ItemId = ITEM_TYPE.ORE_COAL,
            Amount = 64
        };

        ingredientItem = new InventoryItem()
        {
            ItemId = ITEM_TYPE.ORE_IRON,
            Amount = 64,
        };
        
        
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
    
    private void FixedUpdate()
    {
        if (IsSmelting)
        {
            if (FuelEndTime < Time.time)
            {
                if (fuelItem.ItemId != ITEM_TYPE.NULL)
                {
                    FuelEndTime = Time.time + ItemPool.ItemBurnTimes[fuelItem.ItemId];
                    if (fuelItem.Amount == 1)
                    {
                        fuelItem.ItemId = ITEM_TYPE.NULL;
                        fuelItem.Amount = 0;
                    }
                    else fuelItem.Amount--;
                }
                else CancelSmelt();
            }
        }
        if (fuelItem.ItemId != ITEM_TYPE.NULL)
        {
            if (ingredientItem.ItemId != ITEM_TYPE.NULL)
            {
                if (IsSmelting)
                {
                    SmeltProgress = Time.time - StartSmeltTime / SmeltTime;
                    return;
                }
                
                var recipe = Recipes.First(t => t.IngredientIds[0] == ingredientItem.ItemId);

                if (recipe.ProductItemId != ITEM_TYPE.NULL)
                {
                    if (productItem.ItemId == ITEM_TYPE.NULL || recipe.ProductItemId == productItem.ItemId)
                    {
                        StartSmelt();
                        Invoke(nameof(FinishSmelt), SmeltTime);
                        return;
                    }
                }
            }
        }
        
        if(IsSmelting) CancelSmelt();

        // Start Smelt Process
    }

    private void StartSmelt()
    {
        IsSmelting = true;
        SmeltProgress = 0;
        StartSmeltTime = Time.time;
        EndSmeltTime = Time.time + SmeltTime;
    }

    private void CancelSmelt()
    {
        IsSmelting = false;
        SmeltProgress = 0;
        StartSmeltTime = 0;
        EndSmeltTime = Time.time;
    }

    private void FinishSmelt()
    {
        if(!IsSmelting) return;
        
        var recipe = Recipes.First(t => t.IngredientIds[0] == ingredientItem.ItemId);
        
        if (productItem.ItemId == ITEM_TYPE.NULL)
        {
            Debug.Log($"Crafting {recipe.ProductItemId} x {recipe.ProductAmount}");
            productItem = new InventoryItem()
            {
                ItemId = recipe.ProductItemId,
                Amount = recipe.ProductAmount
            };
        }
        else if (productItem.ItemId == recipe.ProductItemId)
        {
            // DO A MAX STACK CHECK
            productItem.Amount++;
            Debug.Log(
                $"Crafting {recipe.ProductItemId} x {productItem.Amount}/{ItemPool.ItemMaxStacks[recipe.ProductItemId]}");
        }

        ingredientItem.Amount -= recipe.IngredientAmounts[0];
        if (ingredientItem.Amount <= 0) ingredientItem.ItemId = ITEM_TYPE.NULL;
        
        CancelSmelt();
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.FURNACE;
    }

    public void Interact(IPlayer player)
    {
    }

    public Transform Transform => transform;
    public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
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