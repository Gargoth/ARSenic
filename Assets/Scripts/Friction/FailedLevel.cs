using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Shows the appropriate popupcanvas on failed level
/// </summary>
[RequireComponent(typeof(DelayedTrigger))]
public class FailedLevel : MonoBehaviour
{
    [Tooltip("Text to display on failed level")] [SerializeField]
    string popupText;

    GameObject failedLevelPrefab;
    GameObject failedLevel;

    void Start()
    {
        failedLevelPrefab = Resources.Load<GameObject>("Prefabs/Failed Level");
        GetComponent<DelayedTrigger>().TriggerEvent.AddListener(SpawnPopup);
        // Prevents spawning of both endcanvas and failedlevelcanvas at the same time
        GameObject.FindWithTag("Friction Target").GetComponent<FrictionTargetScript>().OnFinishLevel
            .AddListener(GetComponent<DelayedTrigger>().CancelTriggerCoroutine);
    }

    void SpawnPopup()
    {
        failedLevel = Instantiate(failedLevelPrefab);
        TextMeshProUGUI popupTextUI =
            failedLevel.transform.Find("Page 4").Find("PopupText").GetComponent<TextMeshProUGUI>();
        popupTextUI.text = popupText;
    }
}