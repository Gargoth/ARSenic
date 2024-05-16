using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class StopwatchScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float currentTime;
    
    void Start()
    {
    }

    void Update()
    {
        if (timerText)
        {
            currentTime += Time.deltaTime;
            int minutes = (int)(currentTime / 60);
            int seconds = (int)currentTime % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
