using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public void GetResourses()
    {
        FindObjectOfType<ResourceManager>().TakeReward(9000, 1000);
    }
}
