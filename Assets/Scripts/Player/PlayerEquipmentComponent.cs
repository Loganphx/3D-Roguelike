using System;


public enum ARMOR_TYPE
{
    HELMET = 0,
    CHESTPLATE = 1,
    LEGGINGS = 2,
    BOOTS = 3,
}
internal struct PlayerEquipmentState : IState
{
    public ITEM_TYPE[] Equipment;

    public bool HasChanged;
}

internal class PlayerEquipmentComponent : IComponent<PlayerEquipmentState>
{
    public PlayerEquipmentState _state;

    public PlayerEquipmentComponent(IPlayer player)
    {
        _state = new PlayerEquipmentState()
        {
            Equipment = new ITEM_TYPE[]
            {
                ITEM_TYPE.NULL, // HELMET
                ITEM_TYPE.NULL, // CHESTPLATE
                ITEM_TYPE.NULL, // LEGGINGS
                ITEM_TYPE.NULL // BOOTS
            }
        };
    }

    public bool EquipItem(int slotIndex, ITEM_TYPE itemToEquip)
    {
        ref var state = ref _state;
        if(slotIndex >= state.Equipment.Length) return false;

        var armorType = (ARMOR_TYPE)slotIndex;
        if (ItemPool.ItemEquipment.TryGetValue(itemToEquip, out var value))
        {
            if (value == armorType)
            {
                state.Equipment[slotIndex] = itemToEquip;
                state.HasChanged = true;
                return true;
            }
        }

        return false;
    }

    public bool OnFixedUpdate(ref PlayerHealthState healthState)
    {
        ref var state = ref _state;

        if (state.HasChanged)
        {
            state.HasChanged = false;
            
            var totalArmor = 0;
            foreach (var equipment in state.Equipment)
            {
                if (equipment != ITEM_TYPE.NULL)
                {
                    totalArmor += ItemPool.ItemArmors[equipment];
                }
            }

            healthState.Armor = totalArmor;
            healthState.HasChanged = true;
            return true;
        }
        return false;
    }
    public PlayerEquipmentState State => _state;
}