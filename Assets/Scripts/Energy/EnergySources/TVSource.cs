using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TVSource : EnergySource
{
    public string Name { get; private set; } = "TV"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public string InAcceptedEnergyType { get; private set; } = "electrical";

    public List<string> OutEnergyType { get; private set; } = new List<string>
    {
        "light",
        "sound"
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/TV");
    }
}
