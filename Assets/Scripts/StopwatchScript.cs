using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class StopwatchScript : Singleton<StopwatchScript>
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float currentTime;
    bool isStopwatchRunning = true;

    public float GetTime()
    {
        return currentTime;
    }

    public void ToggleStopwatch(bool state)
    {
        isStopwatchRunning = state;
    }

    void Update()
    {
        if (timerText != null && isStopwatchRunning)
        {
            currentTime += Time.deltaTime;
            int minutes = (int)(currentTime / 60);
            int seconds = (int)currentTime % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
