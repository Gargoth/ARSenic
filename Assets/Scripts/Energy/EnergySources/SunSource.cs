using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SunSource : EnergySource
{
    public string Name { get; private set; } = "Sun"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;

    public List<IEnergyType> OutEnergyType { get; private set; } = new List<IEnergyType>
    {
        new LightEnergy(),
        new HeatEnergy(),
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Sun");
    }
}
