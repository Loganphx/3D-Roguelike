using UnityEngine;

internal struct PlayerHungerState : IState
{
    public float Hunger;
    public float MaxHunger;
}

internal class PlayerHungerComponent : IComponent<PlayerHungerState>
{
    public PlayerHungerState _hungerState;
    
    public PlayerHungerComponent(IPlayer player, float maxHunger)
    {
        ref var state = ref _hungerState;
        state.MaxHunger = maxHunger;
        state.Hunger = maxHunger;
    }
    
    public PlayerHungerState State { get; }

    public void OnFixedUpdate()
    {
        ref var state = ref _hungerState;
        
        state.Hunger -= Time.fixedDeltaTime/2f;
    }
}