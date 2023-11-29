using System.Collections.Generic;
using UnityEngine;

namespace Muck.Trees
{
  internal class TreeData
  {
    public readonly int Health;
    public readonly ITEM_TYPE DropItemId;

    public TreeData(int health, ITEM_TYPE itemId)
    {
      Health = health;
      DropItemId = itemId;
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
    }

    public void Start()
    {
      _nodeData = _trees[_nodeType];
      _nodeState = new TreeState()
      {
        CurrentHealth = _nodeData.Health
      };
    }

    public void TakeDamage(IDamager player, Vector3 hitDirection, int damage)
    {
      ref var nodeState = ref _nodeState;
      if (nodeState.CurrentHealth <= 0) return;

      nodeState.CurrentHealth -= damage;
      Debug.Log($"Hit {_nodeType} Tree for {damage} => {nodeState.CurrentHealth} / {_nodeData.Health}");

      if (nodeState.CurrentHealth <= 0)
        Death(hitDirection);
    }

    private void Death(Vector3 hitDirection)
    {
      Debug.Log($"{_nodeType} Tree died");
      var position = transform.position;
      var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
      var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
      var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
      var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[_nodeData.DropItemId]];

      var item = Instantiate(itemPrefab, Vector3.zero,
        Quaternion.identity, itemTemplate.transform);

      item.transform.localPosition = Vector3.zero;
    
      itemTemplate.GetComponent<Item>().SetItemType(_nodeData.DropItemId);
      
      // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
      hitDirection.y = 1;
      itemTemplate.GetComponent<Rigidbody>().AddForce(hitDirection * 3f, ForceMode.Impulse);

      var logTransform = transform.Find("Log");
      logTransform.gameObject.SetActive(false);
    }

    private static readonly Dictionary<TreeTypes, TreeData> _trees = new Dictionary<TreeTypes, TreeData>()
    {
      {
        TreeTypes.Tree, new TreeData(
          health: 10,
          itemId: ITEM_TYPE.WOOD)
      },
      {
        TreeTypes.Birch, new TreeData(
          health: 25,
          itemId: ITEM_TYPE.WOOD_BIRCH)
      },
      {
        TreeTypes.Fir, new TreeData(
          health: 50,
          itemId: ITEM_TYPE.WOOD_FIR)
      },
      {
        TreeTypes.Oak, new TreeData(
          health:  100,
          itemId: ITEM_TYPE.WOOD_OAK)
      },
      {
        TreeTypes.DarkOak, new TreeData(
          health: 250,
          itemId: ITEM_TYPE.WOOD_DARKOAK)
      },
    };
  }
}