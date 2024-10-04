using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal struct PlayerUIInteractState
{
  public bool HasChanged;
  public int SelectedRecipeIndex;
}

internal class PlayerUIInteractComponent
{
  private Player _player;
  private readonly EventSystem _eventSystem;
  private PlayerUIInteractState _interactUIState;

  private Dictionary<INTERACTABLE_TYPE, GameObject> _uiPanels;

  private readonly List<ItemSlotUI> _itemSlots;

  public PlayerUIInteractComponent(Player player, EventSystem eventSystem, GameObject craftingStationUI, GameObject furnaceUI,
    GameObject cauldronUI, GameObject fletchingUI)
  {
    _player = player;
    _eventSystem = eventSystem;
    _uiPanels = new Dictionary<INTERACTABLE_TYPE, GameObject>()
    {
      { INTERACTABLE_TYPE.CRAFTING_STATION, craftingStationUI },
      { INTERACTABLE_TYPE.FURNACE, furnaceUI },
      { INTERACTABLE_TYPE.CAULDRON, cauldronUI },
      { INTERACTABLE_TYPE.FLETCHING_TABLE, fletchingUI },
    };
    _itemSlots = new List<ItemSlotUI>();
  }

  public void OnUpdate()
  {
    var eventData          = new PointerEventData(_eventSystem);
    eventData.position = Input.mousePosition;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    _eventSystem.RaycastAll(eventData, raycastResults);

    for (int i = 0; i < raycastResults.Count; i++)
    {
      var hit = raycastResults[i];

      // Debug.Log($"Show hover menu. {hit.gameObject.name}", hit.gameObject);

      foreach (var itemSlot in _itemSlots)
      {
        if (itemSlot.Image.transform.parent == hit.gameObject.transform.parent)
        {
          Debug.Log($"Found recipe for {itemSlot.type} x {itemSlot.AmountText.text}");
        }
      }
      if (_itemSlots.Exists(t => t.Image.gameObject == hit.gameObject))
      {
        Debug.Log("Found a recipe :)");
      }
      break;
    }
  }
  public void OnFixedUpdate(ref PlayerInteractState interactState, ref PlayerInventoryState inventoryState)
  {
    if (interactState.HasChanged == true)
    {
      bool isContainer = !(interactState.InteractableType != INTERACTABLE_TYPE.FLETCHING_TABLE &&
                           interactState.InteractableType != INTERACTABLE_TYPE.CRAFTING_STATION &&
                           interactState.InteractableType != INTERACTABLE_TYPE.FURNACE &&
                           interactState.InteractableType != INTERACTABLE_TYPE.CAULDRON);
      
      if (!isContainer)
      {
        foreach (var uiPanel in _uiPanels)
        {
          uiPanel.Value.SetActive(false);
        }
      }
      else
      {
        Debug.Log($"Opening {interactState.InteractableType} UI...");
        // Open Crafting Station UI

        foreach (var uiPanel in _uiPanels)
        {
          uiPanel.Value.SetActive(false);
        }

        var panel = _uiPanels[interactState.InteractableType];
        panel.SetActive(true);

        _itemSlots.Clear();

        var inventorySlots = panel.transform.Find("Panel").Find("Slots");
        for (int i = 21; i < inventorySlots.childCount; i++)
        {
          var child = inventorySlots.GetChild(i);
          var itemSlot = new ItemSlotUI()
          {
            Image = child.Find("Image").GetComponent<Image>(),
            AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
            type = ITEM_TYPE.NULL
          };
          _itemSlots.Add(itemSlot);
        }

        for (int i = 0; i < inventorySlots.childCount - 7; i++)
        {
          var child = inventorySlots.GetChild(i);
          var itemSlot = new ItemSlotUI()
          {
            Image = child.Find("Image").GetComponent<Image>(),
            AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
            type = ITEM_TYPE.NULL
          };
          _itemSlots.Add(itemSlot);
        }

        var recipes = interactState.InteractableType switch
        {
          INTERACTABLE_TYPE.CRAFTING_STATION => CraftingStation.Recipes,
          INTERACTABLE_TYPE.FLETCHING_TABLE => FletchingTable.Recipes,
          _ => Array.Empty<Recipe>()
        };
        
        if(interactState.InteractableType == INTERACTABLE_TYPE.CRAFTING_STATION) 
          CraftingUI.Instance.Init(Recipe_Select);
      }
    }

    if (interactState.InteractableType != INTERACTABLE_TYPE.NULL)
    {
      // var inventorySlot = _uiPanels[interactState.InteractableType].transform.Find("Panel").Find("Slots");
      if (_itemSlots.Count > 0)
      {
        for (int i = 0; i < inventoryState.Items.Length; i++)
        {
          ref var item = ref inventoryState.Items[i];

          var itemSlot = _itemSlots[i];

          if (item.ItemId == ITEM_TYPE.NULL)
          {
            itemSlot.Image.sprite = null;
            itemSlot.Image.enabled = false;
            itemSlot.AmountText.text = string.Empty;
          }
          else
          {
            itemSlot.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[item.ItemId]];
            itemSlot.Image.enabled = true;
            itemSlot.AmountText.text = item.Amount.ToString();
          }
        }
      }
    }
  }

  public void OnLateFixedUpdate(ref PlayerInteractState interactState)
  {
    if (interactState.InteractableType == INTERACTABLE_TYPE.CRAFTING_STATION ||
        interactState.InteractableType == INTERACTABLE_TYPE.FLETCHING_TABLE)
    {
      ref var uiState = ref _interactUIState;
      if (uiState.HasChanged && uiState.SelectedRecipeIndex != -1)
      {
        Recipe_Craft(ref interactState, uiState.SelectedRecipeIndex);
      }
    }
  }

  private void Recipe_Select(ITEM_TYPE itemType)
  {
    Debug.Log("Selecting Recipe...");
    ref var uiState = ref _interactUIState;
    uiState.SelectedRecipeIndex = -1;
    for (int i = 0; i < CraftingStation.Recipes.Length; i++)
    {
      if (CraftingStation.Recipes[i].ProductItemId == itemType)
      {
        uiState.SelectedRecipeIndex = i;
      }
    }    
    uiState.HasChanged = true;
  }

  private bool Recipe_CanCraft(ref PlayerInteractState interactState, int recipeIndex)
  {
    Debug.Log($"Recipe_CanCraft: {recipeIndex}");
    ref Recipe recipe = ref CraftingStation.Recipes[recipeIndex];
    if (interactState.InteractableType == INTERACTABLE_TYPE.FLETCHING_TABLE)
    {
      recipe = ref FletchingTable.Recipes[recipeIndex];
    }
    
    Debug.Log("Checking if can craft...");
    if (!_player.ContainsItem(recipe.IngredientItemId1, recipe.IngredientAmount1))
    {
      return false;
    }
    else
    {
      Debug.Log($"Player has {recipe.IngredientItemId1} x {recipe.IngredientAmount1}");
    }

    if (!_player.ContainsItem(recipe.IngredientItemId2, recipe.IngredientAmount2))
    {
      return false;
    }
    else Debug.Log($"Player has {recipe.IngredientItemId2} x {recipe.IngredientAmount2}");


    if (!_player.ContainsItem(recipe.IngredientItemId3, recipe.IngredientAmount3))
    {
      return false;
    }
    else Debug.Log($"Player has {recipe.IngredientItemId3} x {recipe.IngredientAmount3}");


    return true;
  }

  private void Recipe_Craft(ref PlayerInteractState interactState, int recipeIndex)
  {
    Debug.Log($"Recipe_Craft: {recipeIndex}");
    ref var uiState = ref _interactUIState;
      
    ref Recipe recipe = ref CraftingStation.Recipes[recipeIndex];
    if (interactState.InteractableType == INTERACTABLE_TYPE.FLETCHING_TABLE)
    {
      recipe = ref FletchingTable.Recipes[recipeIndex];
    }

    // ref var recipe = interactState.InteractableType == INTERACTABLE_TYPE.CRAFTING_STATION ? ref CraftingStation.Recipes[recipeIndex] : ref FletchingTable.Recipes[recipeIndex];
    Debug.Log("Crafting " + recipe.ProductItemId + " x " + recipe.ProductAmount);

    if (!Recipe_CanCraft(ref interactState, recipeIndex))
    {
      Debug.Log("Cannot Craft");
      uiState.SelectedRecipeIndex = -1;
      return;
    }

    if (recipe.IngredientItemId1 != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientItemId1, recipe.IngredientAmount1);
    if (recipe.IngredientItemId2 != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientItemId2, recipe.IngredientAmount2);
    if (recipe.IngredientItemId3 != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientItemId3, recipe.IngredientAmount3);

    _player.AddItem(recipe.ProductItemId, recipe.ProductAmount);

    uiState.SelectedRecipeIndex = -1;
    uiState.HasChanged = false;
  }
}