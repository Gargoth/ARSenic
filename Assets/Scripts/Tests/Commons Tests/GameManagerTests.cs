using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTests
{
    [UnityTest]
    public IEnumerator GameManager_ObjectContainerBinding()
    {
        // ARRANGE
        GameObject objectContainer = new GameObject();
        objectContainer.tag = "Object Container";
        objectContainer.name = "objContainer";
        GameObject mainCamera = new GameObject();
        Camera mainCam = mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera";
        yield return null;
        GameObject gameObj = new GameObject();
        GameManagerScript gameManager = gameObj.AddComponent<GameManagerScript>();
        yield return null;
        Assert.AreSame(gameManager.ObjectContainer, objectContainer, "Object Container not binded properly");

        // TEARDOWN
        GameObject.Destroy(objectContainer);
        GameObject.Destroy(mainCamera);
        GameObject.Destroy(gameObj);
    }
}