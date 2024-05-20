using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnergyModuleManager : Singleton<EnergyModuleManager>
{
    [SerializeField] Color selectedTileColor;
    EventSystem eventSystem;
    GameObject objectContainer;
    GameObject selectedTile;
    [Header("DEBUG")] [SerializeField] bool debugMode;

    void Start()
    {
        objectContainer = GameManagerScript.Instance.ObjectContainer;
        eventSystem = EventSystem.current;

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
            {
                if (hit.collider != null)
                {
                    GameObject selectableObject = hit.collider.gameObject;
                    eventSystem.SetSelectedGameObject(selectableObject);
                }
            }
        }
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
        GameObject debugCamera = new GameObject();
        debugCamera.AddComponent<Camera>().backgroundColor = Color.black;
        PhysicsRaycaster physicsRaycaster = debugCamera.AddComponent<PhysicsRaycaster>();
        physicsRaycaster.eventMask = 1 << 6;
        debugCamera.transform.SetParent(objectContainer.transform.parent);
        float distance = 1f;
        debugCamera.transform.localPosition = Vector3.zero + Vector3.one * distance;
        debugCamera.transform.LookAt(objectContainer.transform);
        debugCamera.name = "DEBUG Camera";
        return debugCamera;
    }

    public void SelectTile(BaseEventData baseEventData)
    {
        GameObject obj = baseEventData.selectedObject;
        EnergyTile tileScript = obj.GetComponent<EnergyTile>();
        if (!tileScript.IsSelectable)
        {
            Debug.Log("Energy: Clicked tile not selectable");
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

    void UnselectTile()
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

    // NOTE: There has to be a better way to do this
    public void AssignEnergySource(string energySourceName)
    {
        if (selectedTile == null)
            return;

        EnergyTile selectedTileScript = selectedTile.GetComponent<EnergyTile>();
        // Remove current source
        Destroy(selectedTileScript.CurrentSource);
        selectedTileScript.CurrentSource = null;

        // Add new source
        // TODO: Update sources
        EnergySource newSource;
        switch (energySourceName)
        {
            case "Sun":
                newSource = selectedTile.AddComponent(typeof(SunSource)) as SunSource;
                selectedTileScript.CurrentSource = newSource;
                break;
            case "SolarPanel":
                newSource = selectedTile.AddComponent(typeof(SolarPanelSource)) as SolarPanelSource;
                selectedTileScript.CurrentSource = newSource;
                break;
            case "Human":
                newSource = selectedTile.AddComponent(typeof(HumanSource)) as HumanSource;
                selectedTileScript.CurrentSource = newSource;
                break;
            case "Stove":
                newSource = selectedTile.AddComponent(typeof(StoveSource)) as StoveSource;
                selectedTileScript.CurrentSource = newSource;
                break;
            case "TV":
                newSource = selectedTile.AddComponent(typeof(TVSource)) as TVSource;
                selectedTileScript.CurrentSource = newSource;
                break;
            case "Generator":
                newSource = selectedTile.AddComponent(typeof(GeneratorSource)) as GeneratorSource;
                selectedTileScript.CurrentSource = newSource;
                break;
        }
    }
}