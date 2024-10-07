public struct Recipe
{
    public ITEM_TYPE IngredientItemId1;
    public byte IngredientAmount1;
  
    public ITEM_TYPE IngredientItemId2;
    public byte IngredientAmount2;
  
    public ITEM_TYPE IngredientItemId3;
    public byte IngredientAmount3;
  
    public ITEM_TYPE ProductItemId;
    public byte ProductAmount;
    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientItemId1 = ITEM_TYPE.NULL;
        IngredientAmount1 = 0;
        IngredientItemId2 = ITEM_TYPE.NULL;
        IngredientAmount2 = 0;
        IngredientItemId3 = ITEM_TYPE.NULL;
        IngredientAmount3 = 0;
    }
    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientItemId1 = ingredientItemId1;
        IngredientAmount1 = ingredientQuantity1;
        
        IngredientItemId2 = ITEM_TYPE.NULL;
        IngredientAmount2 = 0;
        IngredientItemId3 = ITEM_TYPE.NULL;
        IngredientAmount3 = 0;
    }

    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1,
        ITEM_TYPE ingredientItemId2, byte ingredientQuantity2)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientItemId1 = ingredientItemId1;
        IngredientAmount1 = ingredientQuantity1;
        
        IngredientItemId2 = ingredientItemId2;
        IngredientAmount2 = ingredientQuantity2;
        
        IngredientItemId3 = ITEM_TYPE.NULL;
        IngredientAmount3 = 0;
    }

    
    public Recipe(ITEM_TYPE productItemId, byte productQuantity, 
        ITEM_TYPE ingredientItemId1, byte ingredientQuantity1, 
        ITEM_TYPE ingredientItemId2, byte ingredientQuantity2,
        ITEM_TYPE ingredientItemId3, byte ingredientQuantity3)
    {
        ProductItemId = productItemId;
        ProductAmount = productQuantity;

        IngredientItemId1 = ingredientItemId1;
        IngredientAmount1 = ingredientQuantity1;
        
        IngredientItemId2 = ingredientItemId2;
        IngredientAmount2 = ingredientQuantity2;
        
        IngredientItemId3 = ingredientItemId3;
        IngredientAmount3 = ingredientQuantity3;
    }
  
}