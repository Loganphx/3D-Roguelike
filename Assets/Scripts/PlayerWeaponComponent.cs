using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerWeaponState : IState
{
  public ITEM_TYPE EquippedWeapon;
  public int EquippedSlot;
  
  public bool HasChanged;
}
internal class PlayerWeaponComponent : IComponent<PlayerWeaponState>
{
  private readonly Transform _armTransform;
  private GameObject _equippedWeapon;
  
  public PlayerWeaponState _state;
  
  // ReSharper disable once UnusedParameter.Local
  public PlayerWeaponComponent(IPlayer player, Transform armTransform)
  {
    _armTransform = armTransform;
    ref var state = ref _state ;
    
    state.EquippedSlot = -1;
    state.EquippedWeapon = ITEM_TYPE.NULL;
    state.HasChanged = true;
  }
  
  public bool OnFixedUpdate(ref GameplayInput input, ref PlayerInventoryState inventory)
  {
    ref var state = ref _state;
    state.HasChanged = false;
    
    if (input.Action1)
    {
      EquipWeapon(0, ref inventory);
      return state.HasChanged;
    }
    
    if (input.Action2)
    {
      EquipWeapon(1, ref inventory);
      return state.HasChanged;
    }
    if (input.Action3)
    {
      EquipWeapon(2, ref inventory);
      return state.HasChanged;
    }
    
    if (input.Action4)
    {
      EquipWeapon(3, ref inventory);
      return state.HasChanged;
    }
    if (input.Action5)
    {
      EquipWeapon(4, ref inventory);
      return state.HasChanged;
    }
    if (input.Action6)
    {
      EquipWeapon(5, ref inventory);
      return state.HasChanged;
    }
    if (input.Action7)
    {
      EquipWeapon(6, ref inventory);
      return state.HasChanged;
    }

    if(state.EquippedSlot == -1) return state.HasChanged;
    if (inventory.HasChanged)
    {
      ref var item = ref inventory.Items[state.EquippedSlot];
      if(!item.HasChanged) return state.HasChanged;

      EquipWeapon(state.EquippedSlot, ref inventory);
    }

    return state.HasChanged;
  }

  private void EquipWeapon(int slot, ref PlayerInventoryState inventory)
  {
    ref var state = ref _state;
    Debug.Log($"Equip Weapon - {slot}");
    ref var item = ref inventory.Items[slot];
      
    if (state.EquippedSlot != -1 && _equippedWeapon != null)
    {
      Object.Destroy(_equippedWeapon);
      state.EquippedSlot = -1;
      state.EquippedWeapon = ITEM_TYPE.NULL;
      state.HasChanged = true;
    }
    
    if(item.ItemId == ITEM_TYPE.NULL) return;
    Debug.Log($"Attemping to equip {item.ItemId} with damage {item.ItemDamage}");
    if (!item.IsDeployable && !item.IsConsumable && item.ItemDamage == 0) return;

    Debug.Log($"Equipping {item.ItemId} with damage {item.ItemDamage}");

    if(slot == state.EquippedSlot) return;
    
    _equippedWeapon = Object.Instantiate(PrefabPool.Prefabs[ItemPool.ItemPrefabs[item.ItemId]], Vector3.zero, Quaternion.identity, _armTransform.transform);
    _equippedWeapon.transform.localPosition = Vector3.zero;
    _equippedWeapon.transform.localEulerAngles = Vector3.zero;

    _equippedWeapon.transform.Find("Interaction").gameObject.SetActive(false);
    
    state.EquippedSlot = slot;
    state.EquippedWeapon = item.ItemId;
    state.HasChanged = true;
  }

  public PlayerWeaponState State => _state;
}