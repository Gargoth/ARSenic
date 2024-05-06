using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataContainer : Singleton<PersistentDataContainer>
{
    // Flags
    public bool f_startDialogShown = false;
    public bool f_frictionDialogShown = false;
    public bool f_gravityDialogShown = false;
    
    // Fields
    public int selectedLevel;
    public GameObject popupCanvasPrefab;
    
    public Dictionary<string, List<string>> TopicTutorial = new Dictionary<string, List<string>>();
    
    void Start()
    {
        TopicTutorial["FrictionModule"] = new List<string>
        {
            "Pick an object to push (sphere or cube).", 
            "Select a track surface and place it on the road.", 
            "Hold the fist button to push the object. The longer you hold, the stronger the push!",
            "Use your knowledge of friction to reach the flag!"
        }; // List of strings to cycle through
        TopicTutorial["GravityModule"] = new List<string>
        {
            "Drag the slider to set your chosen gravity.",
            "Tap the air resistance button to toggle it.",
            "Tap an object from the inventory to spawn it.",
            "Tap on a spawned object to make it jump.",
            "Explore and observe the effects of gravity on different objects!",
        }; // List of strings to cycle through
    }
}
