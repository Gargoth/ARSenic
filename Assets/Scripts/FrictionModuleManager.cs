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
    
    List<GameObject> selectedRoads;
    List<int> uiTouchFingerIDs;
    FrictionPlayerController player;
    RaycastHit hit;
    float pushProgress;
    bool isPushButton = false;
    bool isPlayerShapeCube = true;

    void Awake()
    {
    }

    IEnumerator Start()
    {
        selectedRoads = new List<GameObject>();
        int selectedLevel = PersistentDataContainer.Instance.selectedLevel;
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