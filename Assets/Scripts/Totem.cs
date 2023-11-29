using UnityEngine;
using Random = System.Random;

public class Totem : MonoBehaviour, IInteractable, IHoverable
{
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Interact(IPlayer player)
    {
        Debug.Log("Interacting with totem");

        var transform1 = transform;
        var forward = transform1.forward;
        var right = transform1.right;
        var position1 = transform1.position;
        Vector3[] positions = new[] {
            position1 + (forward * 2) + (Vector3.up * 1.5f),
            position1 + (forward * 1 + right * 2) + (Vector3.up * 1.5f),
            position1 + (forward * 1 - right * 2) + (Vector3.up * 1.5f)
        };

        foreach (var position in positions)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
        }

        // GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = transform.position + (Vector3.up * 0.45f);
        var powerupPrefab = PrefabPool.Prefabs[PowerupPool.PowerupPrefabs[(POWERUP_TYPE)new Random().Next((int)(POWERUP_TYPE.NULL+1), (int)(POWERUP_TYPE.LEGENDARY+1))]];
        var powerup = Instantiate(powerupPrefab, transform.position + (Vector3.up * 0.45f), Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void OnHoverEnter(IPlayer player)
    {
        _outline.enabled = true;
    }

    public void OnHoverExit(IPlayer player)
    {
        _outline.enabled = false;
    }
}
