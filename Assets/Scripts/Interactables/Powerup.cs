using System.Collections.Generic;
using UnityEngine;

public enum POWERUP_TYPE
{
  NULL = 0,
  // COMMON
  INCREASE_HEALTH,
  INCREASE_STAMINA,
  SLOW_HUNGER,
  SHIELD,
  // UNCOMMON
  EXTRA_JUMP, 
  // RARE
  EXTRA_MELEE_DAMAGE,
  // EPIC
  PIGGY_BANK,
  // LEGENDARY
  THORS_HAMMER,
  
  MAX_VALUE
}
public class Powerup : MonoBehaviour, IInteractable, IHoverable
{
  private Outline _outline;

  [SerializeField] private POWERUP_TYPE _powerupType;

  private static Dictionary<RarityTypes, Color> _rarityColors = new Dictionary<RarityTypes, Color>()
  {
    { RarityTypes.COMMON, new Color(103 / 255f, 34 / 255f, 1 / 255f) },
    { RarityTypes.UNCOMMON, new Color(29 / 255f, 195 / 255f, 0 / 255f) },
    { RarityTypes.RARE, new Color(3 / 255f, 67 / 255f, 195 / 255f) },
    { RarityTypes.EPIC, new Color(149 / 255f, 0 / 255f, 131 / 255f) },
    { RarityTypes.LEGENDARY, new Color(233 / 255f, 71 / 255f, 0 / 255f) },
  };

  private static Dictionary<POWERUP_TYPE, RarityTypes> PowerupColors = new Dictionary<POWERUP_TYPE, RarityTypes>()
  {
    { POWERUP_TYPE.INCREASE_HEALTH,RarityTypes.COMMON},
    { POWERUP_TYPE.INCREASE_STAMINA,RarityTypes.COMMON},
    { POWERUP_TYPE.SLOW_HUNGER,RarityTypes.COMMON},
    { POWERUP_TYPE.SHIELD,RarityTypes.COMMON},
    { POWERUP_TYPE.EXTRA_JUMP, RarityTypes.UNCOMMON },
    { POWERUP_TYPE.EXTRA_MELEE_DAMAGE,RarityTypes.RARE},
    { POWERUP_TYPE.PIGGY_BANK,RarityTypes.EPIC},
    { POWERUP_TYPE.THORS_HAMMER,RarityTypes.LEGENDARY},
  };

  private void Awake()
  {
    _outline = GetComponent<Outline>();
    _outline.enabled = false;
    
    transform.GetComponentInChildren<MeshRenderer>().material.color = _rarityColors[PowerupColors[_powerupType]];
  }

  public void SetPowerupType(POWERUP_TYPE powerupType)
  {
    this._powerupType = powerupType;
    transform.GetComponentInChildren<MeshRenderer>().material.color = _rarityColors[PowerupColors[_powerupType]];
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