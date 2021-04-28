using Mirror;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum GameType{Laps, Speed, Championship}
public class MonoRaceObjective : MonoBehaviour
{
    //public NetworkManagerLobby CurerntGameNetwork;
    public int LeftLaps;
    public GameType CurrentGameType;
    public bool isGameEnd;
    public bool isUIshowed;
    void Start()
    {
        isGameEnd = false;
        CurrentGameType = GameType.Speed;
        if(CurrentGameType == GameType.Laps) {
            LeftLaps = 3;
        } else if (CurrentGameType == GameType.Speed) {
            LeftLaps = 2;
        }
        isUIshowed = false;
    }


    public void FinishOneLap()
    {
        this.LeftLaps = Mathf.Max(LeftLaps - 1, 0);
    }

    public bool IsGameFinished()
    {
        return isGameEnd;
    }

    public bool IsUIDoneShowed(){
        return isUIshowed;
    }
    public void MarkUIShowed(){
        isUIshowed = true;
    }
     void Update()
    {
        if(LeftLaps == 0){
            isGameEnd = true;
            //NetworkManager.singleton.ServerChangeScene("EndScene");
        }
    }



}