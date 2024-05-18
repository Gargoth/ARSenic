using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DelayedTrigger))]
public class FailedLevel : MonoBehaviour
{
    [Tooltip("Put text to display here!")] [SerializeField] string popupText;
    GameObject failedLevelPrefab;
    GameObject failedLevel;
    void Start()
    {
        failedLevelPrefab = Resources.Load<GameObject>("Prefabs/Failed Level");
        GetComponent<DelayedTrigger>().TriggerEvent.AddListener(SpawnPopup);
    }

    void SpawnPopup()
    {
        failedLevel = Instantiate(failedLevelPrefab);
        TextMeshProUGUI popupTextUI = failedLevel.transform.Find("Page 4").Find("PopupText").GetComponent<TextMeshProUGUI>();
        popupTextUI.text = popupText;
    }
}
