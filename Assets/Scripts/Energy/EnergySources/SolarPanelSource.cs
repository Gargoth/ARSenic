using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SolarPanelSource : EnergySource
{
    public string Name { get; private set; } = "SolarPanel"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public string InAcceptedEnergyType { get; private set; } = "light";

    public List<string> OutEnergyType { get; private set; } = new List<string>
    {
        "electrical"
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Solar Panel");
    }
}
