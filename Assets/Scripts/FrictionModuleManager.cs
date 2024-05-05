using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FrictionModuleManager : Singleton<FrictionModuleManager>
{
    [SerializeField] GameObject objectContainer;
    [SerializeField] GameObject progressMask;
    [SerializeField] GameObject shapeToggle;
    [SerializeField] List<Material> materials;
    [SerializeField] List<PhysicMaterial> physicMaterials;
    [SerializeField] List<GameObject> levelPrefabs;
    [SerializeField] float pushProgressRate;
    
    [Header("DEBUG")]
    [SerializeField] bool isTargetFound = false;
    [SerializeField] bool canPush = true;
    [SerializeField] bool forceRebindPlayer = false;
    [SerializeField] bool forceReset = false;
    
    [SerializeField] List<GameObject> selectedRoads;
    List<int> uiTouchFingerIDs;
    EventSystem eventSystem;
    FrictionPlayerController player;
    RaycastHit hit;
    float pushProgress;
    bool isPushButton = false;
    bool isPlayerShapeCube = true;

    IEnumerator Start()
    {
        selectedRoads = new List<GameObject>();
        int selectedLevel = PersistentDataContainer.Instance.selectedLevel;
        eventSystem = EventSystem.current;
        Instantiate(levelPrefabs[selectedLevel], objectContainer.transform);
        yield return null;
    }

    void Update()
    {
        if (isPushButton)
        {
            pushProgress = math.min(pushProgress + (pushProgressRate * Time.deltaTime), 1);
            progressMask.GetComponent<Image>().fillAmount = pushProgress;
        }
        
        // Override Event System
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create a raycast from the Camera and output anything it hits
            int mask = 1 << 6; // Mask for Selectable layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                //Check the hit GameObject has a Collider
                if (hit.collider != null)
                {
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    GameObject m_MyGameObject = hit.collider.gameObject;
                    //Set this GameObject you clicked as the currently selected in the EventSystem
                    eventSystem.SetSelectedGameObject(m_MyGameObject);
                    //Output the current selected GameObject's name to the console
                    Debug.Log("Current selected GameObject : " + eventSystem.currentSelectedGameObject);
                }
        }
        
        // DEBUG
        if (forceReset)
        {
            forceReset = false;
            ResetLevel();
        }
        
        if (forceRebindPlayer)
        {
            forceRebindPlayer = false;
            player = GameObject.FindWithTag("Player").GetComponent<FrictionPlayerController>();
        }
    }

    void UpdatePlayerShape()
    {
        player.ToggleShape(isPlayerShapeCube);
    }

    public void HandleShapeToggle(bool isCube)
    {
        isPlayerShapeCube = isCube;
        UpdatePlayerShape();
    }

    public void HandleTargetFound()
    {
        isTargetFound = true;
        player = GameObject.FindWithTag("Player").GetComponent<FrictionPlayerController>();
        UpdatePlayerShape();
    }

    public void HandleTargetLost()
    {
        isTargetFound = false;
        player = null;
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

    void HandlePhysRaycast(int i, Camera mainCam)
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

    public void ResetLevel()
    {
        canPush = true;
        player.ResetPlayer();
    }

    public void OnPushButtonUp(BaseEventData baseEventData)
    {
        if (isPushButton && isTargetFound)
        {
            isPushButton = false;
            player.PushPlayer(pushProgress);
            pushProgress = 0;
            progressMask.GetComponent<Image>().fillAmount = pushProgress;
            canPush = false;
        }
    }

    public void OnPushButtonDown(BaseEventData baseEventData)
    {
        if (isTargetFound && canPush)
            isPushButton = true;
    }

    public void OnPushButtonExit(BaseEventData baseEventData)
    {
        isPushButton = false;
        player.PushPlayer(pushProgress);
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

    public void OnRoadClick(BaseEventData baseEventData)
    {
        GameObject selectedObject = baseEventData.selectedObject;
        Debug.Log(selectedObject);
        if (selectedRoads.Contains(selectedObject))
        {
            UnselectRoad(selectedObject);
        }
        else
        {
            SelectRoad(selectedObject);
        }
    }

    // TODO: Implementation
    public void SelectRoad(GameObject obj)
    {
        Debug.Log("Selected Road");
        selectedRoads.Add(obj);
        // TODO: Highlight road
    }

    // TODO: Implementation
    public void UnselectRoad(GameObject obj)
    {
        Debug.Log("Unselected Road");
        selectedRoads.Remove(obj);
        // TODO: Remove highlight
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