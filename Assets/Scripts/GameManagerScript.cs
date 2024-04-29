using System;
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
}