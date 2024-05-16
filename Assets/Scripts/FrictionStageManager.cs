using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrictionStageManager : Singleton<FrictionStageManager>
{
    [Tooltip("In seconds")] [SerializeField] float minThreeStarTime;
    [Tooltip("In seconds")] [SerializeField] float minTwoStarTime;
    float targetDuration = 2f;
    FrictionModuleManager frictionModuleManager;
    void Start()
    {
        frictionModuleManager = FrictionModuleManager.Instance;
    }

    public int GetStars(float totalTime)
    {
        if (totalTime < minThreeStarTime)
            return 3;
        else if (totalTime < minTwoStarTime)
            return 2;
        else
            return 1;
    }

    public float GetTargetDuration()
    {
        return targetDuration;
    }

    public void RoadClickHandler(BaseEventData baseEventData)
    {
        frictionModuleManager.OnRoadClick(baseEventData);
    }
}
