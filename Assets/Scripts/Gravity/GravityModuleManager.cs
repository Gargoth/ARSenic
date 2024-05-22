 using System;
 using System.Collections;
 using System.Collections.Generic;
using AirResistance2;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GravityModuleManagerScript : Singleton<GravityModuleManagerScript>
{
    GameObject objectContainer;
    [SerializeField] List<GameObject> spawnPrefabs;
    [SerializeField] float gravityForce;
    bool isAirResOn;
    RaycastHit hit;

    IEnumerator Start()
    {
        isAirResOn = true;
        
        if (PersistentDataContainer.Instance.f_gravityDialogShown)
        {
            Destroy(GameObject.FindWithTag("Popup Canvas"));
        }
        else
        {
            PersistentDataContainer.Instance.f_gravityDialogShown = true;
        }
        yield return new WaitUntil(() => ARManager.Instance != null);
        objectContainer = ARManager.Instance.objectContainer;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            var mainCam = Camera.main;
            for (int i = 0; i < Input.touchCount; i++)
           {
               if (!objectContainer.activeInHierarchy) continue;
               Touch touch = Input.GetTouch(0);
               if (touch.phase != TouchPhase.Began) continue;
               Ray ray = mainCam.ScreenPointToRay(touch.position);
               // ChangeDebugText("Raycast initialized");
               int mask = ~(1 << 2);
               if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
               {
                   // ChangeDebugText("Raycast hit " + hit.collider.name);

                   if (!hit.collider.CompareTag("GravObject")) continue;

                   // ChangeDebugText(Camera.main.WorldToScreenPoint(hit.point).ToString());
                   hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * 125);
                   // ChangeDebugText("Applied force to " + hit.collider.name);
               }
           }
        }
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
        Physics.gravity = Vector3.down * (gravityForce);
    }

    public void UpdateAirResistance(bool isEnabled)
    {
        isAirResOn = isEnabled;
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("GravObject");
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<AirResistance>().enabled = isAirResOn;
        }
    }

    public static void ClearItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("GravObject");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
    }

    public void SpawnPrefab(int prefabNum)
    {
        if (prefabNum < 0 || prefabNum >= spawnPrefabs.Count) return;
        GameObject prefabObject = spawnPrefabs[prefabNum];
        GameObject newObject = Instantiate(prefabObject, objectContainer.transform.position + Vector3.up*0.25f,
            Random.rotation);
        newObject.SetActive(true);
        newObject.GetComponent<AirResistance>().enabled = isAirResOn;
    }
}