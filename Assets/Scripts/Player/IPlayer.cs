using UnityEngine;

public interface IPlayer : IDamager
{
  public Transform Transform { get; }

  public bool IsDead { get; }

  public int GetGoldInInventory();
  public ITEM_TYPE GetCurrentWeapon();
  public int AddItem(ITEM_TYPE itemId, int amount);
  public void RemoveItem(ITEM_TYPE itemId, int amount);
  public void RemoveItem(int slotIndex, int amount);
  public void AddPowerup(POWERUP_TYPE powerupId, int amount);
  public void AddHealth(int amount);
  public void AddStamina(int amount);
  public void AddHunger(int amount);
}