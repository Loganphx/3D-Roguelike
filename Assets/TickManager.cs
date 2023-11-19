using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;
    public        int         Tick  = 0;
    public        int         Frame = 0;

    public void Awake()
    {
        Instance = this;
    }
    public void FixedUpdate()
    {
        Tick++;
    }
    
    public void Update()
    {
        Frame++;
    }
}
