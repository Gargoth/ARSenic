using System;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerScript : Singleton<GameManagerScript>
{
    private List<string> textList = new List<string> { "First Text", "Second Text", "Third Text" }; // List of strings to cycle through
    private int currentIndex = 0; // Index to keep track of the current string in the list
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
    public void TextChanger(TextMeshProUGUI displayText)
    {
        displayText.text = textList[currentIndex];
        
        // Increment the current index, wrapping around to the start if necessary
        currentIndex = (currentIndex + 1) % textList.Count;
    }
}