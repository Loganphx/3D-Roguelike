using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Furnace : MonoBehaviour
{
    public static Recipe[] Recipes = new[]
    {
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.INGOT_IRON,
            ProductAmount = 1,

            IngredientItemId1 = ITEM_TYPE.ORE_IRON,
            IngredientAmount1 = 1,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.INGOT_GOLD,
            ProductAmount = 1,

            IngredientItemId1 = ITEM_TYPE.ORE_GOLD,
            IngredientAmount1 = 1,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.INGOT_MYTHRIL,
            ProductAmount = 1,

            IngredientItemId1 = ITEM_TYPE.ORE_MYTHRIL,
            IngredientAmount1 = 1,
        },
        new Recipe()
        {
            ProductItemId = ITEM_TYPE.INGOT_ADAMANTITE,
            ProductAmount = 1,

            IngredientItemId1 = ITEM_TYPE.ORE_ADAMANTITE,
            IngredientAmount1 = 1,
        },
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

    private void Awake()
    {
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
                
                var recipe = Recipes.First(t => t.IngredientItemId1 == ingredientItem.ItemId);

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
        
        var recipe = Recipes.First(t => t.IngredientItemId1 == ingredientItem.ItemId);
        
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

        ingredientItem.Amount -= recipe.IngredientAmount1;
        if (ingredientItem.Amount <= 0) ingredientItem.ItemId = ITEM_TYPE.NULL;
        
        CancelSmelt();
    }
}