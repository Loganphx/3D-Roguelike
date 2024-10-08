using System.Runtime.InteropServices;

public struct Recipe
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ITEM_TYPE[] IngredientIds;
    
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] IngredientAmounts;
  
    public ITEM_TYPE ProductItemId;
    public byte ProductAmount;
    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientIds = new ITEM_TYPE[4]
        {
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
        };
        IngredientAmounts = new byte[4]
        {
            0,
            0,
            0,
            0
        };
    }
    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientIds = new ITEM_TYPE[4]
        {
            ingredientItemId1,
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
        };
        IngredientAmounts = new byte[4]
        {
            ingredientQuantity1,
            0,
            0,
            0
        };
    }

    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1,
        ITEM_TYPE ingredientItemId2, byte ingredientQuantity2)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientIds = new ITEM_TYPE[4]
        {
            ingredientItemId1,
            ingredientItemId2,
            ITEM_TYPE.NULL,
            ITEM_TYPE.NULL,
        };
        IngredientAmounts = new byte[4]
        {
            ingredientQuantity1,
            ingredientQuantity2,
            0,
            0
        };
    }

    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1, 
        ITEM_TYPE ingredientItemId2, byte ingredientQuantity2,
        ITEM_TYPE ingredientItemId3, byte ingredientQuantity3)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientIds = new ITEM_TYPE[4]
        {
            ingredientItemId1,
            ingredientItemId2,
            ingredientItemId3,
            ITEM_TYPE.NULL,
        };
        IngredientAmounts = new byte[4]
        {
            ingredientQuantity1,
            ingredientQuantity2,
            ingredientQuantity3,
            0
        };
    }
    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1, 
        ITEM_TYPE ingredientItemId2, byte ingredientQuantity2,
        ITEM_TYPE ingredientItemId3, byte ingredientQuantity3,
        ITEM_TYPE ingredientItemId4, byte ingredientQuantity4)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;
        
        IngredientIds = new ITEM_TYPE[4]
        {
            ingredientItemId1,
            ingredientItemId2,
            ingredientItemId3,
            ingredientItemId4,
        };
        IngredientAmounts = new byte[4]
        {
            ingredientQuantity1,
            ingredientQuantity2,
            ingredientQuantity3,
            ingredientQuantity4
        };
    }
  
}