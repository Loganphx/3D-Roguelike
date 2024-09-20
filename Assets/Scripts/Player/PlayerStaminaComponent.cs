using ECS.Movement.Services;
using UnityEngine;

internal struct PlayerStaminaState : IState
{
    public float Stamina;
    public float RegenCooldown;
    public float MaxStamina;

    public bool HasChanged;
}

internal class PlayerStaminaComponent : IComponent<PlayerStaminaState>
{
    public PlayerStaminaState _staminaState;
    
    // ReSharper disable once UnusedParameter.Local
    public PlayerStaminaComponent(IPlayer player, float maxStamina)
    {
        ref var state = ref _staminaState;
        state.MaxStamina = maxStamina;
        state.Stamina = maxStamina;
    }

    public bool ProcessInput(ref GameplayInput input, ref PlayerHungerState hungerState)
    {
        ref var state = ref _staminaState;
        state.HasChanged = false;

        if (state.RegenCooldown > 0)
        {
            state.RegenCooldown -= Time.fixedDeltaTime;
            state.HasChanged = true;
        }
        
        if (state.RegenCooldown > 0)
        {
            return state.HasChanged;
        }
        
        if(hungerState.Hunger <= 0)
        {
            state.Stamina -= Time.fixedDeltaTime;
            state.HasChanged = true;
            return state.HasChanged;
        }
        
        state.Stamina = Mathf.Min(state.Stamina + state.MaxStamina/400f, state.MaxStamina);
        state.HasChanged = true;
        
        hungerState.Hunger -= (Time.fixedDeltaTime/2f) * hungerState.HungerDrainRate;
        hungerState.HasChanged = true;
        
        return state.HasChanged;
    }

    public void AddStamina(int amount)
    {
        ref var state = ref _staminaState;
        state.Stamina = Mathf.Min(state.Stamina + amount, state.MaxStamina);
    }
    public PlayerStaminaState State => _staminaState;
}