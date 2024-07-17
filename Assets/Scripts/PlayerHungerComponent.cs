using System;
using UnityEngine;

internal struct PlayerHungerState : IState
{
    public float Hunger;
    public float MaxHunger;

    public bool HasChanged;
}

internal class PlayerHungerComponent : IComponent<PlayerHungerState>
{
    public PlayerHungerState _hungerState;
    
    // ReSharper disable once UnusedParameter.Local
    public PlayerHungerComponent(IPlayer player, float maxHunger)
    {
        ref var state = ref _hungerState;
        state.MaxHunger = maxHunger;
        state.Hunger = maxHunger;
        state.HasChanged = true;
    }
    
    public bool OnFixedUpdate()
    {
        ref var state = ref _hungerState;
        state.HasChanged = false;

        if (state.Hunger == 0) return false;
        
        state.Hunger = Mathf.Max(0, state.Hunger - Time.fixedDeltaTime/2f);
        state.HasChanged = true;
        
        return state.HasChanged;
    }

    public bool AddHunger(int amount)
    {
        ref var state = ref _hungerState;
        if(Math.Abs(state.Hunger - state.MaxHunger) < 0.05f) return false;
        
        state.Hunger = Mathf.Min(state.MaxHunger, state.Hunger + amount);
        state.HasChanged = true;
        return state.HasChanged;
    }
    
    public PlayerHungerState State => _hungerState;

}