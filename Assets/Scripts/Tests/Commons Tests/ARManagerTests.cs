using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ARManagerTests
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
        yield return null;
        GameObject gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        // ARManager Prerequisites
        GameObject arObj = new GameObject();
        ARManager arManager = arObj.AddComponent<ARManager>();
        arManager.debugMode = true;
        yield return null;
        
        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
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
        yield return null;
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
        Assert.IsTrue(groundPlaneCanvas != null, "Ground Plane Canvas was not instantiated");
        
        // ACT
        arManager.OnFoundGroundPlane();
        yield return null;
        
        // ASSERT
        Assert.IsTrue(groundPlaneCanvas == null, "Ground Plane Canvas was not destroyed");
        
        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
    }
    
    [UnityTest]
    public IEnumerator ARManager_DebugMode_ObjectContainerControl()
    {
        // ARRANGE
        GameObject objectContainer = new GameObject();
        objectContainer.tag = "Object Container";
        GameObject mainCamera = new GameObject();
        Camera mainCam = mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        yield return null;
        GameObject gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        yield return null;
        // ARManager Prerequisites
        GameObject arObj = new GameObject();
        ARManager arManager = arObj.AddComponent<ARManager>();
        arManager.debugMode = true;
        yield return null;
        Assert.AreEqual(arManager.objectContainer, gameManager.ObjectContainer);
        
        // ACT
        arManager.SetActiveObjectContainer(true);
        yield return null;
        
        // ASSERT
        Assert.IsTrue(objectContainer.activeSelf, "Object container was not activated");
        
        // ACT
        arManager.SetActiveObjectContainer(false);
        yield return null;
        
        // ASSERT
        Assert.IsFalse(objectContainer.activeSelf, "Object container was not deactivated");
        
        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
    }
}
