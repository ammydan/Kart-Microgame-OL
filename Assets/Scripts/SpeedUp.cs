using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpeedUp : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        Debug.Log(other.transform.root.tag+" the collider object "+other.tag);
        GameObject obj = other.transform.root.gameObject;
        string str = other.transform.root.tag;
        if(str.Equals("Player")){
            //speed up 
            obj
            .GetComponent<UnityStandardAssets.Vehicles.Car.PlayerMovementController>()
            .SpeedUp();
            Debug.Log("speed up!!!!!");

            //destroy the pickup gameobject
            // Destroy(this.gameObject);
            // CmdDisappear();
            NetworkServer.Destroy(this.gameObject.transform.root.gameObject);
        }
    }

}
