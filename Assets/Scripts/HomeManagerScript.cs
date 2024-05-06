using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
