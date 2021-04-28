using UnityEngine;
//using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    
    [SerializeField] private GameObject EndUI = null;
    [SerializeField] private bool isFirst = true;
    [SerializeField] private string[] playerName = new string[4];
    [SerializeField] private string[] playerTime = new string[4];
    // Pointer to next position to fill the data
    [SerializeField] private int resultIndex = 0;
    // Pointer to next position to filled updated data
    //[SerializeField] private int updatedIndex= 0;
    [SerializeField] private bool isDashBoardInfoUpdated = false;
    [SerializeField] private GameObject Timer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    //void Update()
    //{

    //}
    public string[] getNextUpdate(){
        setUpdateNotNeeded();
        int resSize = resultIndex;
        if(resSize <= 0){
            return null;
        }
        string[] res = new string[resSize];
        int count = 0;
        while(count < resultIndex){
            res[count] = playerName[count] + "_" + playerTime[count];
            count++;
        }
        return res;
    }

    public void addDashBoardData(string name, string time){
        if(resultIndex > 4){
            return;
        }
        playerName[resultIndex] = name;
        playerTime[resultIndex] = time;
        resultIndex++;
        setUpdateNeeded();
    }

    public bool isDashBoardNeedUpdate(){
        return isDashBoardInfoUpdated;
    }
    public void setUpdateNeeded(){
        isDashBoardInfoUpdated = true;
    }
    public void setUpdateNotNeeded(){
        isDashBoardInfoUpdated = false;
    }
    public bool isFirstFinish()
    {
        return isFirst;
        //isWin.gameObject.SetActive(true);
    }
    public void WinnerShowed()
    {
        isFirst = false;
        //players[0].SetActive(true);
    }
    public GameObject getEndUI()
    {
        return EndUI;
        //back.gameObject.SetActive(true);
    }
    public void SetEndUI(GameObject EndUI){
        this.EndUI = EndUI;
    }
    public GameObject getTimer()
    {
        return Timer;
    }
    public void SetTimer(GameObject Timer){
        this.Timer = Timer;
    }

    public string getPlayerName(){
        return gameObject.GetComponent<PlayerName>().playerName;
    }

    public string getPlayerTimer(){
        return Timer.GetComponent<TimerController>().curTime;
    }
}