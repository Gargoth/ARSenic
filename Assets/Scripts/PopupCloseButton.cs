using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCloseButton : MonoBehaviour
{
    // WARN: Unsafe due to possible infinite loop; TODO: Add max loop limit
    public void ClosePopupCanvas()
    {
        Transform currentTransform = transform;
        while (!currentTransform.CompareTag("Popup Canvas"))
            currentTransform = currentTransform.parent;
        Destroy(currentTransform.gameObject);
    }
}
