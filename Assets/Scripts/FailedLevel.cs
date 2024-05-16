using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedLevel : MonoBehaviour
{
    GameObject failedLevelPrefab;
    GameObject failedLevel;
    float targetDuration;
    Coroutine finishingLevelCoroutine;
    void Start()
    {
        targetDuration = FrictionStageManager.Instance.GetTargetDuration();
        failedLevelPrefab = Resources.Load<GameObject>("Prefabs/Failed Level");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && finishingLevelCoroutine == null)
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
                finishingLevelCoroutine = null;
            }
        }
    }
    IEnumerator FinishingLevel(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        failedLevel = Instantiate(failedLevelPrefab);
        Debug.Log("Ended Finishing Level Coroutine");
    }
}
