using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModuleManagerScript : Singleton<GravityModuleManagerScript>
{
    [SerializeField] private GameObject objectContainer;
    
    public void SetActiveObjectContainer(bool value)
    {
        objectContainer.SetActive(value);
    }
}
