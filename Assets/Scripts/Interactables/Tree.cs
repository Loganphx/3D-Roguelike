using System.Collections.Generic;
using UnityEngine;

namespace Muck.Trees
{
  internal class TreeData
  {
    public readonly int Health;
    public readonly ITEM_TYPE DropItemId;
    
    public readonly TOOL_TYPE RequiredToolType;
    public readonly int MinimumToolDamage;

    public TreeData(int health, ITEM_TYPE itemId, TOOL_TYPE requiredToolType, int minimumToolDamage)
    {
      Health = health;
      DropItemId = itemId;
      RequiredToolType = requiredToolType;
      MinimumToolDamage = minimumToolDamage;
    }
  }

  public struct TreeState : IState
  {
    public int CurrentHealth;
  }

  public class Tree : MonoBehaviour, IDamagable
  {
    [SerializeField] private TreeTypes _nodeType;
    private TreeData _nodeData;
    private TreeState _nodeState;

    public void Awake()
    {
      var logTransform = transform.Find("Log");
      if (logTransform == null)
        Debug.LogError("Log Transform not found", this);
      
      var colliders = transform.GetComponentsInChildren<Collider>();
      foreach (var collider in colliders)
      {
        HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
      }
    }

    public void Start()
    {
      _nodeData = _trees[_nodeType];
      _nodeState = new TreeState()
      {
        CurrentHealth = _nodeData.Health
      };
    }

    public Transform Transform => transform;

    public void TakeDamage(IDamager player, Vector3 hitDirection, TOOL_TYPE toolType, int damage)
    {
      ref var nodeState = ref _nodeState;
      
      if (nodeState.CurrentHealth <= 0) return;
      
      if(!toolType.HasFlag(TOOL_TYPE.AXE)) return;
      
      if(damage < _nodeData.MinimumToolDamage) return;
        
      nodeState.CurrentHealth -= damage;
      Debug.Log($"Hit {_nodeType} Tree for {damage} => {nodeState.CurrentHealth} / {_nodeData.Health}");

      if (nodeState.CurrentHealth <= 0)
        Death(hitDirection);
    }

    private void Death(Vector3 hitDirection)
    {
      Debug.Log($"{_nodeType} Tree died");
      
      this.Death(hitDirection, _nodeData.DropItemId);
      
      transform.gameObject.SetActive(true);
      var logTransform = transform.Find("Log");
      logTransform.gameObject.SetActive(false);
    }

    private static readonly Dictionary<TreeTypes, TreeData> _trees = new Dictionary<TreeTypes, TreeData>()
    {
      {
        TreeTypes.Tree, new TreeData(
          health: 10,
          itemId: ITEM_TYPE.WOOD,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:1)
      },
      {
        TreeTypes.Birch, new TreeData(
          health: 25,
          itemId: ITEM_TYPE.WOOD_BIRCH,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:10)
      },
      {
        TreeTypes.Fir, new TreeData(
          health: 50,
          itemId: ITEM_TYPE.WOOD_FIR,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:25)
      },
      {
        TreeTypes.Oak, new TreeData(
          health:  100,
          itemId: ITEM_TYPE.WOOD_OAK,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:50)
      },
      {
        TreeTypes.DarkOak, new TreeData(
          health: 250,
          itemId: ITEM_TYPE.WOOD_DARKOAK,
          requiredToolType: TOOL_TYPE.AXE,
          minimumToolDamage:100)
      },
    };
  }
}