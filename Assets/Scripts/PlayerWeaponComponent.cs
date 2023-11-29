using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerWeaponState : IState
{
  public ITEM_TYPE EquippedWeapon;
  public int EquippedSlot;
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
  }
  
  public void OnFixedUpdate(ref GameplayInput input, ref PlayerInventoryState inventory)
  {
    if (input.Action1)
    {
      EquipWeapon(0, ref inventory);
      return;
    }
    
    if (input.Action2)
    {
      EquipWeapon(1, ref inventory);
      return;
    }
    if (input.Action3)
    {
      EquipWeapon(2, ref inventory);
      return;
    }
    
    if (input.Action4)
    {
      EquipWeapon(3, ref inventory);
      return;
    }
    if (input.Action5)
    {
      EquipWeapon(4, ref inventory);
      return;
    }
    if (input.Action6)
    {
      EquipWeapon(5, ref inventory);
      return;
    }

    ref var state = ref _state;
    if(state.EquippedSlot == -1) return;
    if (inventory.HasChanged)
    {
      ref var item = ref inventory.Items[state.EquippedSlot];
      if(!item.HasChanged) return;

      EquipWeapon(state.EquippedSlot, ref inventory);
    }
  }

  private void EquipWeapon(int slot, ref PlayerInventoryState inventory)
  {
    ref var state = ref _state;
    ref var item = ref inventory.Items[slot];
      
    if (state.EquippedSlot != -1 && _equippedWeapon != null)
    {
      Object.Destroy(_equippedWeapon);
      state.EquippedSlot = -1;
      state.EquippedWeapon = ITEM_TYPE.NULL;
    }
    
    if(item.ItemId == ITEM_TYPE.NULL) return;
    if (!item.IsDeployable && item.ItemDamage == 0) return;
    
    Debug.Log($"Equipping {item.ItemId} with damage {item.ItemDamage}");

    if(slot == state.EquippedSlot) return;
    
    _equippedWeapon = Object.Instantiate(PrefabPool.Prefabs[ItemPool.ItemPrefabs[item.ItemId]], Vector3.zero, Quaternion.identity, _armTransform.transform);
    _equippedWeapon.transform.localPosition = Vector3.zero;
    state.EquippedSlot = slot;
    state.EquippedWeapon = item.ItemId;
  }

  public PlayerWeaponState State => _state;
}