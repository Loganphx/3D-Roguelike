using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class PlayerUIHUDComponent
{
  private readonly TMP_Text _healthText;
  private readonly Image _healthFill;
  private readonly Image _staminaFillBar;
  private readonly Image _hungerFill;

  public PlayerUIHUDComponent(TMP_Text healthText, Image healthFill, Image staminaFillBar, Image hungerFill)
  {
    _healthText = healthText;
    _healthFill = healthFill;
    _staminaFillBar = staminaFillBar;
    _hungerFill = hungerFill;
  }
    
  public void OnUpdate(ref PlayerHealthState healthState, ref PlayerStaminaState staminaState, ref PlayerHungerState hungerState)
  {
    _healthText.text = $"{healthState.Health} / {healthState.MaxHealth}";
    _healthText.color = Color.Lerp(Color.red, Color.green, healthState.Health / (float)healthState.MaxHealth);
    _healthFill.fillAmount = healthState.Health / (float)healthState.MaxHealth;
    _staminaFillBar.fillAmount = staminaState.Stamina / staminaState.MaxStamina;
    _hungerFill.fillAmount = hungerState.Hunger / hungerState.MaxHunger;
        
  }
}