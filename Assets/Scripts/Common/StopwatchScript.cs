using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Handles the stopwatch timer that is displayed in the UI for the levels
/// </summary>
public class StopwatchScript : Singleton<StopwatchScript>
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float currentTime;
    bool isStopwatchRunning = false;

    /// <summary>
    /// Getter function for the current time
    /// </summary>
    /// <returns>Current time</returns>
    public float GetTime()
    {
        return currentTime;
    }

    /// <summary>
    /// Toggles the stopwatch state.
    /// </summary>
    /// <param name="isRunning">True if on, False if off</param>
    public void ToggleStopwatch(bool isRunning)
    {
        isStopwatchRunning = isRunning;
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
