using System.Collections;
using System.Collections.Generic;
using AirResistance2;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GravityModuleManagerTests
{
    [UnityTest]
    public IEnumerator GravityModuleManager_Setup()
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
        // Gravity Module Manager
        GameObject gravObj = new GameObject();
        GravityModuleManagerScript gravMod = gravObj.AddComponent<GravityModuleManagerScript>();
        yield return null;
        yield return null;
        
        Assert.AreSame(gameManager.ObjectContainer, arManager.objectContainer, "ARManager object container not equal to Game Manager object container");
        Assert.AreSame(gravMod.ObjectContainer, arManager.objectContainer, "Gravity Module Manager object container reference not set");

        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
        GameObject.Destroy(gravObj);
    }

    [UnityTest]
    public IEnumerator GravityModuleManager_Prefab_SpawnAndClear()
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
        // Gravity Module Manager
        GameObject gravObj = new GameObject();
        GravityModuleManagerScript gravMod = gravObj.AddComponent<GravityModuleManagerScript>();
        yield return null;
        yield return null;
        
        Assert.AreSame(gameManager.ObjectContainer, arManager.objectContainer, "ARManager object container not equal to Game Manager object container");
        Assert.AreSame(gravMod.ObjectContainer, arManager.objectContainer, "Gravity Module Manager object container reference not set");

        gravMod.spawnPrefabs = new List<GameObject>() { Resources.Load<GameObject>("Standard Assets/Friction/PolygonPrototype/Prefabs/Icons/SM_Icon_Balloon_01") };
        
        // ACT
        gravMod.SpawnPrefab(0);
        gravMod.SpawnPrefab(0);
        gravMod.SpawnPrefab(0);
        yield return null;
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("GravObject");
        Assert.AreEqual(spawnedObjects.Length, 3);
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.IsTrue(obj != null, "Spawned prefab does not exist");
        }
        yield return null;
        
        GravityModuleManagerScript.ClearItems();
        yield return null;
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.IsTrue(obj == null, "Spawned prefab did not get deleted");
        }

        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
        GameObject.Destroy(gravObj);
    }

    [UnityTest]
    public IEnumerator GravityModuleManager_UpdateAirResist()
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
        // Gravity Module Manager
        GameObject gravObj = new GameObject();
        GravityModuleManagerScript gravMod = gravObj.AddComponent<GravityModuleManagerScript>();
        yield return null;
        yield return null;
        
        Assert.AreSame(gameManager.ObjectContainer, arManager.objectContainer, "ARManager object container not equal to Game Manager object container");
        Assert.AreSame(gravMod.ObjectContainer, arManager.objectContainer, "Gravity Module Manager object container reference not set");

        gravMod.spawnPrefabs = new List<GameObject>() { Resources.Load<GameObject>("Standard Assets/Friction/PolygonPrototype/Prefabs/Icons/SM_Icon_Balloon_01") };
        
        // ACT
        gravMod.SpawnPrefab(0);
        gravMod.SpawnPrefab(0);
        gravMod.SpawnPrefab(0);
        yield return null;
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("GravObject");
        Assert.AreEqual(spawnedObjects.Length, 3);
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.IsTrue(obj != null, "Spawned prefab does not exist");
        }
        yield return null;
        
        gravMod.UpdateAirResistance(false);
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.AreEqual(obj.GetComponent<AirResistance>().enabled, false, "Spawned prefab does not exist");
        }
        gravMod.UpdateAirResistance(true);
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.AreEqual(obj.GetComponent<AirResistance>().enabled, true, "Spawned prefab does not exist");
        }

        // TEARDOWN
        GravityModuleManagerScript.ClearItems();
        yield return null;
        foreach (GameObject obj in spawnedObjects)
        {
            Assert.IsTrue(obj == null, "Spawned prefab did not get deleted");
        }
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
        GameObject.Destroy(gravObj);
    }
}