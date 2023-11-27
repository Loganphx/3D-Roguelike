using UnityEngine;

public interface IPlayer : IDamager
{
  public int       Gold      { get; set; }
  public Transform Transform { get; }
  
  public void AddItem(Item item);
}