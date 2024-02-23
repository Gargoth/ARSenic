using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
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
        SceneManager.LoadScene(sceneName);
    }
}