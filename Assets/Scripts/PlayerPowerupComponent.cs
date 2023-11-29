internal struct PowerupItem
{
  public POWERUP_TYPE PowerupId;
  public int Amount;

  public bool HasChanged;
}

internal struct PlayerPowerupState : IState
{
  public bool HasChanged;
  public PowerupItem[] Powerups;
}

internal class PlayerPowerupComponent : IComponent<PlayerPowerupState>
{
  public PlayerPowerupState _state;

  // ReSharper disable once UnusedParameter.Local
  public PlayerPowerupComponent(IPlayer player)
  {
    ref var state = ref _state;
    state.Powerups = new PowerupItem[6*2];
    
    for (int i = 0; i < state.Powerups.Length; i++)
    {
      ref var item = ref state.Powerups[i];

      item.PowerupId = POWERUP_TYPE.NULL;
      item.Amount = 0;
      item.HasChanged = true;
    }
    state.HasChanged = true;

  }

  public void AddPowerup(POWERUP_TYPE powerupId, int amount)
  {
    ref var state = ref _state;
    
    for (int i = 0; i < state.Powerups.Length; i++)
    {
      ref var item = ref state.Powerups[i];
      if (item.PowerupId == powerupId)
      {
        item.Amount      += amount;
        item.HasChanged  =  true;
        
        state.HasChanged =  true;
        return;
      }
    }

    for (int i = 0; i < state.Powerups.Length; i++)
    {
      ref var item = ref state.Powerups[i];
      if (item.PowerupId == POWERUP_TYPE.NULL)
      {
        item.PowerupId      = powerupId;
        item.Amount      = amount;
        item.HasChanged  = true;
        state.HasChanged = true;
        return;
      }
    }
    
  }

  public PlayerPowerupState State => _state;
  
}