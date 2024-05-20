using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SunSource : EnergySource
{
    [field: SerializeField]
    public List<string> OutEnergyType { get; private set; }

    void Awake()
    {
        Name = "Sun";
        IsGenerator = true;
        OutEnergyType = new List<string> { "light", "heat" };
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Sun");
    }
}
