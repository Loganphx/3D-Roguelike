using UnityEngine;
using UnityEngine.UI;

internal class PlayerUIEquipmentComponent
{
    private readonly Image[] _equipmentSlots;

    public PlayerUIEquipmentComponent(GameObject equipmentSlotsHolder)
    {
        _equipmentSlots = new Image[4];
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            _equipmentSlots[i] = equipmentSlotsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }
    
    public void OnFixedUpdate(ref PlayerEquipmentState equipmentState)
    {
        if (!equipmentState.HasChanged) return;
        
        for (int i = 0; i < equipmentState.Equipment.Length; i++)
        {
            var itemId = equipmentState.Equipment[i];
            
            var itemSlot = _equipmentSlots[i];

            if (itemId == ITEM_TYPE.NULL)
            {
                itemSlot.sprite = null;
                itemSlot.enabled = false;
                continue;
            }
            
            itemSlot.sprite = SpritePool.Sprites[ItemPool.ItemSprites[itemId]];
            itemSlot.enabled = true;
            // item.HasChanged = false;
        }
    }
}