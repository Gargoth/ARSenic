using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManagerScript : Singleton<GameManagerScript>
{
    public GameObject ObjectContainer { get; private set; }
    private int currentIndex = 0; // Index to keep track of the current string in the list

    void Start()
    {
        BindObjectContainer();
        ObjectContainer?.SetActive(false);
    }

    public void BindObjectContainer()
    {
        ObjectContainer = GameObject.FindWithTag("Object Container");
    }

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
        // TO DO: don't spawn when the dialogue is already active
        string topicName = SceneManager.GetActiveScene().name;
		currentIndex = 0;
        
        if (!PersistentDataContainer.Instance.popupCanvasPrefab)
        {
            PersistentDataContainer.Instance.popupCanvasPrefab = Resources.Load<GameObject>("Prefabs/Popup Canvas");
        }
        
        GameObject newPopup = Instantiate(PersistentDataContainer.Instance.popupCanvasPrefab, Vector3.zero, Quaternion.identity);
        TextMeshProUGUI textUI = newPopup.transform.Find("Popup Text").GetComponent<TextMeshProUGUI>();
        textUI.text = PersistentDataContainer.Instance.TopicTutorial[topicName][currentIndex];
    }
    public void TextChanger(TextMeshProUGUI displayText)
    {
        string topicName = SceneManager.GetActiveScene().name;

        if (topicName != "")
        {
            // Increment the current index, wrapping around to the start if necessary
            currentIndex = (currentIndex + 1) % PersistentDataContainer.Instance.TopicTutorial[topicName].Count;
			displayText.text = PersistentDataContainer.Instance.TopicTutorial[topicName][currentIndex];
        }
    }
    
}