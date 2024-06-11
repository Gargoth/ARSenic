 using System;
 using System.Collections;
 using System.Collections.Generic;
using AirResistance2;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

 /// <summary>
 /// Handles Energy Module processes
 /// </summary>
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
        
        // Display tutorial dialog if not yet seen
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
        // Handle touch input
        // Fling the target up on touch
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

    /// <summary>
    /// Update gravity based on the slider.
    /// NOte that the slider value is a float between 0 and 1, but the resulting gravity force increases exponentially based on the min and max bounds.
    /// </summary>
    /// <param name="sliderValue"></param>
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

    /// <summary>
    /// Update Air Resistance state based on parameter
    /// </summary>
    /// <param name="isEnabled">True if enabled, else false</param>
    public void UpdateAirResistance(bool isEnabled)
    {
        isAirResOn = isEnabled;
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("GravObject");
        foreach (GameObject obj in spawnedObjects)
        {
            obj.GetComponent<AirResistance>().enabled = isAirResOn;
        }
    }

    /// <summary>
    /// Destroy all gravity objects spawned by the player
    /// </summary>
    public static void ClearItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("GravObject");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
    }

    /// <summary>
    /// Handles gravity object spawning
    /// </summary>
    /// <param name="prefabNum">Index of the gravity object to spawn</param>
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