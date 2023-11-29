using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    private BuildingBlockAnchor[] _anchors;
    private RaycastHit[] _hits;
    public void Awake()
    {
        _anchors = GetComponentsInChildren<BuildingBlockAnchor>();
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
}
