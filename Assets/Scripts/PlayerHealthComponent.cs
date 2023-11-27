internal struct PlayerHealthState : IState
{
    public int Health;
    public int MaxHealth;
}

internal class PlayerHealthComponent : IComponent<PlayerHealthState>
{
    public PlayerHealthState _healthState;

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
        }
    }

    public PlayerHealthState State { get; }
}