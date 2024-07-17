using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class PlayerUIHotbarComponent
{
  private readonly GameObject _hotbarPanel;
  private readonly List<ItemSlotUI> _itemSlots;

  public PlayerUIHotbarComponent(GameObject hotbarPanel,
    Transform hotbarSlotsParent)
  {
    _itemSlots = new List<ItemSlotUI>();
    _hotbarPanel = hotbarPanel;
    
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
  }
  
  public void OnFixedUpdate(ref PlayerInventoryState inventoryState)
  {
    if (!inventoryState.HasChanged) return;

    // _goldText.text = inventoryState.GoldCount.ToString();
    
    bool hasChanged = false;
    
    Debug.Log(inventoryState.Items.Length);
    for (int i = 0; i < 7; i++)
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
        // item.HasChanged = true;
        continue;
      }

      if (itemSlot.type == ITEM_TYPE.NULL)
        itemSlot.Image.enabled = true;

      itemSlot.Image.sprite = SpritePool.Sprites[ItemPool.ItemSprites[item.ItemId]];
      itemSlot.AmountText.text = "x" + item.Amount;
      itemSlot.type = item.ItemId;
      // item.HasChanged = false;
    }
  }
}