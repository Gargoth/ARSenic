using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DelayedTrigger))]
public class FrictionTargetScript : MonoBehaviour
{
    GameObject endCanvasPrefab;
    GameObject endCanvas;
    public UnityEvent OnFinishLevel { get; private set; }
    void Start()
    {
        OnFinishLevel = new UnityEvent();
        endCanvasPrefab = Resources.Load<GameObject>("Prefabs/End Canvas");
        GetComponent<DelayedTrigger>().TriggerEvent.AddListener(FinishLevel);
    }
    
    void FinishLevel()
    {
        Debug.Log("Finishing Level");
        OnFinishLevel.Invoke();
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("Popup Canvas"))
        {
            Destroy(canvas);
        }
        endCanvas = Instantiate(endCanvasPrefab);
        FrictionModuleManager.Instance.FinishLevel();
    }
}
