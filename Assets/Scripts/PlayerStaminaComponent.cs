using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerStaminaState : IState
{
    public float Stamina;
    public float RegenCooldown;
    public float MaxStamina;
}

internal class PlayerStaminaComponent : IComponent<PlayerStaminaState>
{
    public PlayerStaminaState _staminaState;
    
    public PlayerStaminaComponent(IPlayer player, float maxStamina)
    {
        ref var state = ref _staminaState;
        state.MaxStamina = maxStamina;
        state.Stamina = maxStamina;
    }

    public void ProcessInput(ref GameplayInput input, ref PlayerHungerState hungerState)
    {
        ref var state = ref _staminaState;

        state.RegenCooldown -= Time.fixedDeltaTime;
        if (state.RegenCooldown > 0)
        {
            return;
        }
        
        if(hungerState.Hunger <= 0)
        {
            state.Stamina -= Time.fixedDeltaTime;
            return;
        }
        
        _staminaState.Stamina = Mathf.Min(_staminaState.Stamina + .25f, _staminaState.MaxStamina);
        hungerState.Hunger -= Time.fixedDeltaTime/2f;
    }
    
    public PlayerStaminaState State { get; }
}