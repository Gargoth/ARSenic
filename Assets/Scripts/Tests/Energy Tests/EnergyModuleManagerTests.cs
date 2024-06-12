using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class EnergyModuleManagerTests
{
    GameObject objectContainer;
    GameObject mainCamera;
    GameObject gameObj;
    GameObject arObj;
    GameObject energyObj;
    List<GameObject> newObjects;
    int testNum = 0;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // ARRANGE
        testNum = 0;
        if (newObjects == null)
            newObjects = new List<GameObject>();
        objectContainer = new GameObject();
        objectContainer.tag = "Object Container";
        mainCamera = new GameObject();
        Camera mainCam = mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        yield return null;
        gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        // ARManager Prerequisites
        arObj = new GameObject();
        ARManager arManager = arObj.AddComponent<ARManager>();
        arManager.debugMode = true;
        yield return null;
        // Gravity Module Manager
        energyObj = new GameObject();
        EnergyModuleManager energyMod = energyObj.AddComponent<EnergyModuleManager>();
        energyMod.levelPrefabs = new List<GameObject>()
        {
            Resources.Load<GameObject>("Prefabs/Energy Stages/Energy Stage 1")
        };
        yield return null;
        GameObject eventObject = new GameObject();
        EventSystem newEventSystem = eventObject.AddComponent<EventSystem>();
        EventSystem.current = newEventSystem;
        newObjects.Add(eventObject);
        yield return null;

        Assert.AreSame(gameManager.ObjectContainer, arManager.objectContainer,
            "ARManager object container not equal to Game Manager object container");
        Assert.AreSame(energyMod.objectContainer, arManager.objectContainer,
            "Friction Module Manager object container reference not set");
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        testNum = 0;
        foreach (GameObject obj in newObjects)
        {
            GameObject.Destroy(obj);
        }
        newObjects.Clear();
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
        GameObject.Destroy(energyObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EnergyModule_Tile_Selection()
    {
        // ARRANGE
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Energy Tile");
        GameObject newTile = GameObject.Instantiate(tilePrefab);
        newObjects.Add(newTile);
        EnergyModuleManager energyMod = energyObj.GetComponent<EnergyModuleManager>();
        yield return null;
        EventSystem.current.SetSelectedGameObject(newTile);
        BaseEventData baseEventData = new BaseEventData(EventSystem.current);
        
        // ACT
        energyMod.SelectTile(baseEventData);
        Assert.AreSame(newTile, energyMod.selectedTile);
        energyMod.UnselectTile();
        Assert.AreNotSame(newTile, energyMod.selectedTile);
        energyMod.SelectTile(baseEventData);
        Assert.AreSame(newTile, energyMod.selectedTile);
    }
    
    [UnityTest]
    public IEnumerator EnergyModule_Tile_GeneratorSource()
    {
        // ARRANGE
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Energy Tile");
        GameObject newTile = GameObject.Instantiate(tilePrefab);
        newObjects.Add(newTile);
        EnergyTile tileScript = newTile.GetComponent<EnergyTile>();
        EnergyModuleManager energyMod = energyObj.GetComponent<EnergyModuleManager>();
        yield return null;
        EventSystem.current.SetSelectedGameObject(newTile);
        BaseEventData baseEventData = new BaseEventData(EventSystem.current);
        
        // ACT - Check source assignment
        energyMod.SelectTile(baseEventData);
        energyMod.AssignEnergySource("Sun");
        yield return null;
        yield return null;
        Assert.AreEqual(tileScript.CurrentSource.Name, "Sun", "Tile energy source not set to Sun");
        
        // ACT - Check generator power
        Assert.IsTrue(tileScript.IsPowered(), "Tile has Sun source but is not powered");
    }
    
    [UnityTest]
    public IEnumerator EnergyModule_Tile_PrevPower()
    {
        // ARRANGE
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Energy Tile");
        GameObject firstTile = GameObject.Instantiate(tilePrefab);
        GameObject secondTile = GameObject.Instantiate(tilePrefab);
        newObjects.Add(firstTile);
        newObjects.Add(secondTile);
        EnergyTile firstTileScript = secondTile.GetComponent<EnergyTile>();
        EnergyTile secondTileScript = secondTile.GetComponent<EnergyTile>();
        EnergyModuleManager energyMod = energyObj.GetComponent<EnergyModuleManager>();
        yield return null;
        EventSystem.current.SetSelectedGameObject(firstTile);
        BaseEventData baseEventData = new BaseEventData(EventSystem.current);
        
        energyMod.SelectTile(baseEventData);
        energyMod.AssignEnergySource("Sun");
        yield return null;
        yield return null;
        
        // ACT
        EventSystem.current.SetSelectedGameObject(secondTile);
        baseEventData = new BaseEventData(EventSystem.current);
        energyMod.SelectTile(baseEventData);
        energyMod.AssignEnergySource("SolarPanel");
        yield return null;
        yield return null;
        secondTileScript.UpdatePower();
        
        Assert.IsFalse(secondTileScript.IsPowered(), "Unconnected solar panel should not yet be powered");
        secondTileScript.PreviousTile = firstTileScript;
    }
}