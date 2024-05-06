using System;
using UnityEngine;
using Vuforia;

public class ARManager : MonoBehaviour
{
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private ContentPositioningBehaviour contentPositioningBehaviour;
    [SerializeField] GameObject groundPlaneCanvasPrefab;

    public void Start()
    {
    }

    public void OnFoundGroundPlane()
    {
        Destroy(GameObject.FindWithTag("Ground Plane Canvas"));
    }

    public void OnLostGroundPlane()
    {
        Instantiate(groundPlaneCanvasPrefab);
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
