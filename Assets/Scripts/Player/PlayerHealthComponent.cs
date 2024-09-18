using System;

internal struct PlayerHealthState : IState
{
    public int Health;
    public int MaxHealth;

    public bool IsDead;

    public bool HasChanged;
}

internal class PlayerHealthComponent : IComponent<PlayerHealthState>
{
    public PlayerHealthState _healthState;

    // ReSharper disable once UnusedParameter.Local
    public PlayerHealthComponent(IPlayer player, int maxHealth)
    {
        ref var state = ref _healthState;
        state.MaxHealth = maxHealth;
        state.Health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        ref var state = ref _healthState;
        state.Health -= damage;
        if (state.Health <= 0)
        {
            state.Health = 0;
            state.IsDead = true;
        }
    }

    public void AddHealth(int amount)
    {
        ref var state = ref _healthState;
        state.Health = Math.Min(state.Health + amount, state.MaxHealth);
    }

    public PlayerHealthState State => _healthState;
}