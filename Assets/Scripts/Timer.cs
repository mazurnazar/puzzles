using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public bool timerIsRunning = false;
    public float time = 0;
    public string timeText = "";

    private void Update()
    {
        if(timerIsRunning)
        {
            time += Time.deltaTime;
            DisplayTime();
        }
    }
    void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
