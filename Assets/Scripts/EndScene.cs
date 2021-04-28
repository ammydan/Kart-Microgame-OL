using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScene : MonoBehaviour
{
    [SerializeField] private Text isWin;
    [SerializeField] private GameObject[] players;
    [SerializeField] private Button back;
    public TMP_Text[] playername = new TMP_Text[4];
    public TMP_Text[] playertime = new TMP_Text[4];
    public GameObject[] playerActive = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        //EndS = GameObject.Find ("End Scene");
        //EndS.gameObject.SetActive(true);
        //print("UIshow at start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playerWin()
    {
        isWin.gameObject.SetActive(true);
    }
    public void finishGame()
    {
        players[0].SetActive(true);
    }
    public void canGetBack()
    {
        back.gameObject.SetActive(true);
    }
    public void updateEndUI(string[] inputs){
        int len = inputs.Length;
        for(int i=0;i<len;i++){
            string [] str = inputs[i].Split('_');
            playername[i].text = str[0];
            playertime[i].text = str[1];
            playerActive[i].SetActive(true);
        }
    }
}
