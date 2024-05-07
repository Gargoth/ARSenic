using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrictionStageManager : Singleton<FrictionStageManager>
{
    [SerializeField] GameObject spawnObject;
    FrictionModuleManager frictionModuleManager;
    Vector3 spawnPos;
    void Start()
    {
        frictionModuleManager = FrictionModuleManager.Instance;
        spawnPos = spawnObject.transform.position;
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
