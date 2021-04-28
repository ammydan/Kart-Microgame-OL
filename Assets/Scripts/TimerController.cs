using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
	public static TimerController instance;

	public Text timeCounter;
    public string curTime = "00:00.00";
	private TimeSpan timePlaying;
	private bool timerGoing;

	private float elapsedTime;

	[SerializeField] private int seconds = 3;

	private void Awake()
	{	
		instance = this;
	}
    // Start is called before the first frame update
    void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        timerGoing = false;

        BeginTimer();
    }

    // Update is called once per frame
    public void BeginTimer()
    {
        timerGoing = true;
        //startTime = Time.time;
        elapsedTime = 0f;

        StartCoroutine(OnWaitMethod());
        
    }

    public void EndTimer()
    {
    	timerGoing = false;
    }

    private IEnumerator OnWaitMethod()
    {
    	yield return new WaitForSeconds(seconds);
    	StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
    	while(timerGoing)
    	{
    		elapsedTime += Time.deltaTime;
    		timePlaying = TimeSpan.FromSeconds(elapsedTime);
    		string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");

            curTime = timePlaying.ToString("mm':'ss'.'ff");
    		timeCounter.text = timePlayingStr;
    		//Debug.Log(timeCounter.text);

    		yield return null;
    	}
    }
}
