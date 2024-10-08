using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;

[RequireComponent(typeof(Damagable), typeof(HealthBarUI))]
internal class FletchingTable : MonoBehaviour, IInteractable, IHoverable, IDamagable
{
    public static Recipe[] Recipes =
    {
        new Recipe(ITEM_TYPE.ROPE, 1, ITEM_TYPE.FLAX, 3),
        //
        new Recipe(ITEM_TYPE.ARROW_STONE, 5, ITEM_TYPE.WOOD, 5, ITEM_TYPE.ROCK, 5),
        new Recipe(ITEM_TYPE.ARROW_IRON, 5, ITEM_TYPE.WOOD_BIRCH, 5, ITEM_TYPE.INGOT_IRON, 5),
        new Recipe(ITEM_TYPE.ARROW_MYTHRIL, 5, ITEM_TYPE.WOOD_OAK, 5, ITEM_TYPE.ARROW_MYTHRIL, 5),
        new Recipe(ITEM_TYPE.ARROW_ADAMANTITE, 5, ITEM_TYPE.WOOD_DARKOAK, 5, ITEM_TYPE.INGOT_ADAMANTITE, 5),
        //
        new Recipe(ITEM_TYPE.WEAPON_BOW_WOODEN, 1, ITEM_TYPE.ROPE, 1, ITEM_TYPE.WOOD_BIRCH, 10),
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