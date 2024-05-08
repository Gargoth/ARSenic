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
			"1. Point your camera towards the ground plane. Tap it when you see the square.",
            "2. Tap the sphere/cube icon to change the object.", 
            "3. Pick a part of the road and choose between the ice, asphalt, and grass to change the selected road.", 
            "4. Hold the fist button to set an intial push on the object. The longer you hold, the stronger you push!",
			"5. Click the reset button to try again.",
        }; // List of strings to cycle through
        TopicTutorial["GravityModule"] = new List<string>
        {
			"1. Point your camera towards the ground plane. Tap it when you see the square.",
            "2. Drag the slider to set the gravity.",
            "3. Tap the air resistance button to toggle it on/off.",
            "4. Click the inventory button and select an object you want to spawn.",
            "5. Tap on a spawned object to make it jump!",
        }; // List of strings to cycle through
    }
}
