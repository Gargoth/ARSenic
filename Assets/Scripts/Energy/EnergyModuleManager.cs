using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Main handler for the Energy Module processes
/// </summary>
public class EnergyModuleManager : Singleton<EnergyModuleManager>
{
    [SerializeField] Color selectedTileColor;
    [SerializeField] public List<GameObject> levelPrefabs;
    [NonSerialized] public GameObject objectContainer;
    EventSystem eventSystem;
    [NonSerialized] public GameObject selectedTile;
    GameObject[] energyTiles;

    IEnumerator Start()
    {
        eventSystem = EventSystem.current;
        StopwatchScript.Instance.ToggleStopwatch(true);
        int selectedLevel = PersistentDataContainer.Instance.selectedLevel;
        yield return new WaitUntil(() => ARManager.Instance != null);
        objectContainer = ARManager.Instance.objectContainer;
        Instantiate(levelPrefabs[selectedLevel], objectContainer.transform);
        yield return new WaitForSeconds(1f);
        InvokeRepeating("CheckWin", 0f, 1f);
    }

    void Update()
    {
        RaycastSelectable();
    }

    /// <summary>
    /// Checks if all energy tiles are powered.
    /// Repeatedly invoked from the Start function
    /// </summary>
    void CheckWin()
    {
        energyTiles = GameObject.FindGameObjectsWithTag("Energy Tile");
        if (energyTiles.Length == 0)
            return;
        foreach (GameObject energyTile in energyTiles)
        {
            EnergyTile tileScript = energyTile.GetComponent<EnergyTile>();
            if (!tileScript.IsPowered())
                return;
        }

        CancelInvoke("CheckWin");
        Invoke("FinishLevel", 1f);
    }

    /// <summary>
    /// Uses event system and physics raycaster to set selected object to the one that was clicked
    /// TODO: Merge with FrictionModuleManager RaycastSelectable
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
            {
                if (hit.collider != null)
                {
                    GameObject selectableObject = hit.collider.gameObject;
                    eventSystem.SetSelectedGameObject(selectableObject);
                }
            }
        }
    }

    /// <summary>
    /// Handles logic for when a tile is selected.
    /// Handles highlighting with Highlight.cs
    /// </summary>
    /// <param name="baseEventData">Event data received from event system</param>
    public void SelectTile(BaseEventData baseEventData)
    {
        GameObject obj = baseEventData.selectedObject;
        EnergyTile tileScript = obj.GetComponent<EnergyTile>();
        if (!tileScript.IsSelectable)
        {
            Debug.Log("Energy: Clicked tile not selectable");
            return;
        }

        Renderer objRenderer;

        if (selectedTile == obj)
        {
            UnselectTile();
            return;
        }

        if (selectedTile != null)
        {
            UnselectTile();
        }

        Debug.Log("Energy: Applying highlight");
        selectedTile = obj;
        objRenderer = obj.GetComponent<Renderer>();
        objRenderer.material.EnableKeyword("_EMISSION");
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_EmissionColor", selectedTileColor);
        objRenderer.SetPropertyBlock(materialPropertyBlock);
        Debug.Log(selectedTile);
    }

    /// <summary>
    /// Handles logic for when a tile is unselected.
    /// Handles highlighting with Highlight.cs
    /// </summary>
    /// <param name="baseEventData">Event data received from event system</param>
    public void UnselectTile()
    {
        if (selectedTile == null)
            return;
        Renderer objRenderer;
        Debug.Log("Energy: Removing highlight");
        objRenderer = selectedTile.GetComponent<Renderer>();
        objRenderer.SetPropertyBlock(null);
        Debug.Log(selectedTile);
        selectedTile = null;
    }

    /// <summary>
    /// Handles listeners for when a new energy source is assigned to selected tile
    /// Calls CreateNewSource() to handle creation of new energy source
    /// </summary>
    /// <param name="energySourceName">Name of the energy source to be assigned. Has to be string to allow UI button to use it. Will be converted to appropriate GameObject type</param>
    public void AssignEnergySource(string energySourceName)
    {
        if (selectedTile == null)
            return;

        EnergyTile selectedTileScript = selectedTile.GetComponent<EnergyTile>();
        // Remove current source
        if (selectedTileScript.CurrentSource != null)
        {
            selectedTileScript.RemoveComponentListeners();
            Destroy(selectedTileScript.CurrentSource);
            selectedTileScript.CurrentSource = null;
        }

        // Add new source
        CreateNewSource(energySourceName, selectedTileScript);
        selectedTileScript.AddComponentListeners();
        Debug.Log(name + "'s current source is " + selectedTileScript.CurrentSource.Name);
    }

    /// <summary>
    /// Creates new energy source and assigns it to the tile
    /// Handles conversion of energySourceName string to appropriate energy source
    /// </summary>
    /// <param name="energySourceName">Name of the energy source to attach</param>
    /// <param name="selectedTileScript">Script of the tile to attach energy source to</param>
    void CreateNewSource(string energySourceName, EnergyTile selectedTileScript)
    {
        EnergySource newSource;
        switch (energySourceName)
        {
            case "Sun":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.SunSource;
                break;
            case "SolarPanel":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.SolarPanelSource;
                break;
            case "Human":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.HumanSource;
                break;
            case "Stove":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.StoveSource;
                break;
            case "TV":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.TVSource;
                break;
            case "Generator":
                newSource = selectedTile.AddComponent(typeof(EnergySource)) as EnergySource;
                selectedTileScript.CurrentSource = newSource;
                newSource.EnergySourceType = EnergySourceType.GeneratorSource;
                break;
        }
    }

    /// <summary>
    /// Handles level completion
    /// Displays canvas that signifies level end
    /// </summary>
    public void FinishLevel()
    {
        foreach (GameObject canvas in GameObject.FindGameObjectsWithTag("Popup Canvas"))
        {
            Destroy(canvas);
        }

        GameObject endCanvasPrefab = Resources.Load<GameObject>("Prefabs/End Canvas");
        Instantiate(endCanvasPrefab);
        StopwatchScript.Instance.ToggleStopwatch(false);
    }
}