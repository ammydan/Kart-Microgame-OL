using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpeedDown : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.root.tag + " the collider object " + other.tag);
        GameObject obj = other.transform.root.gameObject;
        string str = other.transform.root.tag;
        if (str.Equals("Player"))
        {
            //slow down
            obj
            .GetComponent<UnityStandardAssets.Vehicles.Car.PlayerMovementController>()
            .Slowdown();
            NetworkServer.Destroy(this.gameObject.transform.root.gameObject);
        }
    }

}
