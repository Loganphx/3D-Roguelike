using System.Collections.Generic;
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

  private static Dictionary<POWERUP_TYPE, Color> PowerupColors = new Dictionary<POWERUP_TYPE, Color>()
  {
    { POWERUP_TYPE.COMMON, new Color(103 / 255f, 34 / 255f, 1 / 255f) },
    { POWERUP_TYPE.UNCOMMON, new Color(29 / 255f, 195 / 255f, 0 / 255f) },
    { POWERUP_TYPE.RARE, new Color(3 / 255f, 67 / 255f, 195 / 255f) },
    { POWERUP_TYPE.EPIC, new Color(149 / 255f, 0 / 255f, 131 / 255f) },
    { POWERUP_TYPE.LEGENDARY, new Color(233 / 255f, 71 / 255f, 0 / 255f) },
  };
  
  private void Awake()
  {
    _outline = GetComponent<Outline>();
    _outline.enabled = false;
    
    transform.GetComponentInChildren<MeshRenderer>().material.color = PowerupColors[_powerupType];
  }

  private void FixedUpdate()
  {
    transform.Rotate(0, 1, 0);
  }

  public INTERACTABLE_TYPE GetInteractableType()
  {
    return INTERACTABLE_TYPE.POWERUP;
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