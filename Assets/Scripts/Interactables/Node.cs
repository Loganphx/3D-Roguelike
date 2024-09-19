using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class Node : MonoBehaviour, IDamagable
{
  [SerializeField] private NODE_TYPE _nodeType;

  private NodeData _nodeData;

  private NodeState _nodeState;

  public Transform Transform => transform;

  public void Awake()
  {
    _nodeData = _nodes[_nodeType];
    _nodeState.CurrentHealth = _nodeData.Health;

    var damagable = GetComponent<DamagableResource>();
    damagable.Initialize(LootTablePool.NodeLootTables[_nodeType], _nodeData.Health);
    damagable.Initialize(TOOL_TYPE.PICKAXE, _nodeData.MinimumToolDamage);
  }

  public void OnHit(IDamager player, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
  {
    GetComponent<Damagable>().OnHit(player, hitDirection, hitPosition, toolType, damage);
  }

  private static readonly Dictionary<NODE_TYPE, NodeData> _nodes = new Dictionary<NODE_TYPE, NodeData>()
  {
    {
      NODE_TYPE.Stone, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100, 
        itemId: ITEM_TYPE.ROCK,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:10)
    },
    {
      NODE_TYPE.Coal, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_COAL,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NODE_TYPE.Iron, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_IRON,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NODE_TYPE.Gold, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_GOLD,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:25)
    },
    {
      NODE_TYPE.Mythril, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId: ITEM_TYPE.ORE_MYTHRIL,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:50)
    },
    {
      NODE_TYPE.Adamantite, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId:ITEM_TYPE.ORE_ADAMANTITE,
        requiredToolType:TOOL_TYPE.PICKAXE,
        minimumToolDamage:100)
    },
  };
}