using UnityEngine;

public interface IPlayer : IDamager
{
  public Transform Transform { get; }

  public int GetGoldInInventory();
  public ITEM_TYPE GetCurrentWeapon();
  public void AddItem(ITEM_TYPE itemId, int amount);
  public void RemoveItem(ITEM_TYPE itemId, int amount);
  public void AddPowerup(POWERUP_TYPE powerupId, int amount);
}