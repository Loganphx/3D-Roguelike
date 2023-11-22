using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Node : MonoBehaviour, IDamagable
{
  [SerializeField] private  NodeTypes _nodeType;

  private NodeData  _nodeData;

  private NodeState  _nodeState;

  public void Awake()
  {
    _nodeData = _nodes[_nodeType];
    _nodeState.CurrentHealth = _nodeData.health;
  }

  public void TakeDamage(IPlayer player, float damage)
  {
    ref var nodeState = ref _nodeState;
    if(nodeState.CurrentHealth <= 0) return;
    
    nodeState.CurrentHealth -= (int) damage;
    Debug.Log($"Hit {_nodeType} Node for {damage} => {nodeState.CurrentHealth} / {_nodeData.health}");
    if (nodeState.CurrentHealth <= 0)
      Death();
  }

  private void Death()
  {
    Debug.Log($"{_nodeType} Node died");
    gameObject.SetActive(false);
  }

  private static Dictionary<NodeTypes, NodeData> _nodes = new Dictionary<NodeTypes, NodeData>()
  {
    {
      NodeTypes.Stone, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    },
    {
      NodeTypes.Coal, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    },
    {
      NodeTypes.Iron, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    },
    {
      NodeTypes.Gold, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    },
    {
      NodeTypes.Mythril, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    }, {
      NodeTypes.Adamantite, new NodeData()
      {
        color  = new Color(0.5f, 0.5f, 0.5f),
        health = 100,
        drops  = 1,
      }
    },
    
  };
}
