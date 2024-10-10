using System;
using System.Collections.Generic;
using System.Linq;
using Interactables;
using UniRx;
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
    private float SmeltStartTime;
    private float SmeltEndTime;
    
    [SerializeField, Min(0)] private float SmeltProgress;

    private float FuelStartTime;
    private float FuelEndTime;
    
    [SerializeField] private InventoryItem fuelItem;
    [SerializeField] private InventoryItem ingredientItem;
    [SerializeField] private InventoryItem productItem;

    private Outline _outline;
    
    private BehaviorSubject<(float startTime, float endTime)> OnSmeltEndTimeChanged;
    private BehaviorSubject<(float startTime, float endTime)> OnFuelEndTimeChanged;
    private BehaviorSubject<InventoryItem> OnFuelItemChanged;
    private BehaviorSubject<InventoryItem> OnIngredientItemChanged;
    private BehaviorSubject<InventoryItem> OnProductItemChanged;
    
    private void Awake()
    {
        ResourceManager.Builds.Add(GetHashCode(), this);

        var damagable = GetComponent<Damagable>();
        damagable.Initialize(new LootTable(new List<LootTableItem>()
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
        damagable.OnDeath += OnDeath;

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

        OnSmeltEndTimeChanged = new BehaviorSubject<(float startTime, float endTime)>((0,0)); 
        OnFuelEndTimeChanged = new BehaviorSubject<(float startTime, float endtime)>((0,0));
        OnFuelItemChanged = new BehaviorSubject<InventoryItem>(fuelItem);
        OnIngredientItemChanged = new BehaviorSubject<InventoryItem>(ingredientItem);
        OnProductItemChanged = new BehaviorSubject<InventoryItem>(productItem);
    }

    private void OnDeath(Vector3 hitDirection)
    {
        // DROP ITEMS
        var position = transform.position;
        var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
        var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
        
        if (ingredientItem.ItemId != ITEM_TYPE.NULL)
        {
            var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
            var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ingredientItem.ItemId]];

            var item = Instantiate(itemPrefab, Vector3.zero,
                Quaternion.identity, itemTemplate.transform);

            item.transform.localPosition = Vector3.zero;

            itemTemplate.GetComponent<Item>().SetItemType(ingredientItem.ItemId, ingredientItem.Amount);

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
                    FuelStartTime = Time.time;
                    FuelEndTime = FuelStartTime + ItemPool.ItemBurnTimes[fuelItem.ItemId];
                    OnFuelEndTimeChanged.OnNext((FuelStartTime, FuelEndTime));
                    
                    if (fuelItem.Amount == 1)
                    {
                        fuelItem.ItemId = ITEM_TYPE.NULL;
                        fuelItem.Amount = 0;
                    }
                    else fuelItem.Amount--;
                    OnFuelItemChanged.OnNext(fuelItem);
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
                    SmeltProgress = Time.time - SmeltStartTime / SmeltTime;
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
        SmeltStartTime = Time.time;
        SmeltEndTime = SmeltStartTime + SmeltTime;
        OnSmeltEndTimeChanged.OnNext((SmeltStartTime, SmeltEndTime));
    }

    private void CancelSmelt()
    {
        IsSmelting = false;
        SmeltProgress = 0;
        SmeltStartTime = 0;
        SmeltEndTime = 0;
        OnSmeltEndTimeChanged.OnNext((SmeltStartTime, SmeltEndTime));
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
            OnProductItemChanged.OnNext(productItem);
        }
        else if (productItem.ItemId == recipe.ProductItemId)
        {
            // DO A MAX STACK CHECK
            productItem.Amount++;
            Debug.Log(
                $"Crafting {recipe.ProductItemId} x {productItem.Amount}/{ItemPool.ItemMaxStacks[recipe.ProductItemId]}");
            OnProductItemChanged.OnNext(productItem);
        }

        ingredientItem.Amount -= recipe.IngredientAmounts[0];
        if (ingredientItem.Amount <= 0) ingredientItem.ItemId = ITEM_TYPE.NULL;
        OnIngredientItemChanged.OnNext(ingredientItem);

        CancelSmelt();
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.FURNACE;
    }

    public void Interact(IPlayer player)
    {
        FurnaceUI.Instance.Init(OnFuelEndTimeChanged, OnSmeltEndTimeChanged, OnFuelItemChanged, OnIngredientItemChanged, OnProductItemChanged);
        
        OnFuelEndTimeChanged.OnNext((FuelStartTime, FuelEndTime));
        OnSmeltEndTimeChanged.OnNext((SmeltStartTime, SmeltEndTime));
       
        OnFuelItemChanged.OnNext(fuelItem);
        OnIngredientItemChanged.OnNext(ingredientItem);
        OnProductItemChanged.OnNext(productItem);
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