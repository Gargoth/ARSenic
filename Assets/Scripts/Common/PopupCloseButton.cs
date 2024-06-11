using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for closing the popup canvases
/// </summary>
public class PopupCloseButton : MonoBehaviour
{
    /// <summary>
    /// Local wrapper for GameManagerScript's MoveToScene function
    /// </summary>
    /// <param name="sceneName">Name of the scene to move to</param>
    public void MoveToScene(string sceneName)
    {
        GameManagerScript.MoveToScene(sceneName);
    }
    
    /// <summary>
    /// Recursively searches for the popup canvas' root and destroys it
    /// </summary>
    public void ClosePopupCanvas()
    {
        Transform currentTransform = transform;
        while (!currentTransform.CompareTag("Popup Canvas"))
            currentTransform = currentTransform.parent;
        Destroy(currentTransform.gameObject);
    }
}
