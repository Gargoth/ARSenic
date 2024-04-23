using UnityEngine;
using Vuforia;

public class ARManager : MonoBehaviour
{
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private ContentPositioningBehaviour contentPositioningBehaviour;

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
