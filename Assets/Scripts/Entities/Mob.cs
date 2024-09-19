using System;
using System.Collections.Generic;
using Interactables;
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

public enum MOB_TYPE
{
    SHEEP = 1,
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

    [SerializeField] private MOB_TYPE _mobType;
    
    private IDamagable _target;
    private MobAttackData _currentAttack;

    private bool isAttacking;
    private bool isPathing;

    public Transform Transform => _transform;
    public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
        GetComponent<Damagable>().OnHit(damager, hitDirection, hitPosition, toolType, damage);
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _physicsScene = gameObject.scene.GetPhysicsScene();
        
        _mobData = mobs[_mobType];

        GetComponent<Damagable>().Initialize(LootTablePool.MobLootTables[_mobType], 100);
    }


    private void FixedUpdate()
    {
        var position = _transform.position;
        
        if (!isAttacking)
        {
            var (build, buildPosition, buildDistance) = ResourceManager.GetClosestBuild(position);
            var (player, playerPosition, playerDistance) = PlayerPool.GetClosestPlayer(position);
            if (float.IsPositiveInfinity(playerDistance) && float.IsPositiveInfinity(buildDistance))
            {
                if (isPathing)
                {
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(IsMovingHash, false);
                }
                return;
            }

            float targetDistance = 0;
            Vector3 targetPosition;
            if (float.IsPositiveInfinity(buildDistance) || playerDistance < buildDistance)
            {
                Debug.Log($"Player {playerDistance} < {buildDistance}");
                _target = player;
                targetDistance = playerDistance;
                targetPosition = playerPosition;
            }
            else 
            {
                Debug.Log($"Build {buildDistance} < {playerDistance}");
                _target = build;
                targetDistance = buildDistance;
                targetPosition = buildPosition;
            }

            _currentAttack = _mobData.attacks[0];

            _navMeshAgent.stoppingDistance = _currentAttack.attackDistance;
            
            // Debug.Log(playerDistance);
            // TODO: Determine which attack to use based on ?

            
            if (targetDistance >= _currentAttack.attackDistance)
            {
                isPathing = true;
                _navMeshAgent.isStopped = false;
                _rigidbody.isKinematic = true;
                // Do some sort of pathing
                _navMeshAgent.SetDestination(targetPosition);
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
                
                _transform.LookAt(targetPosition);
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
        // Debug.Log("Mob.StartAttack");
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

        var hitDirection = _target.Transform.position - attackOrigin.position;
        Debug.DrawRay(attackOrigin.position, hitDirection, Color.red, 2);
        var hits = _physicsScene.Raycast(attackOrigin.position, hitDirection, 
            _hits, 5, (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Damagable")), QueryTriggerInteraction.Collide);
        if (hits != 0)
        {
            Debug.Log(_hits[0].transform.root);
            var hit = _hits[0];
            HitboxSystem.ColliderHashToDamagable[_hits[0].collider.GetHashCode()]
                .OnHit(this, attackOrigin.forward, hit.point, TOOL_TYPE.NULL, 10);
        }
    }
    
    // This will play at the end of the attack animation (via AnimationEvent)

    [InvokedByAnimationEvent, UsedImplicitly]
    private void FinishAttack()
    {
        // Debug.Log("Mob.FinishAttack");
        isAttacking = false;
    }

    private static Dictionary<MOB_TYPE, MobData> mobs = new Dictionary<MOB_TYPE, MobData>()
    {
        {
            MOB_TYPE.SHEEP, new MobData()
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