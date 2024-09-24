using System;
using System.Linq;
using UnityEngine;

public struct DayState
{
    public float currentTime;
    public int currentDay;
    public float currentHour;

    public bool HasChanged;
}
public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }
    
    public const int DayInSeconds = 30;
    private Light _sunLight;

    public DayState dayState;

    public Action<int> DayStarted;
    public Action<int> NightStarted;
    private void Awake()
    {
        _sunLight = gameObject.scene
            .GetRootGameObjects()
            .First(t => t.name == "Sun")
            .GetComponent<Light>();

        Instance = this;
    }

    private void Start()
    {
        DayTime();
    }

    private void FixedUpdate()
    {
        ref var state = ref dayState;

        if (state.HasChanged)
        {
            state.HasChanged = false;
        }

        state.currentTime += Time.fixedDeltaTime;

        if (state.currentTime > DayInSeconds) state.currentTime = 0;
        
        // // (30 / 120) = 2 AM
        var timeInHours = (state.currentTime / DayInSeconds) * 24;
        state.currentHour = timeInHours;
        state.HasChanged = true;
        //
        // var rotation = _sunLight.transform.eulerAngles;
        // // // 7 AM(0) -> PEAKS AT 1 PM(90) -> SETS AT 7 PM(180)
        // //
        // rotation.x = (timeInHours * 15) - 105;
        // //
        // // if (rotation.x > 360)
        // // {
        // //     rotation.x -= 360;
        // // }
        // //
        // // // 240
        // // if (rotation.x < 0)
        // // {
        // //     rotation.x += 360;
        // // }

        // var newRotation = Vector3.Lerp(new Vector3(0, rotation.y, rotation.z), new Vector3(360, rotation.y, rotation.z), currentTime / dayInSeconds);
        // Debug.Log($"{currentTime}/{dayInSeconds} - {rotation.x}");

        _sunLight.transform.Rotate((Time.fixedDeltaTime/DayInSeconds)*360,0, 0);
    }

    // Y Angle to 0
    // Starts at 7 AM
    private void DayTime() 
    {
        Debug.Log("Day Time!");
        ref var state = ref dayState;
        
        state.currentTime = (DayInSeconds / 24f) * 7;
        state.currentHour = 7;
        state.currentDay++;
        state.HasChanged = true;

        _sunLight.transform.rotation = Quaternion.Euler(Vector3.zero);

        DayStarted?.Invoke(state. currentDay);

        Invoke(nameof(NightTime), (DayInSeconds / 24f) * 15);
    }
    // Starts at 10 PM
    private void NightTime()
    {
        Debug.Log("Night Time!");
        ref var state = ref dayState;
        state.currentTime = (DayInSeconds / 24f) * 22;
        state.currentHour = 22;
        state.HasChanged = true;

        _sunLight.transform.rotation = Quaternion.Euler(new Vector3(225, 0 ,0));

        NightStarted?.Invoke(state.currentDay);
        
        Invoke(nameof(DayTime), (DayInSeconds / 24f) * 9);
    }
    
    
}