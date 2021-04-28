using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	Debug.Log("Here1");
        TimerController.instance.BeginTimer();
        Debug.Log("Here2");
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
