using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script for changing the next topic tutorial message
/// </summary>
public class PopUpNextButton : MonoBehaviour
{
    /// <summary>
    /// Wrapper for GameManagerScript's TextChanger function
    /// </summary>
    /// <param name="displayText">TextMeshProUGUI component to update</param>
    public void ChangeText(TextMeshProUGUI displayText)
    {
        GameManagerScript.Instance.TextChanger(displayText);
    }
}
