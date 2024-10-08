using System;
using UnityEngine;

[Serializable]
internal struct InventoryItem
{
  [SerializeField]
  public ITEM_TYPE ItemId;
  public int Amount;
  public int MaxStack;
  
  public int ItemDamage;
  public bool IsDeployable;
  public bool IsConsumable;

  public bool HasChanged;
}
[Serializable]
internal struct PlayerInventoryState : IState
{
  [SerializeField]
  public InventoryItem[] Items;
  public int GoldCount;
  public bool HasChanged;
}
[Serializable]
internal class PlayerInventoryComponent : IComponent<PlayerInventoryState>
{
  public PlayerInventoryState _state;

  // ReSharper disable once UnusedParameter.Local
  public PlayerInventoryComponent(IPlayer player)
  {
    ref var state = ref _state;
    state.Items = new InventoryItem[7*4];

    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];

      item.ItemId = ITEM_TYPE.NULL;
      item.IsDeployable  = false;
      item.IsConsumable  = false;
      item.Amount = 0;
      item.HasChanged = true;
    }
    state.HasChanged = true;
  }

  public PlayerInventoryState State => _state;

  public int AddItem(ITEM_TYPE itemId, int itemDamage, bool isDeployable, bool isConsumable, int amount)
  {
    ref var state = ref _state;

    // var width = (int)Math.Floor(Mathf.Sqrt(state.Items.Length));
    // var height = (int)Math.Ceiling(state.Items.Length/(float)width);
    // var str = "";
    // for (int i = 0; i < height; i++)
    // {
    //   for (int j = 0; j < width; j++)
    //   {
    //     str += state.Items[i*width + j].ItemId + " x " + state.Items[i*width + j].Amount + $"({i}, {j}) | ";
    //   }
    //   str += "\n";
    // }
    
    //Debug.Log(str);
    
    if(itemId == ITEM_TYPE.COIN) state.GoldCount += amount;
    
    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];
      if (item.ItemId == itemId)
      {
        int remainingRoom = item.MaxStack - item.Amount;

        var amountToAdd = Mathf.Min(amount, remainingRoom);
        amount -= amountToAdd;
        
        item.Amount += amountToAdd;
        item.HasChanged  =  true;
        state.HasChanged =  true;
        
        if(amount == 0) return 0;
        
      }
    }

    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];
      if (item.ItemId == ITEM_TYPE.NULL)
      {
        item.ItemId      = itemId;
        item.ItemDamage  = itemDamage;
        item.IsDeployable  = isDeployable;
        item.IsConsumable  = isConsumable;
        item.MaxStack = ItemPool.ItemMaxStacks[itemId];
        
        int remainingRoom = item.MaxStack - item.Amount;

        var amountToAdd = Mathf.Min(amount, remainingRoom);
        amount -= amountToAdd;
          
        item.Amount += amountToAdd;
        
        item.HasChanged  = true;
        state.HasChanged = true;

        Debug.Log("Adding " + itemId + " x " + item.Amount + $" to Slot {i}. ({amount} remaining)");

        if (amount == 0)
        {
          for (int j = 0; j < state.Items.Length; j++)
          {
            ref var item2 = ref state.Items[j];
            // Debug.Log($"{j}: {item2.ItemId} x {item2.Amount}");
          }
          return 0;
        }
      }
    }

    state.GoldCount -= amount;
    
    Debug.LogError("Full inventory error");
    return amount;
  }

  public void RemoveItem(ITEM_TYPE itemType, int amount)
  {
    Debug.Log("Removing " + itemType + " x " + amount + " from Player.");
    ref var state = ref _state;

    if (itemType == ITEM_TYPE.COIN)
    {
      state.GoldCount = Mathf.Max(0, state.GoldCount - amount);
    }
      
    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];

      if (item.ItemId == itemType)
      {
        item.Amount -= amount;
        
        item.HasChanged = true;
        state.HasChanged = true;
        
        if(item.Amount > 0) return;
        
        var remainingAmount = -item.Amount;

        item.ItemId = ITEM_TYPE.NULL;
        item.Amount = 0;
        item.ItemDamage = 0;
        item.IsDeployable = false;
        item.IsConsumable = false;
        
        item.HasChanged = true;
        state.HasChanged = true;
        
        if(remainingAmount == 0) return;
        
        amount = remainingAmount;
      }
    }
  }
    
  public void RemoveItem(int slotIndex, int amount)
  {
    ref var state = ref _state;

    ref var item = ref state.Items[slotIndex];
    var itemType = item.ItemId;
    
    if (itemType == ITEM_TYPE.COIN)
    {
      state.GoldCount = Mathf.Max(0, state.GoldCount - amount);
    }
      
    item.Amount -= amount;
        
    item.HasChanged = true;
    state.HasChanged = true;
        
    if(item.Amount > 0) return;
      
    item.ItemId = ITEM_TYPE.NULL;
    item.Amount = 0;
    item.ItemDamage = 0;
    item.IsDeployable = false;
    item.IsConsumable = false;

    item.HasChanged = true;
    state.HasChanged = true;
  }

  public bool ContainsItem(ITEM_TYPE itemType, int amount)
  {
    ref var state = ref _state;
      
    int total = 0;
    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];

      if (item.ItemId == itemType)
      {
        total += item.Amount;
      }
    }

    return total >= amount;
  }
  public void OnFixedUpdate()
  {
    ref var state = ref _state;
    
    if (state.HasChanged)
    {
      state.HasChanged = false;

      for (int i = 0; i < state.Items.Length; i++)
      {
        ref var item = ref state.Items[i];
        
        item.HasChanged = false;
      }
    }
  }
}