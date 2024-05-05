using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionTargetScript : MonoBehaviour
{
    GameObject endCanvasPrefab;
    GameObject endCanvas;
    void Start()
    {
        endCanvasPrefab = Resources.Load<GameObject>("Prefabs/End Canvas");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision detected with Player");
            endCanvas = Instantiate(endCanvasPrefab);
        }
    }
}
