using System;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    public static Recipe[] Recipes = Array.Empty<Recipe>();
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.CRAFTING_STATION;
    }

    public void Interact(IPlayer player)
    {
        throw new NotImplementedException();
    }
}