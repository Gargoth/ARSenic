using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Vuforia;

public class ARManager : Singleton<ARManager>
{
    [SerializeField] public GameObject objectContainer;
    [SerializeField] ContentPositioningBehaviour contentPositioningBehaviour;
    [SerializeField] GameObject groundPlaneCanvasPrefab;
    GameObject groundPlaneCanvas;

    #region DEBUG

    [Header("DEBUG")] [SerializeField] bool debugMode;
    [SerializeField] Vector3 offset;
    [SerializeField] float distance;
    GameObject debugCamera;

    #endregion

    void Start()
    {
        if (objectContainer == null)
            objectContainer = GameManagerScript.Instance.ObjectContainer;

        if (debugMode)
        {
            Debug.Log("Debug Mode detected");
            StartCoroutine(InitializeDebugMode());
        }
        else
        {
            objectContainer.transform.SetParent(GameObject.Find("Ground Plane Stage").transform);
        }
    }

    void Update()
    {
        HandleDebugMode();
    }

    void HandleDebugMode()
    {
        debugCamera.transform.localPosition = Vector3.zero + offset.normalized * distance;
        debugCamera.transform.LookAt(objectContainer.transform);
    }

    public void OnFoundGroundPlane()
    {
        Destroy(groundPlaneCanvas);
    }

    public void OnLostGroundPlane()
    {
        if (!debugMode)
            groundPlaneCanvas = Instantiate(groundPlaneCanvasPrefab);
    }

    public void HandlePlaneFinderHit(HitTestResult hitTestResult)
    {
        if (!objectContainer.activeInHierarchy)
            contentPositioningBehaviour.PositionContentAtPlaneAnchor(hitTestResult);
    }

    public void SetActiveObjectContainer(bool value)
    {
        objectContainer.SetActive(value);
    }

    IEnumerator InitializeDebugMode()
    {
        Debug.Log("Initializing Debug Mode");
        GameObject debugCamera = InitializeDebugCamera();
        yield return new WaitForEndOfFrame();
        Debug.Log("Changing Camera");
        GameObject arCamera = Camera.main.gameObject;
        arCamera.tag = "Untagged";
        arCamera.gameObject.SetActive(false);
        debugCamera.tag = "MainCamera";
        objectContainer.SetActive(true);
    }

    GameObject InitializeDebugCamera()
    {
        Debug.Log("Initializing Debug Camera");
        debugCamera = new GameObject();
        debugCamera.AddComponent<Camera>().backgroundColor = Color.black;
        PhysicsRaycaster physicsRaycaster = debugCamera.AddComponent<PhysicsRaycaster>();
        physicsRaycaster.eventMask = 1 << 6;
        debugCamera.transform.SetParent(objectContainer.transform.parent);
        debugCamera.transform.localPosition = Vector3.zero + offset.normalized * distance;
        debugCamera.transform.LookAt(objectContainer.transform);
        debugCamera.name = "DEBUG Camera";
        return debugCamera;
    }
}