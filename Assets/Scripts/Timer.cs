using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    private bool timerIsRunning = false;
    private float time = 0;
    private string timeText = "";

    public bool TimerIsRunning { get => timerIsRunning; set { timerIsRunning = value; } }
    
    public float _Time { get => time;  set { time = value; } }

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
