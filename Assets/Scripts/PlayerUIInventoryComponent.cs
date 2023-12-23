using System.Collections.Generic;
using ECS.Movement.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class ItemSlotUI
{
  public Image Image;
  public TMP_Text AmountText;
  public ITEM_TYPE type;
}

internal class PlayerUIInventoryComponent
{
  private readonly GameObject _inventoryPanel;
  private readonly List<ItemSlotUI> _itemSlots;

  private readonly TMP_Text _goldText;

  public PlayerUIInventoryComponent(GameObject inventoryPanel, Transform inventorySlotsParent,
    Transform hotbarSlotsParent, TMP_Text goldText)
  {
    _inventoryPanel = inventoryPanel;
    _goldText = goldText;

    _itemSlots = new List<ItemSlotUI>();
    for (int i = 0; i < hotbarSlotsParent.childCount; i++)
    {
      var child = hotbarSlotsParent.GetChild(i);
      var itemSlot = new ItemSlotUI()
      {
        Image = child.Find("Image").GetComponent<Image>(),
        AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
        type = ITEM_TYPE.NULL
      };
      _itemSlots.Add(itemSlot);
    }
    for (int i = 0; i < inventorySlotsParent.childCount; i++)
    {
      var child = inventorySlotsParent.GetChild(i);
      var itemSlot = new ItemSlotUI()
      {
        Image = child.Find("Image").GetComponent<Image>(),
        AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
        type = ITEM_TYPE.NULL
      };
      _itemSlots.Add(itemSlot);
    }
  }

  public void OnUpdate(ref GameplayInput input)
  {
    if (!input.ToggleInventory) return;

    _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
  }

  public void OnFixedUpdate(ref PlayerInventoryState inventoryState)
  {
    if (!inventoryState.HasChanged) return;

    _goldText.text = inventoryState.GoldCount.ToString();
    
    bool hasChanged = false;
    
    for (int i = 0; i < inventoryState.Items.Length; i++)
    {
      ref var item = ref inventoryState.Items[i];

      if (!item.HasChanged) continue;

      var itemSlot = _itemSlots[i];

      if (item.Amount == 0)
      {
        itemSlot.Image.sprite = null;
        itemSlot.Image.enabled = false;
        itemSlot.AmountText.text = "";
        itemSlot.type = ITEM_TYPE.NULL;
        item.HasChanged = false;
        continue;
      }

      if (itemSlot.type == ITEM_TYPE.NULL)
        itemSlot.Image.enabled = true;

      itemSlot.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[item.ItemId]];
      itemSlot.AmountText.text = "x" + item.Amount;
      itemSlot.type = item.ItemId;
      item.HasChanged = false;
    }
  }
}