using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;
    public        int         Tick;
    public        int         Frame;

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
