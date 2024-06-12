using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class FrictionModuleManagerTests
{
    GameObject objectContainer;
    GameObject mainCamera;
    GameObject gameObj;
    GameObject arObj;
    GameObject frictionObj;
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
        frictionObj = new GameObject();
        FrictionModuleManager frictionMod = frictionObj.AddComponent<FrictionModuleManager>();
        frictionMod.levelPrefabs = new List<GameObject>()
        {
            Resources.Load<GameObject>("Prefabs/Friction Stages/Friction Stage 1")
        };
        yield return null;
        yield return null;

        Assert.AreSame(gameManager.ObjectContainer, arManager.objectContainer,
            "ARManager object container not equal to Game Manager object container");
        Assert.AreSame(frictionMod.objectContainer, arManager.objectContainer,
            "Friction Module Manager object container reference not set");
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        testNum = 0;
        newObjects.Clear();
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
        GameObject.Destroy(arObj);
        GameObject.Destroy(frictionObj);
        yield return null;
    }

    void IncrementTestNum()
    {
        Debug.Log("Incrementing test num");
        testNum++;
    }

    [UnityTest]
    public IEnumerator FrictionModule_DelayedTrigger()
    {
        // ARRANGE
        GameObject triggerObj = new GameObject();
        newObjects.Add(triggerObj);
        DelayedTrigger trigger = triggerObj.AddComponent<DelayedTrigger>();
        trigger.TriggerEvent.AddListener(IncrementTestNum);
        
        // ACT
        trigger.StartTriggerCoroutine(0.2f);
        trigger.CancelTriggerCoroutine();
        yield return new WaitForSeconds(0.3f);
        Assert.AreEqual(testNum, 0);
        
        trigger.StartTriggerCoroutine(0.2f);
        yield return new WaitForSeconds(0.3f);
        Assert.AreEqual(testNum, 1);
    }

    [UnityTest]
    public IEnumerator FrictionModule_Push()
    {
        // ARRANGE
        GameObject progressMask = new GameObject();
        newObjects.Add(progressMask);
        progressMask.AddComponent<Image>();
        List<GameObject> bottomMenu = new List<GameObject>();
        FrictionModuleManager frictionMod = frictionObj.GetComponent<FrictionModuleManager>();
        frictionMod.progressMask = progressMask;
        frictionMod.bottomMenu = bottomMenu;
        Rigidbody playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        Assert.IsTrue(playerRb != null, "Player not found");
        
        // ACT
        frictionMod.OnPushButtonDown(new BaseEventData(EventSystem.current));
        yield return new WaitForSeconds(1f);
        frictionMod.OnPushButtonUp(new BaseEventData(EventSystem.current));
        yield return new WaitForFixedUpdate();
        
        Assert.GreaterOrEqual(playerRb.velocity.magnitude, 0.1f, "Player did not get pushed");
    }
}