using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Contains functions that change the entire game's state (i.e. changing the scene)
/// </summary>
public class GameManagerScript : Singleton<GameManagerScript>
{
    public GameObject ObjectContainer { get; private set; }
    private int currentIndex = 0; // Index to keep track of the current string in the list
    
    [SerializeField] List<GameObject> popUps;

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

    /// <summary>
    /// Spawns a popup dialog with the specified message.
    /// </summary>
    /// <param name="message">The message to display in the popup dialog.</param>
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

    /// <summary>
    /// Updates the text of the displayText parameter with the next topic tutorial message for the current scene.
    /// If there are no more topic tutorial messages, the currentIndex is wrapped around to the start.
    /// </summary>
    /// <param name="displayText">The TextMeshProUGUI component to update with the next topic tutorial message.</param>
    public void TextChanger(TextMeshProUGUI displayText)
    {
        string topicName = SceneManager.GetActiveScene().name;
        
        if (topicName != "")
        {
            popUps[currentIndex].SetActive(false);
            // Increment the current index, wrapping around to the start if necessary
            currentIndex = (currentIndex + 1) % PersistentDataContainer.Instance.TopicTutorial[topicName].Count;
			displayText.text = PersistentDataContainer.Instance.TopicTutorial[topicName][currentIndex];
            // popUps[currentIndex].SetActive(true);
        }
    }
}