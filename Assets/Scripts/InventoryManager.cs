using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject popUpCanvas;
    
    // Start is called before the first frame update
    // void Start()
    // {
    //
    // }
    
    // Update is called once per frame
    // void Update()
    // {
    //
    // }

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
