using System;
using UnityEngine;

public class BuildingBlock : MonoBehaviour, IDamagable
{
    private BuildingBlockAnchor[] _anchors;
    private RaycastHit[] _hits;

    private Animator _animator;

    private int health = 100;
    
    public void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _anchors = GetComponentsInChildren<BuildingBlockAnchor>();
        
        var box = transform.GetChild(0).GetChild(0);
        var collider = box.GetComponent<Collider>();
        HitboxSystem.ColliderHashToDamagable.Add(collider.GetInstanceID(), this);
    }
    
    public void Start()
    {
        for (int i = 0; i < _anchors.Length; i++)
        {
            var anchor = _anchors[i];

            var transform1 = anchor.transform;
            var position = transform1.root.GetChild(0).position;
            _hits = Physics.RaycastAll(position, -transform1.right, (Vector3.Distance(transform1.position, position)), 1 >> LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Collide);

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


    public void TakeDamage(IDamager player, Vector3 hitDirection, TOOL_TYPE toolType, int damage)
    {
        health -= damage;
        Debug.Log($"{player} hit BuildingBlock for {damage} ({health}/100)");

        if (health <= 0)
        {
            Death(hitDirection);
        }
    }

    private void Death(Vector3 hitDirection)
    {
        Debug.Log("BuildingBlock has died");
        var position = transform.position;
        var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
        var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
        var itemTemplate = Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
        var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[ITEM_TYPE.BUILDING_FOUNDATION]];

        var item = Instantiate(itemPrefab, Vector3.zero,
            Quaternion.identity, itemTemplate.transform);

        item.transform.localPosition = Vector3.zero;
    
        itemTemplate.GetComponent<Item>().SetItemType(ITEM_TYPE.BUILDING_FOUNDATION);

        // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
        hitDirection.y = 1;
        var rigidBody = itemTemplate.GetComponent<Rigidbody>();
        rigidBody.linearDamping = 0.5f;
        rigidBody.AddForce(hitDirection * 10f, ForceMode.Impulse);
        
        Destroy(gameObject);
    }
}
