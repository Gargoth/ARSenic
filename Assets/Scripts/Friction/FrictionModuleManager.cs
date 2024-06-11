using System;
using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Main Handler for the Friction Module processes
/// </summary>
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

    private bool isDebugMode;

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
        StopwatchScript.Instance.ToggleStopwatch(true);
        player = GameObject.FindWithTag("Player").GetComponent<FrictionPlayerController>();

        if (ARManager.Instance.debugMode)
            isDebugMode = true;
        else
            isDebugMode = false;
    }

    void FixedUpdate()
    {
        // Manually set gravity to the downward direction of the object container
        // Needed since Vuforia does not set rotation properly
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

    /// <summary>
    /// Uses event system and physics raycaster to set selected object to the one that was clicked
    /// TODO: Merge with EnergyModuleManager RaycastSelectable
    /// </summary>
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

    /// <summary>
    /// Wrapper for player shape toggle
    /// </summary>
    void UpdatePlayerShape()
    {
        player.ToggleShape(isPlayerShapeCube);
    }

    /// <summary>
    /// Toggles the player shape
    /// Public function for button usage
    /// </summary>
    /// <param name="isCube">True if cube, false if sphere</param>
    public void HandleShapeToggle(bool isCube)
    {
        isPlayerShapeCube = isCube;
        UpdatePlayerShape();
    }

    /// <summary>
    /// Called when target plane is found
    /// Set player's initial state based on its position on start
    /// Public function for Vuforia component listener
    /// </summary>
    public void HandleTargetFound()
    {
        isTargetFound = true;
        player = GameObject.FindWithTag("Player").GetComponent<FrictionPlayerController>();
        player.SetInitialState();
        UpdatePlayerShape();
    }

    /// <summary>
    /// Called when target plane is lost.
    /// Reset fields for player and push progress bar.
    /// Public function for Vuforia component listener.
    /// </summary>
    public void HandleTargetLost()
    {
        isTargetFound = false;
        player = null;
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

    /// <summary>
    /// Invokes all functions listening to ResetActions event and resets fields
    /// Public function for button usage
    /// </summary>
    public void ResetLevel()
    {
        CanPush = true;
        ResetActions.Invoke();
		ResetColor();
    }

    /// <summary>
    /// Pushes object if conditions are met
    /// Called by event system when push button is released
    /// </summary>
    /// <param name="baseEventData"></param>
    public void OnPushButtonUp(BaseEventData baseEventData)
    {
        if (isPushButton && (isDebugMode || isTargetFound))
        {
            isPushButton = false;
            player.PushPlayer(pushProgress);
            pushProgress = 0;
            progressMask.GetComponent<Image>().fillAmount = pushProgress;
            CanPush = false;
            GrayOut();
        }
    }

    /// <summary>
    /// Sets isPushButton to true which increments the progress bar
    /// </summary>
    /// <param name="baseEventData"></param>
    public void OnPushButtonDown(BaseEventData baseEventData)
    {
        if ((isDebugMode || isTargetFound) && CanPush)
            isPushButton = true;
    }

    /// <summary>
    /// Resets push progress bar if touch leaves the button without releasing
    /// </summary>
    /// <param name="baseEventData"></param>
    public void OnPushButtonExit(BaseEventData baseEventData)
    {
        pushProgress = 0;
        progressMask.GetComponent<Image>().fillAmount = pushProgress;
    }

    /// <summary>
    /// Grays out buttons
    /// </summary>
	public void GrayOut()
	{
		for (int i = 0; i < bottomMenu.Count; i++)
		{
			Image image = bottomMenu[i].GetComponent<Image>();
			image.color = Color.gray;
		}
	}
  
    /// <summary>
    /// Resets UI button colors
    /// </summary>
	public void ResetColor()
	{
		for (int i = 0; i < bottomMenu.Count; i++)
		{
			Image image = bottomMenu[i].GetComponent<Image>();
			image.color = Color.white;
		}
	}

    /// <summary>
    /// Toggles road state.
    /// Calls either UnselectRoad or SelectRoad depending on initial state.
    /// Called by FrictionStageManager when road object is clicked.
    /// </summary>
    /// <param name="baseEventData"></param>
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

    /// <summary>
    /// Adds selected object to selectedRoads list
    /// Handles highlighting for when road is selected.
    /// </summary>
    /// <param name="obj">Selected road object</param>
    public void SelectRoad(GameObject obj)
    {
        selectedRoads.Add(obj);
        Renderer objRenderer = obj.GetComponent<Renderer>();
        objRenderer.material.EnableKeyword("_EMISSION");
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_EmissionColor", emissionColor);
        objRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    /// <summary>
    /// Removes selected object from selectedRoads list
    /// Handles highlighting for when road is unselected.
    /// </summary>
    /// <param name="obj">Unselected road object</param>
    public void UnselectRoad(GameObject obj)
    {
        selectedRoads.Remove(obj);
        Renderer objRenderer = obj.GetComponent<Renderer>();
        objRenderer.SetPropertyBlock(null);
    }

    /// <summary>
    /// Unselects all selected roads
    /// </summary>
    public void ClearSelectedRoads()
    {
        for (int i = selectedRoads.Count - 1; i >= 0; i--)
        {
            UnselectRoad(selectedRoads[i]);
        }
    }

    /// <summary>
    /// Calls ChangeMaterial and ChangePhysicMaterial to change the road material of selected roads.
    /// </summary>
    /// <param name="index">Index of road material and physics material to apply</param>
    public void ChangeRoad(int index)
    {
        if (selectedRoads.Count == 0)
            Toast.Show("Select a road first!");
        ChangeMaterial(index);
        ChangePhysicMaterial(index);
        ClearSelectedRoads();
    }

    /// <summary>
    /// Changes material of selected roads
    /// </summary>
    /// <param name="index">Index of road material to apply</param>
    void ChangeMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            MeshRenderer mesh = road.GetComponent<MeshRenderer>();
            mesh.material = materials[index];
        }
    }
    
    /// <summary>
    /// Changes physics material of selected roads
    /// </summary>
    /// <param name="index">Index of physics material to apply</param>
    void ChangePhysicMaterial(int index)
    {
        foreach (GameObject road in selectedRoads)
        {
            Collider collider = road.GetComponent<Collider>();
            collider.material = physicMaterials[index];
        }
    }
    
    /// <summary>
    /// Handles level completion
    /// </summary>
    public void FinishLevel()
    {
        isFinished = false;
        StopwatchScript.Instance.ToggleStopwatch(false);
    }
}