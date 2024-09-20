using System;
using UnityEngine;

internal struct PlayerHealthState : IState
{
    public float Health;
    public int MaxHealth;
    
    public float Shield;
    public int MaxShield;
    
    // Every 10 Armor is 1% Damage Reduction
    public int Armor;

    public float HealthRegen;
    
    public float RegenCooldown; 
    
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

    public bool OnFixedUpdate(ref PlayerHungerState hungerState)
    {
        ref var state = ref _healthState;
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

        if (hungerState.Hunger != 0)
        {
            if (!Mathf.Approximately(state.Health, state.MaxHealth))
            {
                AddHealth((Mathf.Log10(state.MaxHealth)-1) * 0.005f + state.HealthRegen); // Around 6m to full by default
            }
            else if(state.MaxShield != 0)
            {
                AddShield((Mathf.Log10(state.MaxShield)) * 0.005f); // Around 3m to full by default
            }
        }
        
        return state.HasChanged;
    }
    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        
        ref var state = ref _healthState;

        var damageReduction = (state.Armor / 10f) / 100f; // 600 / 10 = .60% DR
        // Shield = 50, Damage = 100 (50-100) = -(-50)
        // Shield = 40, Damage = 30 (40-30) = -(10)
        damage *= (1 - damageReduction); // 40%
        var damageLeft = -(state.Shield - damage);

        DecreaseShield(damage);
        
        if (damageLeft > 0)
        {
            DecreaseHealth(damageLeft);
        }

        state.RegenCooldown = 2f;
    }

    public void AddHealth(float amount)
    {
        if(amount == 0) return;

        ref var state = ref _healthState;
        
        state.Health = Math.Min(state.Health + amount, state.MaxHealth);
        state.IsDead = state.Health == 0;
        state.HasChanged = true;
    }

    public void DecreaseHealth(float amount)
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

    public void AddShield(float amount)
    {
        if(amount == 0) return;

        ref var state = ref _healthState;
        state.Shield = Math.Min(state.Shield + amount, state.MaxShield);
        state.HasChanged = true;
    }

    public void DecreaseShield(float amount)
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