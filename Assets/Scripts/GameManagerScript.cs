using System;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerScript : Singleton<GameManagerScript>
{
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
        // TODO: HELLO CEEJ IM SORRY IDK IF THERE'S A BETTER WAY TO DO THIS BUT IT WORKS HUHU
        string currentSceneName = SceneManager.GetActiveScene().name;

        string topicName = currentSceneName switch
        {
            "FrictionModule" => "friction",
            _ => "",
            // TODO(Lara): tutorial for Gravity
            // populate when new scenes come up
        };

        if (topicName != "")
        {
            displayText.text = PersistentDataContainer.Instance.TopicTutorial[topicName][currentIndex];

            // Increment the current index, wrapping around to the start if necessary
            currentIndex = (currentIndex + 1) % PersistentDataContainer.Instance.TopicTutorial[topicName].Count;
        }
    }
    
}