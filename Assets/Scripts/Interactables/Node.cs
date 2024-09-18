using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Node : MonoBehaviour, IDamagable
{
  [SerializeField] private NodeTypes _nodeType;

  private NodeData _nodeData;

  private NodeState _nodeState;

  public Transform Transform => transform;

  public void Awake()
  {
    _nodeData = _nodes[_nodeType];
    _nodeState.CurrentHealth = _nodeData.Health;
    
    var colliders = transform.GetComponentsInChildren<Collider>();
    foreach (var collider in colliders)
    {
      HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
    }
  }

  public void TakeDamage(IDamager player, Vector3 hitDirection, TOOL_TYPE toolType, int damage)
  {
    ref var nodeState = ref _nodeState;
    if (nodeState.CurrentHealth <= 0) return;

    if(!toolType.HasFlag(TOOL_TYPE.PICKAXE)) return;
    if(damage < _nodeData.MinimumToolDamage) return;

    nodeState.CurrentHealth -= damage;
    Debug.Log($"Hit {_nodeType} Node for {damage} => {nodeState.CurrentHealth} / {_nodeData.Health}");
    
    if (nodeState.CurrentHealth <= 0)
      Death(hitDirection);
  }
  
  private void Death(Vector3 hitDirection)
  {
    Debug.Log($"{_nodeType} Node died");
    // var dropPrefab = PrefabPool.Prefabs[_nodeData.itemType];
    
    this.Death(hitDirection, _nodeData.ItemType);
  }

  private static readonly Dictionary<NodeTypes, NodeData> _nodes = new Dictionary<NodeTypes, NodeData>()
  {
    {
      NodeTypes.Stone, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100, 
        itemId: ITEM_TYPE.ROCK,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:10)
    },
    {
      NodeTypes.Coal, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_COAL,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NodeTypes.Iron, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_IRON,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NodeTypes.Gold, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_GOLD,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NodeTypes.Mythril, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId: ITEM_TYPE.ORE_MYTHRIL,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:50)
    },
    {
      NodeTypes.Adamantite, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId:ITEM_TYPE.ORE_ADAMANTITE,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:100)
    },
  };
}