using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrictionStageManager : MonoBehaviour
{
    FrictionModuleManager frictionModuleManager;
    void Start()
    {
        frictionModuleManager = FrictionModuleManager.Instance;
    }

    public void RoadClickHandler(BaseEventData baseEventData)
    {
        frictionModuleManager.OnRoadClick(baseEventData);
    }
}
