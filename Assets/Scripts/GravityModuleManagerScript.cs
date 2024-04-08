using System;
using System.Collections;
using System.Collections.Generic;
using AirResistance2;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using Random = UnityEngine.Random;

public class GravityModuleManagerScript : Singleton<GravityModuleManagerScript>
{
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private ContentPositioningBehaviour _contentPositioningBehaviour;
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private float gravityForce;
    private RaycastHit hit;

    void Update()
    {
        if (Input.touchCount == 1) {
           for (int i = 0; i < Input.touchCount; i++)
           {
               if (!objectContainer.activeInHierarchy) continue;
               Touch touch = Input.GetTouch(0);
               if (touch.phase != TouchPhase.Began) continue;
               Ray ray = Camera.main.ScreenPointToRay(touch.position);
               changeDebugText("Raycast initialized");
                
               if (Physics.Raycast(ray, out hit))
               {
                   changeDebugText("Raycast hit " + hit.collider.name);

                   if (!hit.collider.CompareTag("Toothbrush")) continue;

                   changeDebugText(Camera.main.WorldToScreenPoint(hit.point).ToString());
                   hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * 75);
                   changeDebugText("Applied force to " + hit.collider.name);
               }
           }
        }
    }

    public void HandlePlaneFinderHit(HitTestResult hitTestResult)
    {
        if (!objectContainer.activeInHierarchy)
            _contentPositioningBehaviour.PositionContentAtPlaneAnchor(hitTestResult);
    }

    public void SetActiveObjectContainer(bool value)
    {
        objectContainer.SetActive(value);
    }

    public void UpdateGravity(Single sliderValue)
    {
        float x = 0;
        float y = 9.81f;
        float z = 274;
        float A = (x*z - Mathf.Pow(y,2)) / (x - 2*y + z);
        float B = Mathf.Pow((y-x),2) / (x - 2 * y + z);
        float C = 2 * Mathf.Log((z-y) / (y-x));
        gravityForce = (A + B * Mathf.Exp(C * sliderValue));
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("Toothbrush");
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<GravModObject>().gravityForce = gravityForce;
        }
    }

    public void UpdateAirResistance(bool isEnabled)
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("Toothbrush");
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<AirResistance>().enabled = isEnabled;
        }
    }

    public void changeDebugText(string val)
    {
        _debugText.text = val;
    }

    public static void ClearItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Toothbrush");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
    }

    public void SpawnPrefab(int prefabNum)
    {
        changeDebugText("Try spawn prefab " + prefabNum + " out of " + spawnPrefabs.Count + " prefabs");
        if (prefabNum < 0 || prefabNum >= spawnPrefabs.Count) return;
        changeDebugText("Attempt to spawn prefab " + prefabNum);
        GameObject prefabObject = spawnPrefabs[prefabNum];
        GameObject newObject = Instantiate(prefabObject, objectContainer.transform.position + Vector3.up,
            Random.rotation);
        newObject.GetComponent<GravModObject>().gravityForce = gravityForce;
        changeDebugText("Prefab spawned");
        newObject.SetActive(true);
        changeDebugText("Prefab spawned DONE");
    }
}