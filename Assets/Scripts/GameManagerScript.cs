using System;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : Singleton<GameManagerScript>
{
    public static void DoLog(string log)
    {
        Debug.Log(log);
    }
    
    public static void MoveToScene(string sceneName)
    {
        Physics.gravity = Vector3.down * 9.81f;
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void SetSelectedLevel(int num)
    {
        PersistentDataContainer.Instance.selectedLevel = num;
    }

    public void SpawnPopupDialog(string message)
    {
        if (!PersistentDataContainer.Instance.popupCanvasPrefab)
        {
            PersistentDataContainer.Instance.popupCanvasPrefab = Resources.Load<GameObject>("Prefabs/Popup Canvas");
        }

        GameObject newPopup = Instantiate(PersistentDataContainer.Instance.popupCanvasPrefab, Vector3.zero, Quaternion.identity);
        TextMeshProUGUI textUI = newPopup.transform.Find("Popup Text").GetComponent<TextMeshProUGUI>();
        textUI.text = message;
    }
}