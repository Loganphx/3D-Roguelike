using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Node : MonoBehaviour, IDamagable
{
  [SerializeField] private NodeTypes _nodeType;

  private NodeData _nodeData;

  private NodeState _nodeState;

  public void Awake()
  {
    _nodeData = _nodes[_nodeType];
    _nodeState.CurrentHealth = _nodeData.health;
  }

  public void TakeDamage(IDamager player, Vector3 hitDirection, int damage)
  {
    ref var nodeState = ref _nodeState;
    if (nodeState.CurrentHealth <= 0) return;

    nodeState.CurrentHealth -= damage;
    Debug.Log($"Hit {_nodeType} Node for {damage} => {nodeState.CurrentHealth} / {_nodeData.health}");
    if (nodeState.CurrentHealth <= 0)
      Death(hitDirection);
  }

  private void Death(Vector3 hitDirection)
  {
    Debug.Log($"{_nodeType} Node died");
    // var dropPrefab = PrefabPool.Prefabs[_nodeData.itemType];
    var position = transform.position;
    var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
    var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
    var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
    var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[_nodeData.itemType]];

    var item = Instantiate(itemPrefab, Vector3.zero,
      Quaternion.identity, itemTemplate.transform);

    item.transform.localPosition = Vector3.zero;
    
    itemTemplate.GetComponent<Item>().SetItemType(_nodeData.itemType);

    // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
    hitDirection.y = 1;
    itemTemplate.GetComponent<Rigidbody>().AddForce(hitDirection * 3f, ForceMode.Impulse);
    // drop.GetComponent<Rigidbody>().AddForce(hitDirection * 10f, ForceMode.Impulse);

    //drop.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    gameObject.SetActive(false);
  }

  private static readonly Dictionary<NodeTypes, NodeData> _nodes = new Dictionary<NodeTypes, NodeData>()
  {
    {
      NodeTypes.Stone, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100, 
        itemId: ITEM_TYPE.ROCK)
    },
    {
      NodeTypes.Coal, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_COAL)
    },
    {
      NodeTypes.Iron, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_IRON)
    },
    {
      NodeTypes.Gold, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health: 100,
        itemId: ITEM_TYPE.ORE_GOLD)
    },
    {
      NodeTypes.Mythril, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId: ITEM_TYPE.ORE_MYTHRIL)
    },
    {
      NodeTypes.Adamantite, new NodeData(
        color: new Color(0.5f, 0.5f, 0.5f),
        health:  100,
        itemId:ITEM_TYPE.ORE_ADAMANTITE)
    },
  };
}