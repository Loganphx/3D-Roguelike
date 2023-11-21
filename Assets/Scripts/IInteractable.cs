public interface IInteractable
{
  public void Interact(IPlayer player);
}

public interface IDamagable
{
  public void TakeDamage(IPlayer player, float damage);
}

public interface IHoverable
{
  public void OnHoverEnter();
  public void OnHoverExit();
}