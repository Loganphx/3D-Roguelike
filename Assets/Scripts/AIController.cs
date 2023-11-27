using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update

    private AIPath _aiPath;
    void Start()
    {
        _aiPath = GetComponent<AIPath>();
    }


    private void FixedUpdate()
    {
        _aiPath.destination = PlayerPool.Players[0].Transform.position;
    }
}
