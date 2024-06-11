using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic function for toggling a GameObject's state on or off.
/// TODO: Make this more generic
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public GameObject popUpCanvas;

    public void OpenInventory()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(true);
        }
    }
    
    public void CloseInventory()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.SetActive(false);
        }
    }
}
