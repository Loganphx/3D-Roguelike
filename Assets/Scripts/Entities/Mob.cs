using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class MobAttackData
{
    public float attackDistance;
    public int attackIndex;
    public string originPath;
}
public class MobData
{
    public bool stopOnAttack;
    public bool ignoreBuildings;
    public MobAttackData[] attacks;
}

public enum MobType
{
    Sheep = 1,
}

public struct MobState : IState
{
    public int   CurrentHealth;
    public int MaxHealth;
    public bool  IsAttacking;
}

public class Mob : MonoBehaviour, IDamager, IDamagable
{
    private Transform _transform;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private PhysicsScene _physicsScene;
    private MobData _mobData;

    [SerializeField] private MobType _mobType;
    
    private IPlayer _target;
    private MobAttackData _currentAttack;

    private MobState _mobState;
    private bool isAttacking;
    private bool isPathing;

    public Transform Transform => _transform;
    public int CurrentHealth
    {
        get
        {
            ref var state = ref _mobState;
            return state.CurrentHealth;
        }
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _physicsScene = gameObject.scene.GetPhysicsScene();

        var colliders = _transform.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
        }
        
        _mobData = mobs[_mobType];

        _mobState = new MobState()
        {
            CurrentHealth = 100,
            MaxHealth = 100
        };    
    }


    private void FixedUpdate()
    {
        var position = _transform.position;
        
        if (!isAttacking)
        {
            var (player, playerPosition, playerDistance) = PlayerPool.GetClosestPlayer(position);
            if (float.IsPositiveInfinity(playerDistance))
            {
                if (isPathing)
                {
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(IsMovingHash, false);
                }
                return;
            }
            
            _target = player;
            _currentAttack = _mobData.attacks[0];

            _navMeshAgent.stoppingDistance = _currentAttack.attackDistance;
            
            // Debug.Log(playerDistance);
            // TODO: Determine which attack to use based on ?

            
            if (playerDistance >= _currentAttack.attackDistance)
            {
                isPathing = true;
                _navMeshAgent.isStopped = false;
                _rigidbody.isKinematic = true;
                // Do some sort of pathing
                _navMeshAgent.SetDestination(playerPosition);
                _animator.SetBool(IsMovingHash, true);
            }
            else
            {
                // Starts the windup (animation)
                if (_mobData.stopOnAttack)
                {
                    isPathing = false;
                    _rigidbody.isKinematic = true;
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(IsMovingHash, false);
                }
                
                // _transform.LookAt(playerPosition);
                _animator.SetTrigger(AttackHash);
            }
            
            // TODO Implement Cooldown for the attack (Trigger GCD)
        }
        
        if (Vector3.Distance(_target.Transform.position, _transform.position) < 1.2)
        {
            _rigidbody.isKinematic = false;
        }
    }

    // This will play at the beginning of the attack animation (via AnimationEvent)
    [InvokedByAnimationEvent, UsedImplicitly]
    private void StartAttack()
    {
        Debug.Log("Mob.StartAttack");
        isAttacking = true;
    }
    
    // This will play at the apex of the attack animation (via AnimationEvent)
    [InvokedByAnimationEvent, UsedImplicitly]
    private void Attack()
    {
        // Debug.Log("Mob.Attack");
        // Do damage
        var paths = _currentAttack.originPath.Split("/");
        var attackOrigin = _transform; 
        foreach (var path in paths)
        {
            attackOrigin = attackOrigin.Find(path);
        }
        
        var hit = _physicsScene.Raycast(attackOrigin.position, attackOrigin.forward, _hits, 5, (1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Collide);
        if (hit != 0)
        {
            // Debug.Log(_hits[0].transform.root);
            HitboxSystem.ColliderHashToDamagable[_hits[0].collider.GetHashCode()].TakeDamage(this, attackOrigin.forward, TOOL_TYPE.NULL, 10);
        }
    }
    
    // This will play at the end of the attack animation (via AnimationEvent)

    [InvokedByAnimationEvent, UsedImplicitly]
    private void FinishAttack()
    {
        // Debug.Log("Mob.FinishAttack");
        isAttacking = false;
    }

    private static Dictionary<MobType, MobData> mobs = new Dictionary<MobType, MobData>()
    {
        {
            MobType.Sheep, new MobData()
            {
                stopOnAttack = true,
                ignoreBuildings = true,
                attacks = new []
                {
                    new MobAttackData()
                    {
                        attackDistance = 2.5f,
                        attackIndex = 0,
                        originPath = "Animal/Head",
                    }
                },
            }
        }
    };

    private readonly RaycastHit[] _hits = new RaycastHit[5];
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    
    public void TakeDamage(IDamager damager, Vector3 hitDirection, TOOL_TYPE toolType, int damage)
    {
        ref var mobState = ref _mobState;
        if(mobState.CurrentHealth <= 0) return;
    
        mobState.CurrentHealth -= damage;
        Debug.Log($"Hit {_mobType} for {damage} => {mobState.CurrentHealth} / {_mobState.MaxHealth}");
        if (mobState.CurrentHealth <= 0)
        {
            Death(hitDirection);
        }
    }
    public void Death(Vector3 hitDirection)
    {
        this.Death(hitDirection, ITEM_TYPE.COIN);
    }
    
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log($"OnCollisionEnter: {other.collider}", other.collider);
    //     var hitCollider = other.collider;
    //
    //     if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         Debug.Log($"We hit a player {other.GetContact(0).point}");
    //         var position = _transform.position;
    //         var otherPosition = other.transform.position;
    //
    //         var direction = position - otherPosition;
    //         _rigidbody.AddForce(direction * 10f, ForceMode.Force);
    //     }
    // }
}
