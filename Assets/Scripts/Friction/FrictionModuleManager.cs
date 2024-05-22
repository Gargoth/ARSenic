using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FrictionModuleManager : Singleton<FrictionModuleManager>
{
    [SerializeField] GameObject objectContainer;
    
	[Header("ROAD MATERIALS")]
    [SerializeField] List<Material> materials;
    [SerializeField] List<PhysicMaterial> physicMaterials;
    [SerializeField] List<GameObject> levelPrefabs;
    [SerializeField] Color emissionColor;

	[Header("PUSH")]
    [SerializeField] GameObject progressMask;
	[SerializeField] float pushProgressRate;
	[SerializeField] List<GameObject> bottomMenu;
    
    [Header("DEBUG")]
    [SerializeField] bool isTargetFound = false;

    [SerializeField]
    public bool CanPush { get; private set; } = true;
    
    [SerializeField] List<GameObject> selectedRoads;
    List<int> uiTouchFingerIDs;
    EventSystem eventSystem;
    FrictionPlayerController player;
    RaycastHit hit;
    public UnityEvent ResetActions { get; private set; }
    float pushProgress;
    bool isPushButton = false;
    bool isPlayerShapeCube = true;
    bool isFinished = false;

    IEnumerator Start()
    {
        if (PersistentDataContainer.Instance.f_frictionDialogShown)
        {
            Destroy(GameObject.FindWithTag("Popup Canvas"));
            StopwatchScript.Instance.ToggleStopwatch(true);
        }
        else
        {
            PersistentDataContainer.Instance.f_frictionDialogShown = true;
        }

        ResetActions = new UnityEvent();
        selectedRoads = new List<GameObject>();
        int selectedLevel = PersistentDataContainer.Instance.selectedLevel;
        eventSystem = EventSystem.current;
        yield return new WaitUntil(() => ARManager.Instance != null);
        objectContainer = ARManager.Instance.objectContainer;
        Instantiate(levelPrefabs[selectedLevel], objectContainer.transform);
    }

    void FixedUpdate()
    {
        if (isTargetFound)
        {
            Vector3 transformUp = objectContainer.transform.up;
            Vector3 newGravity = -transformUp.normalized * 9.81f;
            Physics.gravity = newGravity;
        }
    }

    void Update()
    {
        if (isPushButton)
        {
            pushProgress = math.min(pushProgress + (pushProgressRate * Time.deltaTime), 1);
            progressMask.GetComponent<Image>().fillAmount = pushProgress;
        }
        
        RaycastSelectable();
    }

    void RaycastSelectable()
    {
        // Override Event System
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int mask = 1 << 6; // Mask for Selectable layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                if (hit.collider != null)
                {
                    GameObject selectableObject = hit.collider.gameObject;
                    eventSystem.SetSelectedGameObject(selectableObject);
                }
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
        player.SetInitialState();
        UpdatePlayerShape();
    }

    public void HandleTargetLost()
    {
        isTargetFound = false;
        player = null;
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

    public void ResetLevel()
    {
        CanPush = true;
        ResetActions.Invoke();
		ResetColor();
    }

    public void OnPushButtonUp(BaseEventData baseEventData)
    {
        if (isPushButton && isTargetFound)
        {
            isPushButton = false;
            player.PushPlayer(pushProgress);
            pushProgress = 0;
            progressMask.GetComponent<Image>().fillAmount = pushProgress;
            CanPush = false;
            GrayOut();
        }
    }

    public void OnPushButtonDown(BaseEventData baseEventData)
    {
        if (isTargetFound && CanPush)
            isPushButton = true;
    }

    public void OnPushButtonExit(BaseEventData baseEventData)
    {
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

	public void GrayOut()
	{
		for (int i = 0; i < bottomMenu.Count; i++)
		{
			Image image = bottomMenu[i].GetComponent<Image>();
			image.color = Color.gray;
		}
	}

	public void ResetColor()
	{
		for (int i = 0; i < bottomMenu.Count; i++)
		{
			Image image = bottomMenu[i].GetComponent<Image>();
			image.color = Color.white;
		}
	}

    public void OnRoadClick(BaseEventData baseEventData)
    {
        GameObject selectedObject = baseEventData.selectedObject;
        if (selectedRoads.Contains(selectedObject))
        {
            UnselectRoad(selectedObject);
        }
        else
        {
            SelectRoad(selectedObject);
        }
    }

    public void SelectRoad(GameObject obj)
    {
        selectedRoads.Add(obj);
        Renderer objRenderer = obj.GetComponent<Renderer>();
        objRenderer.material.EnableKeyword("_EMISSION");
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_EmissionColor", emissionColor);
        objRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void UnselectRoad(GameObject obj)
    {
        selectedRoads.Remove(obj);
        Renderer objRenderer = obj.GetComponent<Renderer>();
        objRenderer.SetPropertyBlock(null);
    }

    public void ClearSelectedRoads()
    {
        for (int i = selectedRoads.Count - 1; i >= 0; i--)
        {
            UnselectRoad(selectedRoads[i]);
        }
    }

    public void ChangeRoad(int index)
    {
        ChangeMaterial(index);
        ChangePhysicMaterial(index);
        ClearSelectedRoads();
    }

    // Changes material of all selected roads
    void ChangeMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            MeshRenderer mesh = road.GetComponent<MeshRenderer>();
            mesh.material = materials[index];
        }
    }

    // Changes physics material of all selected roads
    void ChangePhysicMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            Collider collider = road.GetComponent<Collider>();
            collider.material = physicMaterials[index];
        }
    }

    public void FinishLevel()
    {
        isFinished = false;
        StopwatchScript.Instance.ToggleStopwatch(false);
    }
}