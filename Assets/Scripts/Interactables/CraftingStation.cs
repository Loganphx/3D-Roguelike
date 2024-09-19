using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable, IDamagable
{
    public static Recipe[] Recipes = Array.Empty<Recipe>();

    private int health = 100;

    private void Awake()
    {
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
    }

    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.CRAFTING_STATION;
    }

    public void Interact(IPlayer player)
    {
        throw new NotImplementedException();
    }

    public Transform Transform => transform;

    public void OnHit(IDamager player, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death(hitDirection);
            GetComponent<Damagable>().OnHit(player, hitDirection, hitPosition, toolType, damage);
        }
    }

    private void Death(Vector3 hitDirection)
    {
    }
}