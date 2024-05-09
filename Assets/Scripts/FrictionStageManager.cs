using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrictionStageManager : Singleton<FrictionStageManager>
{
    [SerializeField] GameObject spawnObject;
    float targetDuration = 2f;
    FrictionModuleManager frictionModuleManager;
    Vector3 spawnPos;
    void Start()
    {
        frictionModuleManager = FrictionModuleManager.Instance;
        spawnPos = spawnObject.transform.position;
    }

    public float GetTargetDuration()
    {
        return targetDuration;
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos;
    }

    public void RoadClickHandler(BaseEventData baseEventData)
    {
        frictionModuleManager.OnRoadClick(baseEventData);
    }
}
