using Mirror;
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

    public class RaceObjective : NetworkBehaviour
{
    //[Header("Settings")]
    //[SerializeField] private int MaxLeftLaps = 2;
    //[SerializeField] private int DeductPreArrive = 1;
    //[SerializeField] private bool GameStartState = false;
    //[SerializeField] private bool GameEndState = false;
    //[SerializeField] private int 
    /*public override void OnStartLocalPlayer()
    {
        print("setlocal");
        base.OnStartLocalPlayer();
        gameObject.name = "Local";
    }*/

    [Server]
    public void SetPalyerFinish(string s)
    {
        print("SetPalyerFinish from server");
        RpcAnnounceWinnerShow();
        RpcUpdateDashBoard(s);
        // Add in case if server did not change
        //gameObject.GetComponent<GameInfo>().WinnerShowed();
    }

    
    [ClientRpc]
    public void RpcAnnounceWinnerShow(){
        print("RpcAnnounceWinnerShow from server");
        GameObject currPlayer = GameObject.Find("Local");
        /*if(! hasAuthority) {
            print("Not local player return");
            return;
        }*/
        currPlayer.GetComponent<GameInfo>().WinnerShowed();
    }
    
    [ClientRpc]
    public void RpcUpdateDashBoard(string s){
        print(s);
        string[] words = s.Split('_');
        GameObject currPlayer = GameObject.Find("Local");
        currPlayer.GetComponent<GameInfo>().addDashBoardData(words[0], words[1]);
    }

    
    
    [Command]
    private void CmdEndGame(string s)
    {
        print("CmdEndGame from client");
        SetPalyerFinish(s);
    } 

    

    [ClientCallback]
    public void Update()
    {
        if(! hasAuthority) {return;}
        gameObject.name = "Local";
        if(gameObject.GetComponent<MonoRaceObjective>().IsGameFinished() && !gameObject.GetComponent<MonoRaceObjective>().IsUIDoneShowed()) 
        {
            print("gameFinished");
            // If it is First finished
            if(gameObject.GetComponent<GameInfo>().isFirstFinish()){
                print("isFirst");
                // Maybe Not needed
                gameObject.GetComponent<GameInfo>().WinnerShowed();
                // EndGame and let server tell all other client winner showed TODO: Pass FinishTime and name INFO in Player GameInfo
                string cur = gameObject.GetComponent<GameInfo>().getPlayerName() + "_" + gameObject.GetComponent<GameInfo>().getPlayerTimer();
                CmdEndGame(cur);
                // Active UI logic for Winner
                gameObject.GetComponent<GameInfo>().getEndUI().gameObject.SetActive(true);
                print("ACTIVEUI");
                gameObject.GetComponent<GameInfo>().getEndUI().GetComponent<EndScene>().playerWin();
                print("setPlayerWin");
                gameObject.GetComponent<MonoRaceObjective>().MarkUIShowed();
                
            }else{
                // Not firstFinish as first
                //Tell everyOne Finish,TODO: Pass FinishTime and name INFO in Player GameInfo
                string cur = gameObject.GetComponent<GameInfo>().getPlayerName() + "_" + gameObject.GetComponent<GameInfo>().getPlayerTimer();
                CmdEndGame(cur);
                // Active UI logic for NonWinner
                gameObject.GetComponent<GameInfo>().getEndUI().gameObject.SetActive(true);
                print("ACTIVEUI Not First");
                gameObject.GetComponent<MonoRaceObjective>().MarkUIShowed();
            }
        }
        //TODO: logic update UI when dashBoard data update
        if(gameObject.GetComponent<MonoRaceObjective>().IsUIDoneShowed() && gameObject.GetComponent<GameInfo>().isDashBoardNeedUpdate()){
            //updatde 
            string[] temp = gameObject.GetComponent<GameInfo>().getNextUpdate();
            gameObject.GetComponent<GameInfo>().getEndUI().GetComponent<EndScene>().updateEndUI(temp);
        }
    }

    }
