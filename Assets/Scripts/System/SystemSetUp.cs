using UnityEngine;

public class SystemSetUp : MonoBehaviour 
{
    private void Awake() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  
    }    
}