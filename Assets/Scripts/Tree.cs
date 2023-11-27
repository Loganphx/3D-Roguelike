using System.Collections.Generic;
using UnityEngine;

namespace Muck.Trees
{
    internal class TreeData
    {
        public Color Color;
        public int   Health;
        public string   DropPrefabPath;
    }
    
    public struct TreeState : IState
    {
        public int CurrentHealth;
    }
    
    public class Tree : MonoBehaviour, IDamagable
    {
        [SerializeField] private TreeTypes _nodeType;
        private                  TreeData  _nodeData;
        private                  TreeState _nodeState;

        public void Awake()
        {
            var logTransform = transform.Find("Log");
            if(logTransform == null)
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
            if(nodeState.CurrentHealth <= 0) return;
            
            nodeState.CurrentHealth -= damage;
            Debug.Log($"Hit {_nodeType} Tree for {damage} => {nodeState.CurrentHealth} / {_nodeData.Health}");
            
            if(nodeState.CurrentHealth <= 0)
                Death(hitDirection);
        }

        private void Death(Vector3 hitDirection)
        {
            Debug.Log($"{_nodeType} Tree died");
            var dropPrefab = PrefabPool.Prefabs[_nodeData.DropPrefabPath];
            var position = transform.position;
            var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
            var drop = Instantiate(dropPrefab, dropPosition, Quaternion.identity);
            
            hitDirection.y = 1;
            
            drop.GetComponent<Rigidbody>().AddForce(hitDirection * 3f, ForceMode.Impulse);

            var logTransform = transform.Find("Log");
            logTransform.gameObject.SetActive(false);
        }

        private static readonly Dictionary<TreeTypes, TreeData> _trees = new Dictionary<TreeTypes, TreeData>()
        {
            {
                TreeTypes.Tree, new TreeData()
                {
                    Health = 10,
                    DropPrefabPath = "Prefabs/Items/Item_Wood_Tree"
                }
            },
            {
                TreeTypes.Birch, new TreeData()
                {
                    Health = 25,
                    DropPrefabPath = "Prefabs/Items/Item_Wood_Birch"
                }
            },
            {
                TreeTypes.Fir, new TreeData()
                {
                    Health = 50,
                    DropPrefabPath = "Prefabs/Items/Item_Wood_Fir"
                }
            },
            {
                TreeTypes.Oak, new TreeData()
                {
                    Health = 100,
                    DropPrefabPath = "Prefabs/Items/Item_Wood_Oak"
                }
            },
            {
                TreeTypes.DarkOak, new TreeData()
                {
                    Health = 250,
                    DropPrefabPath = "Prefabs/Items/Item_Wood_DarkOak"
                }
            },
        };
    }
}