using UnityEngine;

public enum POWERUP_TYPE
{
  NULL = 0,
  COMMON,
  UNCOMMON,
  RARE,
  EPIC,
  LEGENDARY,
}
public class Powerup : MonoBehaviour, IInteractable, IHoverable
{
  private Outline _outline;

  [SerializeField] private POWERUP_TYPE _powerupType;
  
  private void Awake()
  {
    _outline = GetComponent<Outline>();
    _outline.enabled = false;
  }

  private void FixedUpdate()
  {
    transform.Rotate(0, 1, 0);
  }

  public void Interact(IPlayer player)
  {
    Debug.Log($"Interacting with {_powerupType} powerup");
    player.AddPowerup(_powerupType, 1);
    gameObject.SetActive(false);
  }

  public void OnHoverEnter(IPlayer player)
  {
    _outline.enabled = true;
  }

  public void OnHoverExit(IPlayer player)
  {
    _outline.enabled = false;
  }
}