using System;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : Singleton<GameManagerScript>
{
    async void Awake()
    {
        try
        {
            // Initialize the Unity Services SDK
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    
    public static void DoLog(string log)
    {
        Debug.Log(log);
    }
    
    public static void MoveToScene(string sceneName) 
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}