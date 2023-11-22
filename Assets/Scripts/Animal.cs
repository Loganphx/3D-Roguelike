using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public struct AnimalState : IState
{
    public int   CurrentHealth;
    public float LastAttackTime;
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

    public float AttackDistance;
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

        if (closestPlayer.distance <= _animalData.AttackDistance)
        {
            transform.LookAt(closestPlayer.player.Transform.position, Vector3.up);
            if(state.RemainingCooldown <= 0)
            {
                // Attack Player
                Debug.Log("Attack player");
                state.LastAttackTime    = (int) Time.time;
                state.RemainingCooldown = (int) _animalData.AttackCooldown;
            }
            else
            {
                Debug.Log("Waiting for cooldown");
            }
        }
        else
        {
            Debug.Log("Move Towards Player");
        }
        
        _animalState.RemainingCooldown -= Time.deltaTime;
    }

    private static Dictionary<AnimalTypes, AnimalData> _animals = new Dictionary<AnimalTypes, AnimalData>()
    {
        { AnimalTypes.Sheep, new AnimalData()
        {
            Damage = 10,
            Health = 100,
            AttackDistance = 2.5f,
            AttackCooldown = 3f,
        }},
        { AnimalTypes.Cow, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            AttackDistance = 2.5f,
            AttackCooldown = 3f,
        }},
        { AnimalTypes.Wolf, new AnimalData()
        {
            Damage         = 10,
            Health         = 100,
            AttackDistance = 2.5f,
            AttackCooldown = 3f,
        }}
    };

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
}
