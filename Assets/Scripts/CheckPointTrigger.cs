using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    // public Rigidbody triggerBody; 
    // public UnityEvent onTriggerEnter;


    void OnTriggerEnter(Collider other){
       // print("trigger");
        var hitRb = other.attachedRigidbody;
        print(hitRb);

        GameObject test=  hitRb.gameObject;
        test.GetComponent<MonoRaceObjective>().FinishOneLap();

        // Add EndUI for this game to player
        GameObject EndUI = this.transform.Find("End_Scene").gameObject;
        test.GetComponent<GameInfo>().SetEndUI(EndUI);
        
        // Add Timer for this game to player
        GameObject Timer = this.transform.Find("TimerController").gameObject;
        test.GetComponent<GameInfo>().SetTimer(Timer);
    }
}