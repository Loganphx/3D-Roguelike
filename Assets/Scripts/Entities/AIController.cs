using Pathfinding;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update

    private AIPath _aiPath;

    private void Awake()
    {
        _aiPath = GetComponent<AIPath>();
    }


    private void FixedUpdate()
    {
        _aiPath.destination = PlayerPool.Players[0].Transform.position;
    }
}
