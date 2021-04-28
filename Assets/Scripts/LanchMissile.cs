using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LanchMissile : NetworkBehaviour
{
    public GameObject myMissile;
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
            //instantiate a missile gameObject.
            GameObject missile = Instantiate(myMissile,this.transform.position+new Vector3(0,1,0),obj.transform.rotation);
            Debug.Log("test the rotation!! "+transform.rotation);
            missile.GetComponent<Missile>().avoidPlayer = obj;
            Vector3 scaleChange = new Vector3(-0.7f, -0.7f, -0.7f);
            missile.transform.localScale+=scaleChange;
            Debug.Log("Instantiate!!!");
            NetworkServer.Spawn(missile);
            NetworkServer.Destroy(this.gameObject.transform.root.gameObject);
        }
    }
}
