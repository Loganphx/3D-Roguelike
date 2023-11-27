using System;
using UnityEngine;

struct InventoryItem
{
  public ITEM_TYPE ItemId;
  public int Amount;

  public bool HasChanged;
}
internal struct PlayerInventoryState : IState
{
  public InventoryItem[] Items;
  public bool HasChanged;
}
internal class PlayerInventoryComponent : IComponent<PlayerInventoryState>
{
  public PlayerInventoryState _state;

  public PlayerInventoryComponent(IPlayer player)
  {
    ref var state = ref _state;
    state.Items = new InventoryItem[24];

    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];

      item.ItemId = ITEM_TYPE.NULL;
      item.Amount = 0;
      item.HasChanged = true;
    }
    state.HasChanged = true;

  }

  public PlayerInventoryState State => _state;

  public void AddItem(ITEM_TYPE itemId, int amount)
  {
    ref var state = ref _state;

    var width = (int)Math.Floor(Mathf.Sqrt(state.Items.Length));
    var height = (int)Math.Ceiling(state.Items.Length/(float)width);

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
    
    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];
      if (item.ItemId == itemId)
      {
        item.Amount += amount;
        item.HasChanged = true;
          state.HasChanged = true;
        return;
      }
    }

    for (int i = 0; i < state.Items.Length; i++)
    {
      ref var item = ref state.Items[i];
      if (item.ItemId == 0)
      {
        item.ItemId = itemId;
        item.Amount = amount;
        item.HasChanged = true;
        state.HasChanged = true;
        return;
      }
    }
  }
}