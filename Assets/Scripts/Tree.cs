using System.Collections.Generic;
using UnityEngine;

namespace Muck.Trees
{
    internal class TreeData
    {
        public Color Color;
        public int   Health;
        public int   Drops;
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
        
        public void TakeDamage(IPlayer player, float damage)
        {
            ref var nodeState = ref _nodeState;
            if(nodeState.CurrentHealth <= 0) return;
            
            nodeState.CurrentHealth -= (int) damage;
            Debug.Log($"Hit {_nodeType} Tree for {damage} => {nodeState.CurrentHealth} / {_nodeData.Health}");
            
            if(nodeState.CurrentHealth <= 0)
                Death();
        }

        private void Death()
        {
            Debug.Log($"{_nodeType} Tree died");
            var logTransform = transform.Find("Log"); 
            logTransform.gameObject.SetActive(false);
        }

        private static Dictionary<TreeTypes, TreeData> _trees = new Dictionary<TreeTypes, TreeData>()
        {
            {
                TreeTypes.Tree, new TreeData()
                {
                    Health = 10,
                }
            },
            {
                TreeTypes.Birch, new TreeData()
                {
                    Health = 25,
                }
            },
            {
                TreeTypes.Fir, new TreeData()
                {
                    Health = 50,
                }
            },
            {
                TreeTypes.Oak, new TreeData()
                {
                    Health = 100,
                }
            },
            {
                TreeTypes.DarkOak, new TreeData()
                {
                    Health = 250,
                }
            },
        };
    }
}