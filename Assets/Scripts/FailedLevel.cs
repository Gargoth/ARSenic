using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FailedLevel : MonoBehaviour
{
    [Tooltip("Put text to display here!")] [SerializeField] string popupText;
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

    void SpawnPopup(string popupText)
    {
        failedLevel = Instantiate(failedLevelPrefab);
        TextMeshProUGUI popupTextUI = failedLevel.transform.Find("Page 4").Find("PopupText").GetComponent<TextMeshProUGUI>();
        popupTextUI.text = popupText;
    }
    
    IEnumerator FinishingLevel(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        SpawnPopup(popupText);
        Debug.Log("Ended Finishing Level Coroutine");
    }
}
