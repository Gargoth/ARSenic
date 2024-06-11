using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the home screen scene.
/// Currently only displays the start dialog if it has not been seen yet.
/// </summary>
public class HomeManagerScript : MonoBehaviour
{
    void Start()
    {
        if (PersistentDataContainer.Instance.f_startDialogShown)
        {
            Destroy(GameObject.FindWithTag("Popup Canvas"));
        }
        else
        {
            PersistentDataContainer.Instance.f_startDialogShown = true;
        }
    }
}
