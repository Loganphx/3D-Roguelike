using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public static class ResourceManager
    {
        public static Dictionary<int, BuildingBlock> Builds = new Dictionary<int, BuildingBlock>();

        public static (IDamagable closestPlayer, Vector3 closestPlayerPosition, float closestDistance) GetClosestBuild(Vector3 position)
        {
            var closestDistance = float.PositiveInfinity;
            Vector3 closestPlayerPosition = Vector3.negativeInfinity;
            IDamagable closestPlayer = null;
        
            // Debug.Log(Players.Count);
            foreach (var build in Builds)
            {
                // if (!build..IsDead)
                {
                    var playerPosition = build.Value.Transform.position;
                    var distance = Vector3.Distance(build.Value.Transform.position, position);
                    // Debug.Log(distance);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayerPosition = playerPosition;
                        closestPlayer   = build.Value;
                    }
                }
                // else Debug.LogWarning($"Player is dead {player}");
        
            }

            return (closestPlayer, closestPlayerPosition, closestDistance);
        }
    }
    public class BuildingBlock : MonoBehaviour, IDamagable
    {
        private BuildingBlockAnchor[] _anchors;
        private RaycastHit[] _hits;

        private Animator _animator;

        private int health = 100;
    
        public void Awake()
        {
            ResourceManager.Builds.Add(GetHashCode(), this);
            _animator = transform.GetComponent<Animator>();
            _anchors = GetComponentsInChildren<BuildingBlockAnchor>();
            
            GetComponent<Damagable>().Initialize(new LootTable(new List<LootTableItem>()
            {
                new LootTableItem()
                {
                    ItemType = ITEM_TYPE.BUILDING_FOUNDATION,
                    maxAmount = 1,
                    minAmount = 1,
                    dropChance = 100,
                    useStats = false
                }
            })
            ,100);
        }
    
        public void Start()
        {
            for (int i = 0; i < _anchors.Length; i++)
            {
                var anchor = _anchors[i];

                var transform1 = anchor.transform;
                var position = transform1.root.GetChild(0).position;
                _hits = Physics.RaycastAll(position, -transform1.right, (Vector3.Distance(transform1.position, position)), 
                    1 >> LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Collide);

                if (_hits.Length > 0)
                {
                    for (int j = 0; j < _hits.Length; j++)
                    {
                        ref var hitData = ref _hits[j];
                        if (hitData.transform.root == transform1.root) continue;
                        // Debug.Log(anchor.gameObject, anchor.gameObject);
                        // Debug.Log(hitData.collider.gameObject, hitData.collider.gameObject);
                        anchor.gameObject.SetActive(false);
                        var otherAnchor = hitData.transform.GetComponentInParent<BuildingBlockAnchor>();
                        if(otherAnchor != null) otherAnchor.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_anchors == null) return;
            for (int i = 0; i < _anchors.Length; i++)
            {
                var anchor = _anchors[i];
                var transform1 = anchor.transform;
                var position = transform1.root.GetChild(0).position;
                _hits = Physics.RaycastAll(position, -transform1.right, (Vector3.Distance(transform1.position, position)), LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Collide);

                if (_hits.Length > 0)
                {
                    for (int j = 0; j < _hits.Length; j++)
                    {
                        ref var hitData = ref _hits[j];
                        if (hitData.transform.root == transform1.root) continue;
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(position, hitData.point);
                    }
                }
            }
        }


        private void OnDestroy()
        {
            Debug.Log("OnDestroy");
            ResourceManager.Builds.Remove(GetHashCode());
        }

        private void OnDisable()
        {
            ResourceManager.Builds.Remove(GetHashCode());
        }

        public Transform Transform => transform;

        public void OnHit(IDamager player, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage)
        {
            GetComponent<Damagable>().OnHit(player, hitDirection, hitPosition, toolType, damage);
        }
    }
}
