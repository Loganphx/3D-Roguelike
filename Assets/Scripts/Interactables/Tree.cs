using System.Collections.Generic;
using Interactables.Trees;
using UnityEngine;

namespace Interactables
{
  internal class TreeData
  {
    public readonly int Health;
    
    public readonly TOOL_TYPE RequiredToolType;
    public readonly int MinimumToolDamage;

    public TreeData(int health, TOOL_TYPE requiredToolType, int minimumToolDamage)
    {
      Health = health;
      RequiredToolType = requiredToolType;
      MinimumToolDamage = minimumToolDamage;
    }
  }

  public struct TreeState : IState
  {
    public int CurrentHealth;
  }

  [RequireComponent(typeof(DamagableResource))]
  public class Tree : MonoBehaviour, IDamagable
  {
    private Animator _animator;
    
    [SerializeField] private TREE_TYPE _treeType;
    private TreeData _nodeData;
    
    public void Awake()
    {
      _animator = GetComponent<Animator>();

      _nodeData = _trees[_treeType];
      
      var damagable = GetComponent<DamagableResource>();
      damagable.Initialize(LootTablePool.TreeLootTables[_treeType], _nodeData.Health);
      damagable.Initialize(TOOL_TYPE.AXE, _nodeData.MinimumToolDamage);
      damagable.OnDeath += OnDeath;
    }
    
    public Transform Transform => transform;

    public void OnHit(IDamager player, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
    {
      GetComponent<Damagable>().OnHit(player, hitDirection, hitPosition, toolType, damage);
    }

    private void OnDeath(Vector3 hitDirection)
    {
      _animator.SetTrigger(Death);
      // Todo: Have tree fall in the hit direction! 
    }

    private static readonly Dictionary<TREE_TYPE, TreeData> _trees = new Dictionary<TREE_TYPE, TreeData>()
    {
      {
        TREE_TYPE.Tree, new TreeData(
          health: 10,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:1)
      },
      {
        TREE_TYPE.Birch, new TreeData(
          health: 25,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:10)
      },
      {
        TREE_TYPE.Fir, new TreeData(
          health: 50,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:25)
      },
      {
        TREE_TYPE.Oak, new TreeData(
          health:  100,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:50)
      },
      {
        TREE_TYPE.DarkOak, new TreeData(
          health: 250,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:100)
      },
    };

    private static readonly int Death = Animator.StringToHash("Death");
  }
}