using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrictionStageManager : Singleton<FrictionStageManager>
{
    [Tooltip("In seconds")] [SerializeField] float minThreeStarTime;
    [Tooltip("In seconds")] [SerializeField] float minTwoStarTime;
    FrictionModuleManager frictionModuleManager;
    /// <summary>
    /// Handles metadata for each stage
    /// </summary>
    void Start()
    {
        frictionModuleManager = FrictionModuleManager.Instance;
    }

    /// <summary>
    /// Gets number of stars based on total time taken
    /// </summary>
    /// <param name="totalTime">Total time taken</param>
    /// <returns>Number of stars rewarded</returns>
    public int GetStars(float totalTime)
    {
        if (totalTime < minThreeStarTime)
            return 3;
        else if (totalTime < minTwoStarTime)
            return 2;
        else
            return 1;
    }

    /// <summary>
    /// Wrapper for frictionModuleManager's OnRoadClick function
    /// Called by event system
    /// </summary>
    /// <param name="baseEventData"></param>
    public void RoadClickHandler(BaseEventData baseEventData)
    {
        frictionModuleManager.OnRoadClick(baseEventData);
    }
}
