using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataContainer : Singleton<PersistentDataContainer>
{
    public int selectedLevel;
    public GameObject popupCanvasPrefab;
    
    public Dictionary<string, List<string>> TopicTutorial = new Dictionary<string, List<string>>();
    
    void Start()
    {
        TopicTutorial["friction"] = new List<string>
        {
            "Pick an object to push (sphere or cube).", 
            "Select a track surface and place it on the road.", 
            "Hold the fist button to push the object. The longer you hold, the stronger the push!",
            "Use your knowledge of friction to reach the flag!"
        }; // List of strings to cycle through
    }
}
