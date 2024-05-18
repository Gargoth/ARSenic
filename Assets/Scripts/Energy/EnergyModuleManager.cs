using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnergyModuleManager : MonoBehaviour
{
    EventSystem eventSystem;
    GameObject objectContainer;
    GameObject selectedTile;
    [SerializeField] GameObject finalTile;  // If this tile is powered, trigger level end
    
    void Start()
    {
        objectContainer = GameManagerScript.Instance.ObjectContainer;
        eventSystem = EventSystem.current;
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
                if (hit.collider != null)
                {
                    GameObject selectableObject = hit.collider.gameObject;
                    eventSystem.SetSelectedGameObject(selectableObject);
                }
        }
    }
}
