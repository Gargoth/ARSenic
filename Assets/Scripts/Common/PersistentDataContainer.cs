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
			"1. Point your camera towards the ground plane. <b>Tap</b> when you see the square.",
            "2. Tap the <b>sphere/cube</b> icon to change the object.", 
            "3. Pick a part of the road to change and select one of the three: <b><color=#72c6f0>ice</color=#72c6f0>, <b><color=#c2cde7>asphalt</b></color=#c2cde7>, <b><color=#9be4bd>grass</b></color=#9be4bd>.",
			"4. <b><color=#72c6f0>Ice</b></color=#72c6f0>: <b>low friction</b>.<b><color=#c2cde7>Asphalt</b></color=#c2cde7>: <b>medium friction</b>.<b><color=#9be4bd>Grass</b></color=#9be4bd>: <b>high friction</b>", 
            "5. Hold the fist button to set an intial push on the object. <b><i>The longer you hold, the stronger you push!</b></i>",
			"6. Click the <b>reset</b> button to try again.",
        }; // List of strings to cycle through
        TopicTutorial["GravityModule"] = new List<string>
        {
			"1. Point your camera towards the ground plane. <b>Tap</b> when you see the square.",
            "2. Drag the slider to set the gravity.",
            "3. Tap the air resistance button to toggle it on/off.",
            "4. Click the inventory button and select an object you want to spawn.",
            "5. Tap on a spawned object to make it jump!",
        }; // List of strings to cycle through
    }
}
