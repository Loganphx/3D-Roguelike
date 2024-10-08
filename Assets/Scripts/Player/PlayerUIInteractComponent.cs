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
  public INTERACTABLE_TYPE InteractableType;
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
    GameObject cauldronUI, GameObject fletchingUI, GameObject anvilUI)
  {
    _player = player;
    _eventSystem = eventSystem;
    _uiPanels = new Dictionary<INTERACTABLE_TYPE, GameObject>()
    {
      { INTERACTABLE_TYPE.CRAFTING_STATION, craftingStationUI },
      { INTERACTABLE_TYPE.FURNACE, furnaceUI },
      { INTERACTABLE_TYPE.CAULDRON, cauldronUI },
      { INTERACTABLE_TYPE.FLETCHING_TABLE, fletchingUI },
      { INTERACTABLE_TYPE.ANVIL, anvilUI },
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
                           interactState.InteractableType != INTERACTABLE_TYPE.ANVIL &&
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

        ref var interactUIState = ref _interactUIState;
        if (interactState.InteractableType == INTERACTABLE_TYPE.CRAFTING_STATION)
        {
          interactUIState.InteractableType = INTERACTABLE_TYPE.CRAFTING_STATION;
          CraftingUI.Instance.Init(Recipe_Select);
        }
        else if (interactState.InteractableType == INTERACTABLE_TYPE.FLETCHING_TABLE)
        {
          interactUIState.InteractableType = INTERACTABLE_TYPE.FLETCHING_TABLE;
          FletchingUI.Instance.Init(Recipe_Select);
        }
        else if (interactState.InteractableType == INTERACTABLE_TYPE.ANVIL)
        {
          interactUIState.InteractableType = INTERACTABLE_TYPE.ANVIL;
          AnvilUI.Instance.Init(Recipe_Select);
        }
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
        interactState.InteractableType == INTERACTABLE_TYPE.FLETCHING_TABLE ||
        interactState.InteractableType == INTERACTABLE_TYPE.ANVIL)
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
    Recipe[] recipes;
    switch (uiState.InteractableType)
    {
      case INTERACTABLE_TYPE.CRAFTING_STATION:
        recipes = CraftingStation.Recipes;
        break;
      case INTERACTABLE_TYPE.FLETCHING_TABLE:
        recipes = FletchingTable.Recipes;
        break;
      case INTERACTABLE_TYPE.ANVIL:
        recipes = Anvil.Recipes;
        break;
      default:
        throw new ArgumentOutOfRangeException(uiState.InteractableType.ToString() + " is yet not supported");
    }
    
    for (int i = 0; i < recipes.Length; i++)
    {
      if (recipes[i].ProductItemId == itemType)
      {
        uiState.SelectedRecipeIndex = i;
      }
    }    
    uiState.HasChanged = true;
  }

  private bool Recipe_CanCraft(ref PlayerInteractState interactState, int recipeIndex)
  {
    Debug.Log($"Recipe_CanCraft: {recipeIndex}");
    ref Recipe recipe = ref CraftingStation.Recipes[0];
    switch (interactState.InteractableType)
    {
      case INTERACTABLE_TYPE.CRAFTING_STATION:
        recipe = ref CraftingStation.Recipes[recipeIndex];
        break;
      case INTERACTABLE_TYPE.FLETCHING_TABLE:
        recipe = ref FletchingTable.Recipes[recipeIndex];
        break;
      case INTERACTABLE_TYPE.ANVIL:
        recipe = ref Anvil.Recipes[recipeIndex];
        break;
      default:
        throw new ArgumentOutOfRangeException(interactState.InteractableType.ToString() + " is yet not supported");
    }

    Debug.Log("Checking if can craft...");
    if (!_player.ContainsItem(recipe.IngredientIds[0], recipe.IngredientAmounts[0]))
    {
      return false;
    }
    else
    {
      Debug.Log($"Player has {recipe.IngredientIds[0]} x {recipe.IngredientAmounts[0]}");
    }

    if (!_player.ContainsItem(recipe.IngredientIds[1], recipe.IngredientAmounts[1]))
    {
      return false;
    }
    else Debug.Log($"Player has {recipe.IngredientIds[1]} x {recipe.IngredientAmounts[1]}");


    if (!_player.ContainsItem(recipe.IngredientIds[2], recipe.IngredientAmounts[2]))
    {
      return false;
    }
    else Debug.Log($"Player has {recipe.IngredientIds[2]} x {recipe.IngredientAmounts[2]}");

    if (!_player.ContainsItem(recipe.IngredientIds[3], recipe.IngredientAmounts[3]))
    {
      return false;
    }
    else Debug.Log($"Player has {recipe.IngredientIds[3]} x {recipe.IngredientAmounts[3]}");

    return true;
  }

  private void Recipe_Craft(ref PlayerInteractState interactState, int recipeIndex)
  {
    Debug.Log($"Recipe_Craft: {recipeIndex}");
    ref var uiState = ref _interactUIState;
      
    ref Recipe recipe = ref CraftingStation.Recipes[0];
    switch (interactState.InteractableType)
    {
      case INTERACTABLE_TYPE.CRAFTING_STATION:
        recipe = ref CraftingStation.Recipes[recipeIndex];
        break;
      case INTERACTABLE_TYPE.FLETCHING_TABLE:
        recipe = ref FletchingTable.Recipes[recipeIndex];
        break;
      case INTERACTABLE_TYPE.ANVIL:
        recipe = ref Anvil.Recipes[recipeIndex];
        break;
      default:
        throw new ArgumentOutOfRangeException(interactState.InteractableType.ToString() + " is yet not supported");
    }

    // ref var recipe = interactState.InteractableType == INTERACTABLE_TYPE.CRAFTING_STATION ? ref CraftingStation.Recipes[recipeIndex] : ref FletchingTable.Recipes[recipeIndex];
    Debug.Log("Crafting " + recipe.ProductItemId + " x " + recipe.ProductAmount);

    if (!Recipe_CanCraft(ref interactState, recipeIndex))
    {
      Debug.Log("Cannot Craft");
      uiState.SelectedRecipeIndex = -1;
      return;
    }

    if (recipe.IngredientIds[0] != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientIds[0], recipe.IngredientAmounts[1]);
    if (recipe.IngredientIds[1] != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientIds[1], recipe.IngredientAmounts[1]);
    if (recipe.IngredientIds[2] != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientIds[2], recipe.IngredientAmounts[2]);
    if (recipe.IngredientIds[3] != ITEM_TYPE.NULL)
      _player.RemoveItem(recipe.IngredientIds[3], recipe.IngredientAmounts[3]);

    _player.AddItem(recipe.ProductItemId, recipe.ProductAmount);

    uiState.SelectedRecipeIndex = -1;
    uiState.HasChanged = false;
  }
}