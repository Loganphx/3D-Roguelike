using System.Collections.Generic;
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
  private List<ItemSlotUI> _itemSlots;

  public PlayerUIInventoryComponent(Transform slotsParent)
  {
    _itemSlots = new List<ItemSlotUI>();
    for (int i = 0; i < slotsParent.childCount; i++)
    {
      var child = slotsParent.GetChild(i);
      var itemSlot = new ItemSlotUI()
      {
        Image = child.Find("Image").GetComponent<Image>(),
        AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
        type = ITEM_TYPE.NULL
      };
      _itemSlots.Add(itemSlot);
    }
  }
  public void OnFixedUpdate(ref PlayerInventoryState inventoryState)
  {
    if (!inventoryState.HasChanged) return;

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
      
      if(itemSlot.type == ITEM_TYPE.NULL)
        itemSlot.Image.enabled = true;
      
      itemSlot.Image.sprite = ItemPool.ItemSprites[item.ItemId];
      itemSlot.AmountText.text = "x" +item.Amount;
      itemSlot.type = item.ItemId;
      item.HasChanged = false;
    }
  }
}