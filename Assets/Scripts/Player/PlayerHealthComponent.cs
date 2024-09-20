using System;

internal struct PlayerHealthState : IState
{
    public int Health;
    public int MaxHealth;
    
    public int Shield;
    public int MaxShield;
    
    public int Armor;

    public bool IsDead;

    public bool HasChanged;
}

internal class PlayerHealthComponent : IComponent<PlayerHealthState>
{
    public PlayerHealthState _healthState;

    // ReSharper disable once UnusedParameter.Local
    public PlayerHealthComponent(IPlayer player, int maxHealth, int maxShield = 0)
    {
        ref var state = ref _healthState;
        
        state.Health = maxHealth;
        state.MaxHealth = maxHealth;

        state.Shield = maxShield;
        state.MaxShield = maxShield;

        state.HasChanged = true;
    }

    public void TakeDamage(int damage)
    {
        ref var state = ref _healthState;

        // Shield = 50, Damage = 100 (50-100) = -(-50)
        // Shield = 40, Damage = 30 (40-30) = -(10)
        var damageLeft = -(state.Shield - damage);

        DecreaseShield(damage);
        
        if (damageLeft > 0)
        {
            DecreaseHealth(damageLeft);
        }
    }

    public void AddHealth(int amount)
    {
        if(amount == 0) return;

        ref var state = ref _healthState;
        state.Health = Math.Min(state.Health + amount, state.MaxHealth);
        state.HasChanged = true;
    }

    public void DecreaseHealth(int amount)
    {
        if(amount == 0) return;
        
        ref var state = ref _healthState;
        state.Health -= amount;
        state.HasChanged = true;
        
        if (state.Health <= 0)
        {
            state.Health = 0;
            state.IsDead = true;
        }
    }

    public void AddShield(int amount)
    {
        if(amount == 0) return;

        ref var state = ref _healthState;
        state.Shield = Math.Min(state.Shield + amount, state.MaxShield);
        state.HasChanged = true;
    }

    public void DecreaseShield(int amount)
    {
        if(amount == 0) return;
        
        ref var state = ref _healthState;
        state.Shield -= amount;
        state.HasChanged = true;
        
        if (state.Shield <= 0)
        {
            state.Shield = 0;
        }
    }


    public PlayerHealthState State => _healthState;
}