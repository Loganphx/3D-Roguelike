
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class PowerupSlotUI
{
  public Image Image;
  public TMP_Text AmountText;
  public POWERUP_TYPE type;
}

internal class PlayerUIPowerupComponent
{
  private readonly List<PowerupSlotUI> _powerupSlots;

  public PlayerUIPowerupComponent(Transform powerupSlotsParent)
  {
    _powerupSlots = new List<PowerupSlotUI>();

    for (int i = 0; i < powerupSlotsParent.childCount; i++)
    {
      var child = powerupSlotsParent.GetChild(i);
      var itemSlot = new PowerupSlotUI()
      {
        Image = child.Find("Image").GetComponent<Image>(),
        AmountText = child.Find("Amount").GetComponent<TMP_Text>(),
        type = POWERUP_TYPE.NULL
      };
      _powerupSlots.Add(itemSlot);
    }
  }

  public bool OnFixedUpdate(ref PlayerPowerupState powerupState)
  {
    if (!powerupState.HasChanged) return false;

    bool hasChanged = false;
    
    for (int i = 0; i < powerupState.Powerups.Length; i++)
    {
      ref var item = ref powerupState.Powerups[i];

      if (!item.HasChanged) continue;

      var itemSlot = _powerupSlots[i];

      if (item.Amount == 0)
      {
        itemSlot.Image.sprite = null;
        itemSlot.Image.enabled = false;
        itemSlot.AmountText.text = "";
        itemSlot.type = POWERUP_TYPE.NULL;
        item.HasChanged = false;
        continue;
      }

      if (itemSlot.type == POWERUP_TYPE.NULL)
        itemSlot.Image.enabled = true;

      itemSlot.Image.sprite = SpritePool.Sprites[PowerupPool.PowerupSprites[item.PowerupId]];
      itemSlot.AmountText.text = "x" + item.Amount;
      itemSlot.type = item.PowerupId;
      item.HasChanged = false;
      hasChanged = true;
    }

    return hasChanged;
  }
}