using System;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Collider _collider;
    private void OnDrawGizmos()
    {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(transform.position, transform.forward*5);
    //     Gizmos.DrawWireMesh(_mesh, _collider.transform.position, _collider.transform.rotation, _collider.transform.localScale);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.parent.position + new Vector3(0,.5f,0), _collider.bounds.extents.x);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position + new Vector3(0,0.5f,0) + transform.forward * .18f * 2, _collider.bounds.extents.x);
        // Gizmos.DrawWireSphere(transform.parent.position + new Vector3(0,1.5f,0) + transform.forward * .18f * 2, _collider.bounds.extents.x);
        
        // this.Invoke();
    }
}
