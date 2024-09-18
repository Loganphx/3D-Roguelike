using System;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable, IDamagable
{
    public static Recipe[] Recipes = Array.Empty<Recipe>();

    private int health = 100;
    private void Awake()
    {
        var collider = transform.GetChild(0).Find("Interaction").GetComponent<Collider>();
        HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
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

    public void TakeDamage(IDamager player, Vector3 hitDirection, TOOL_TYPE toolType, int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death(hitDirection);
        }
    }

    private void Death(Vector3 hitDirection)
    {
        this.Death(hitDirection, ITEM_TYPE.DEPLOYABLE_CRAFTING_STATION);
    }
}