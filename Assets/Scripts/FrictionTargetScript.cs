using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionTargetScript : MonoBehaviour
{
    GameObject endCanvasPrefab;
    GameObject endCanvas;
    float targetDuration;
    Coroutine finishingLevelCoroutine;
    void Start()
    {
        targetDuration = FrictionStageManager.Instance.GetTargetDuration();
        endCanvasPrefab = Resources.Load<GameObject>("Prefabs/End Canvas");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger enter");
            finishingLevelCoroutine = StartCoroutine(FinishingLevel(targetDuration));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger exit");
            if (finishingLevelCoroutine != null)
            {
                StopCoroutine(finishingLevelCoroutine);
            }
        }
    }

    void FinishLevel()
    {
        endCanvas = Instantiate(endCanvasPrefab);
    }

    IEnumerator FinishingLevel(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        endCanvas = Instantiate(endCanvasPrefab);
        Debug.Log("Ended Finishing Level Coroutine");
    }
}
