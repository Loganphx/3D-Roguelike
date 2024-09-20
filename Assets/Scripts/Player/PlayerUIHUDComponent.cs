using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class PlayerUIHUDComponent
{
  private readonly TMP_Text _healthText;
  private readonly Image _healthFill;
  private readonly Image _shieldFill;
  private readonly Image _staminaFill;
  private readonly Image _hungerFill;

  public PlayerUIHUDComponent(TMP_Text healthText, Image healthFill, Image shieldFill,
    Image staminaFillBar, Image hungerFill)
  {
    _healthText = healthText;
    _healthFill = healthFill;
    _shieldFill = shieldFill;
    _staminaFill = staminaFillBar;
    _hungerFill = hungerFill;
  }
    
  public void OnUpdate(ref PlayerHealthState healthState, ref PlayerStaminaState staminaState, ref PlayerHungerState hungerState)
  {
    _healthText.text = healthState.Shield > 0 
      ? $"<color=#5DdfF1>{(healthState.Health+healthState.Shield):N0}</color> / {healthState.MaxHealth}"
      : $"{healthState.Health:N0} / {healthState.MaxHealth}";
    
    _healthText.color = Color.Lerp(Color.red, Color.green, healthState.Health / (float)healthState.MaxHealth);
    _healthFill.fillAmount = healthState.Health / (float)healthState.MaxHealth;
    _shieldFill.fillAmount = healthState.Shield / (float)healthState.MaxHealth;
   
    
    _staminaFill.fillAmount = staminaState.Stamina / staminaState.MaxStamina;
    _hungerFill.fillAmount = hungerState.Hunger / hungerState.MaxHunger;
        
  }
}