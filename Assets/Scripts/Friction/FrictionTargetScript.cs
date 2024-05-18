using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DelayedTrigger))]
public class FrictionTargetScript : MonoBehaviour
{
    GameObject endCanvasPrefab;
    GameObject endCanvas;
    void Start()
    {
        endCanvasPrefab = Resources.Load<GameObject>("Prefabs/End Canvas");
        GetComponent<DelayedTrigger>().TriggerEvent.AddListener(FinishLevel);
    }
    
    void FinishLevel()
    {
        endCanvas = Instantiate(endCanvasPrefab);
        FrictionModuleManager.Instance.FinishLevel();
    }
}
