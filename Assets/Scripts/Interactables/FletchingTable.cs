using System;
using UnityEngine;

internal class FletchingTable : MonoBehaviour, IInteractable
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
    public INTERACTABLE_TYPE GetInteractableType()
    {
        return INTERACTABLE_TYPE.FLETCHING_TABLE;
    }

    public void Interact(IPlayer player)
    {
    }
}