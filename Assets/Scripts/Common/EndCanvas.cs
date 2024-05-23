using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using TMPro;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private GameObject Medal;
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
    void Start()
    {
        float time = Mathf.Round(StopwatchScript.Instance.GetTime() * 100) / 100;

        Debug.Log("Time is " + time);
        
        timerText.text = "Time: " + time + " seconds";

        if (time <= 60)
        {
            Debug.Log("Award is GOLD");
            Image medalImage = Medal.GetComponent<Image>();
            if (medalImage != null)
            {
                medalImage.sprite = gold;
            }
        }
        
        else if (time > 60 && time < 100)
        {
            Debug.Log("Award is SILVER");
            Image medalImage = Medal.GetComponent<Image>();
            if (medalImage != null)
            {
                medalImage.sprite = silver;
            }
        }
        
        else
        {
            Debug.Log("Award is BRONZE");
            Image medalImage = Medal.GetComponent<Image>();
            if (medalImage != null)
            {
                medalImage.sprite = bronze;
            }
        }
    }
    
}
