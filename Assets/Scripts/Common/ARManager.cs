using System;
using UnityEngine;
using Vuforia;

public class ARManager : MonoBehaviour
{
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private ContentPositioningBehaviour contentPositioningBehaviour;
    [SerializeField] GameObject groundPlaneCanvasPrefab;
    GameObject groundPlaneCanvas;

    public void Start()
    {
    }

    public void OnFoundGroundPlane()
    {
        Destroy(groundPlaneCanvas);
    }

    public void OnLostGroundPlane()
    {
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
}
