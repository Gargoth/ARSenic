using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ARManagerTestScript
{
    [UnityTest]
    public IEnumerator ARManager_DebugMode_Setup()
    {
        // ARRANGE
        GameObject objectContainer = new GameObject();
        objectContainer.tag = "Object Container";
        GameObject mainCamera = new GameObject();
        Camera mainCam = mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        GameObject gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        // ARManager Prerequisites
        GameObject arObj = new GameObject();
        ARManager arManager = arObj.AddComponent<ARManager>();
        arManager.debugMode = true;
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ARManager_DebugMode_GroundPlaneCanvas()
    {
        // ARRANGE
        GameObject objectContainer = new GameObject();
        objectContainer.tag = "Object Container";
        GameObject mainCamera = new GameObject();
        Camera mainCam = mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        GameObject gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        // ARManager Prerequisites
        GameObject arObj = new GameObject();
        ARManager arManager = arObj.AddComponent<ARManager>();
        arManager.debugMode = true;
        yield return null;
        
        // ACT
        arManager.debugMode = false;
        arManager.OnLostGroundPlane();
        GameObject groundPlaneCanvas = GameObject.FindWithTag("Ground Plane Canvas");
        yield return null;
        
        // ASSERT
        Assert.IsTrue(groundPlaneCanvas != null);
        
        // ACT
        arManager.OnFoundGroundPlane();
        yield return null;
        
        // ASSERT
        Assert.IsTrue(groundPlaneCanvas == null);
    }
}
