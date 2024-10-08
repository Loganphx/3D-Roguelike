using System;
using System.Collections.Generic;
using System.Linq;
using Interactables;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
public class Cauldron : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes = {
        new Recipe(ITEM_TYPE.FOOD_MEAT_COOKED, 1, 
            ITEM_TYPE.MEAT_RAW, 1),
        new Recipe(ITEM_TYPE.FOOD_BEEF_STEW, 1, 
            ITEM_TYPE.MEAT_RAW, 1,
            ITEM_TYPE.MEAT_RAW, 1,
            ITEM_TYPE.BOWL, 1,
            ITEM_TYPE.WHEAT, 1),
        new Recipe(ITEM_TYPE.FOOD_POT_PIE, 1, 
            ITEM_TYPE.MEAT_RAW, 1,
            ITEM_TYPE.MEAT_RAW, 1,
            ITEM_TYPE.MEAT_RAW, 1,
            ITEM_TYPE.WHEAT, 1),
        new Recipe(ITEM_TYPE.FOOD_BREAD, 1, 
            ITEM_TYPE.WHEAT, 1,
            ITEM_TYPE.WHEAT, 1,
            ITEM_TYPE.WHEAT, 1),
    };

    public const int SmeltTime = 10; // 10 seconds to smelt any ore

    private bool IsSmelting;
    private float StartSmeltTime;
    private float EndSmeltTime;
    [SerializeField, Min(0)] private float SmeltProgress;

    private float FuelEndTime;
    
    [SerializeField] private InventoryItem fuelItem;
    [SerializeField] private InventoryItem ingredientItem1;
    [SerializeField] private InventoryItem ingredientItem2;
    [SerializeField] private InventoryItem ingredientItem3;
    [SerializeField] private InventoryItem ingredientItem4;
    [SerializeField] private InventoryItem productItem;

    private Outline _outline;
    private void Awake()
    {
        ResourceManager.Builds.Add(GetHashCode(), this);

        var damagable = GetComponent<Damagable>();
        damagable.Initialize(new LootTable(new List<LootTableItem>()
        {
            new()
            {
                ItemType = ITEM_TYPE.DEPLOYABLE_CAULDRON,
                maxAmount = 1,
                minAmount = 1,
                dropChance = 100,
                useStats = false
            }
        }), 100);
        damagable.OnDeath += OnDeath;
        
        _outline = transform.GetChild(0).GetComponent<Outline>();
        _outline.enabled = false;
        
        fuelItem = new InventoryItem()
        {
            ItemId = ITEM_TYPE.ORE_COAL,
            Amount = 64
        };

        ingredientItem1 = new InventoryItem()
        {
            ItemId = ITEM_TYPE.WHEAT,
            Amount = 64,
        };
        ingredientItem2 = new InventoryItem()
        {
            ItemId = ITEM_TYPE.WHEAT,
            Amount = 64,
        };
        ingredientItem3 = new InventoryItem()
        {
            ItemId = ITEM_TYPE.WHEAT,
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
            if (ingredientItem1.ItemId != ITEM_TYPE.NULL)
            {
                if (IsSmelting)
                {
                    SmeltProgress = Time.time - StartSmeltTime / SmeltTime;
                    return;
                }

                var recipe = Recipes.First(t => t.IngredientIds[0] == ingredientItem1.ItemId 
                                                && t.IngredientIds[1] == ingredientItem2.ItemId 
                                                && t.IngredientIds[2] == ingredientItem3.ItemId 
                                                && t.IngredientIds[3] == ingredientItem4.ItemId);

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
        
        var recipe = Recipes.First(t => t.IngredientIds[0] == ingredientItem1.ItemId 
                                        && t.IngredientIds[1] == ingredientItem2.ItemId 
                                        && t.IngredientIds[2] == ingredientItem3.ItemId 
                                        && t.IngredientIds[3] == ingredientItem4.ItemId);
        
        if (productItem.ItemId == ITEM_TYPE.NULL)
        {
            Debug.Log($"Cooking {recipe.ProductItemId} x {recipe.ProductAmount}");
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
                $"Cooking {recipe.ProductItemId} x {productItem.Amount}/{ItemPool.ItemMaxStacks[recipe.ProductItemId]}");
        }

        ingredientItem1.Amount -= recipe.IngredientAmounts[0];
        if (ingredientItem1.Amount <= 0) ingredientItem1.ItemId = ITEM_TYPE.NULL;
      
        ingredientItem2.Amount -= recipe.IngredientAmounts[1];
        if (ingredientItem2.Amount <= 0) ingredientItem2.ItemId = ITEM_TYPE.NULL;
        
        ingredientItem3.Amount -= recipe.IngredientAmounts[2];
        if (ingredientItem3.Amount <= 0) ingredientItem3.ItemId = ITEM_TYPE.NULL;
        
        ingredientItem4.Amount -= recipe.IngredientAmounts[3];
        if (ingredientItem4.Amount <= 0) ingredientItem4.ItemId = ITEM_TYPE.NULL;
        
        CancelSmelt();
    }

    private void OnDeath()
    {
        // DROP ITEMS
        var position = transform.position;
        var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
        var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
        var hitDirection = -transform.forward;
        
        if (ingredientItem1.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ingredientItem1.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero,
                Quaternion.identity, itemTemplate.transform);

            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(ingredientItem1.ItemId, ingredientItem1.Amount);

            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }
        
        if (ingredientItem2.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ingredientItem2.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero,
                Quaternion.identity, itemTemplate.transform);

            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(ingredientItem2.ItemId, ingredientItem2.Amount);

            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }
        
        if (ingredientItem3.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ingredientItem3.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero,
                Quaternion.identity, itemTemplate.transform);

            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(ingredientItem3.ItemId, ingredientItem3.Amount);

            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }

        if (ingredientItem4.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ingredientItem4.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero,
                Quaternion.identity, itemTemplate.transform);

            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(ingredientItem4.ItemId, ingredientItem4.Amount);

            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }
        
        if (fuelItem.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[fuelItem.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, itemTemplate.transform);
            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(fuelItem.ItemId, fuelItem.Amount);

            // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }

        if (productItem.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[productItem.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, itemTemplate.transform);
            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(productItem.ItemId, productItem.Amount);

            // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
            hitDirection.y = 1;
            var rigidBody = itemTemplate.GetComponent<Rigidbody>();
            rigidBody.linearDamping = 0.5f;
            rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
        }
    }
        
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.CAULDRON;
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