using System;
using UnityEngine;

internal class FletchingTable : MonoBehaviour, IInteractable
{
    public static Recipe[] Recipes = Array.Empty<Recipe>();
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.FLETCHING_TABLE;
    }

    public void Interact(IPlayer player)
    {
        throw new NotImplementedException();
    }
}