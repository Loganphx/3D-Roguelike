public interface IInteractable
{
  public void Interact(IPlayer player);
}

public interface IHoverable
{
  public void OnHoverEnter();
  public void OnHoverExit();
}