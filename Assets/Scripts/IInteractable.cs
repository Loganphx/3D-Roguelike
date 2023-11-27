
using UnityEngine;

public interface IInteractable
{
  public void Interact(IPlayer player);
}

public interface IDamager
{
  
}
public interface IDamagable
{
  public void TakeDamage(IDamager player, Vector3 hitDirection, int damage);
}

public interface IHoverable
{
  public void OnHoverEnter();
  public void OnHoverExit();
}