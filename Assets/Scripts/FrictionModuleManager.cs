using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FrictionModuleManager : Singleton<FrictionModuleManager>
{
    [SerializeField] GameObject objectContainer;
    [SerializeField] List<Material> materials;
    [SerializeField] List<PhysicMaterial> physicMaterials;
    [SerializeField] GameObject pushButtonObj;
    [SerializeField] GameObject fillMaskObj;
    Image pushButton;
    Image fillMask;
    List<GameObject> selectedRoads;
    RaycastHit hit;
    List<int> uiTouchFingerIDs;

    private void Awake()
    {
        selectedRoads = new List<GameObject>();
        pushButton = pushButtonObj.GetComponent<Image>();
        fillMask = fillMaskObj.GetComponent<Image>();
    }

    void Update()
    {
        var mainCam = Camera.main;
        for (int i = 0; i < Input.touchCount; i++)
        {
            // Graphics Raycasts
            Touch touch = Input.GetTouch(i);
            /* TODO: Use graphics raycast to check if touch is on pushButton
             * If it started on pushButton, slowly increase the fill value of the mask
             * (to simulate loading the progress bar)
             * On release, do something
             */

            // Physics Raycasts
            HandlePhysRaycast(i, mainCam);
        }
    }

    private void HandlePhysRaycast(int i, Camera mainCam)
    {
        if (!objectContainer.activeInHierarchy) return;
        Touch touch = Input.GetTouch(i);
        if (touch.phase != TouchPhase.Began) return;
        Ray ray = mainCam!.ScreenPointToRay(touch.position);
        int mask = ~(1 << 2);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (!hit.collider.CompareTag("Road")) return;
            // Do stuff
            // TODO: Highlight road
            // TODO: Select road
            SelectRoad(hit.collider.gameObject);
        }
    }

    // TODO: Implementation
    public void SelectRoad(GameObject obj)
    {
        // TODO: Highlight road
        // TODO: Add to Selected Roads
    }

    // TODO: Implementation
    public void UnselectRoad(GameObject obj)
    {
        // TODO: Remove highlight
        // TODO: Remove from selected roads
    }

    // TODO: Test
    public void ClearSelectedRoads()
    {
        foreach (GameObject road in selectedRoads)
        {
            UnselectRoad(road);
        }
    }

    // Changes material of all selected roads
    public void ChangeMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            MeshRenderer mesh = road.GetComponent<MeshRenderer>();
            mesh.material = materials[index];
        }
    }

    // Changes physics material of all selected roads
    public void ChangePhysicMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            Collider collider = road.GetComponent<Collider>();
            collider.material = physicMaterials[index];
        }
    }
}