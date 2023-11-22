using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public struct AnimalState : IState
{
    public int   CurrentHealth;
    public float LastAttackTime;
    public bool  IsAttacking;
    public float RemainingCooldown;
}

public enum AnimalTypes
{
    Sheep,
    Wolf,
    Cow,
}

public class AnimalData
{
    public int Health;

    public float FollowDistance = 10f;
    public float AttackDistance;
    public float WindupCooldown;
    public float AttackCooldown;
    public int   Damage;
}
public class Animal : MonoBehaviour, IComponent<AnimalState>, IDamagable
{
    [SerializeField] private AnimalTypes _animalType;
    [SerializeField] private AnimalData _animalData;
    [SerializeField] private AnimalState _animalState;

    private AIPath _aiPath;
    void Awake()
    {
        _animalData = _animals[_animalType];
        _animalState.CurrentHealth = _animalData.Health;
    }
    void FixedUpdate()
    {
        ref var state = ref _animalState;
        // Find Closest Player
        var closestPlayer = PlayerPool.GetClosestPlayer(transform.position);
        // Debug.Log($"Closest player is {closestPlayer.player}, {closestPlayer.distance}");
        // Move Towards Player

        if(closestPlayer.distance > _animalData.FollowDistance) return;
        
        if (closestPlayer.distance <= _animalData.AttackDistance)
        {
            if(!state.IsAttacking)
            {
                if (!(state.RemainingCooldown > 0))
                {
                    // Wind up attack
                    transform.LookAt(closestPlayer.player.Transform.position, Vector3.up);
                    BeginAttack(ref state);
                }
                else
                {
                    Debug.Log("Waiting for cooldown");
                }
            }
            
            // Attack
            transform.LookAt(closestPlayer.player.Transform.position, Vector3.up);
            Attack(ref state);
        }
        else
        {
            if (state.IsAttacking)
            {
                // Attack
                transform.LookAt(closestPlayer.player.Transform.position, Vector3.up);
                Attack(ref state);
            }
            else Debug.Log("Move Towards Player");
        }
        
        _animalState.RemainingCooldown -= Time.fixedDeltaTime;
    }

    private void BeginAttack(ref AnimalState state)
    {
        Debug.Log("Begin Attack windup");
        state.LastAttackTime    = (int) Time.time;
        state.RemainingCooldown = (int) _animalData.WindupCooldown;
        state.IsAttacking       = true;
    }

    private void Attack(ref AnimalState state)
    {
        if(state.RemainingCooldown > 0) return;
        
        Debug.Log("Attack!");
        
        // Do Damage to players within range.
        
        EndAttack(ref state);
    }
    
    private void EndAttack(ref AnimalState state)
    {
        Debug.Log("End Attack");
        state.RemainingCooldown = _animalData.AttackCooldown;
        state.IsAttacking       = false;
    }
    

    public AnimalState State => _animalState;

    public void        TakeDamage(IPlayer player, float damage)
    {
        ref var animalState = ref _animalState;
        if(animalState.CurrentHealth <= 0) return;
    
        animalState.CurrentHealth -= (int) damage;
        Debug.Log($"Hit {_animalType} for {damage} => {animalState.CurrentHealth} / {_animalData.Health}");
        if (animalState.CurrentHealth <= 0)
            Death();
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }

    private static Dictionary<AnimalTypes, AnimalData> _animals = new Dictionary<AnimalTypes, AnimalData>()
    {
        { AnimalTypes.Sheep, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,
        }},
        { AnimalTypes.Cow, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,
        }},
        { AnimalTypes.Wolf, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            AttackDistance = 2.5f,
            WindupCooldown = 0.3f,
            AttackCooldown = 3f,
        }}
    };
}
