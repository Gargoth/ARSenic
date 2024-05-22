using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpNextButton : MonoBehaviour
{
    public void ChangeText(TextMeshProUGUI displayText)
    {
        GameManagerScript.Instance.TextChanger(displayText);
    }
}
