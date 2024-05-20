using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StoveSource : EnergySource
{
    public string Name { get; private set; } = "Stove"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public string InAcceptedEnergyType { get; private set; } = "heat";

    public List<string> OutEnergyType { get; private set; } = new List<string>
    {
        "chemical"
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Stove with food");
    }
}
