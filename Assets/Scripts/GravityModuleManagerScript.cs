using System;
using System.Collections;
using System.Collections.Generic;
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
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.up * 200);
                    changeDebugText("Added force to " + hit.collider.name);
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

                GameObject newObject = Instantiate(prefabObject, objectContainer.transform.position + Vector3.up, Random.rotation);
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

    public void changeDebugText(string val)
    {
        _debugText.text = val;
    }
}