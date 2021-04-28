using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerName : NetworkBehaviour
{
    public TMP_Text displayName;
    [SyncVar(hook=nameof(changeMyName))]

    public string playerName="default";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void changeMyName(string oldValue,string newValue){
        Debug.Log("in the NamePlayer "+newValue);
        playerName = newValue;
        Debug.Log(playerName);
        displayName.text = playerName;
    }
}
