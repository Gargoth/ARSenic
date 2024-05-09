using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCloseButton : MonoBehaviour
{
    public void MoveToScene(string sceneName)
    {
        GameManagerScript.MoveToScene(sceneName);
    }
    
    public void ClosePopupCanvas()
    {
        Transform currentTransform = transform;
        while (!currentTransform.CompareTag("Popup Canvas"))
            currentTransform = currentTransform.parent;
        Destroy(currentTransform.gameObject);
    }
}
