using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Missile : NetworkBehaviour
{
    private float passTime = 0;
    private float liveTime = 3;
    private float mySpeed = 25;

    public GameObject avoidPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(passTime>=liveTime){
            // Debug.Log("destroy!!!"+passTime);
            NetworkServer.Destroy(gameObject);
            // DestroyImmediate(gameObject);
        }else{
            // Debug.Log("前进方向！"+transform.forward);
            transform.Translate(transform.forward* mySpeed * Time.deltaTime,Space.Self);
            // Debug.Log(transform.forward* mySpeed * Time.deltaTime+" delta time:"+Time.deltaTime);
            passTime+=Time.deltaTime;
            // Debug.Log("continue passtime:"+passTime+" "+liveTime+" "+(passTime>=liveTime));
        }
        
    }
    void OnTriggerEnter(Collider other){
        Debug.Log(other.transform.root.tag+" the collider object "+other.tag+" slow down");
        GameObject obj = other.transform.root.gameObject;
        string str = other.transform.root.tag;
        if(str.Equals("Player")&&obj!=avoidPlayer){
            //slow down!!
            Debug.Log("Slow down");
            obj
            .GetComponent<UnityStandardAssets.Vehicles.Car.PlayerMovementController>()
            .Slowdown();
            NetworkServer.Destroy(this.gameObject.transform.root.gameObject);
        }
    }
}
