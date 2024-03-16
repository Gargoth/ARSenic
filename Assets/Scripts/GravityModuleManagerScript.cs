using System;
using System.Collections;
using System.Collections.Generic;
using AirResistance2;
using TMPro;
using UnityEngine;
using Vuforia;
using Random = UnityEngine.Random;

public class GravityModuleManagerScript : Singleton<GravityModuleManagerScript>
{
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private ContentPositioningBehaviour _contentPositioningBehaviour;
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private RectTransform removePanel;
    [SerializeField] private float gravityForce;

    void Update()
    {
        if (Input.touchCount == 1) {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!objectContainer.activeInHierarchy) continue;
                Touch touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began) continue;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                changeDebugText("Raycast initialized");
                if (Physics.Raycast(ray, out hit))
                {
                    changeDebugText("Raycast hit " + hit.collider.name);
                    if (!hit.collider.CompareTag("Toothbrush")) continue;
                    removePanel.gameObject.SetActive(true);
                    removePanel.anchoredPosition3D = (Vector2)Camera.main.WorldToScreenPoint(hit.point);
                    changeDebugText(Camera.main.WorldToScreenPoint(hit.point).ToString());
                    
                    // TODO: Remove panel on tap outside of ui
                    // TODO: Spawn panel to the left if the object is too far right
                }
            }
        }
        else if (Input.touchCount == 2)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!objectContainer.activeInHierarchy) continue;
                Touch touch = Input.GetTouch(i);
                if (touch.phase != TouchPhase.Began) continue;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                changeDebugText("Raycast initialized");
                if (Physics.Raycast(ray, out hit))
                {
                    changeDebugText("Raycast hit " + hit.collider.name);
                    if (!hit.collider.CompareTag("Toothbrush")) continue;
                    changeDebugText("Deleted " + hit.collider.name);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        else if (Input.touchCount == 3)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!objectContainer.activeInHierarchy) continue;
                Touch touch = Input.GetTouch(i);
                if (touch.phase != TouchPhase.Began) continue;
                if (spawnPrefabs.Count == 0) continue;
                GameObject prefabObject = spawnPrefabs[Random.Range(0, spawnPrefabs.Count - 1)];
                GameObject newObject = Instantiate(prefabObject, objectContainer.transform.position + Vector3.up,
                    Random.rotation);
                newObject.GetComponent<GravModObject>().gravityForce = gravityForce;
                newObject.SetActive(true);
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
}