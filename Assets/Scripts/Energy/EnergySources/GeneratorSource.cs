using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GeneratorSource : EnergySource
{
    public string Name { get; private set; } = "Generator"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public string InAcceptedEnergyType { get; private set; } = "mechanical";

    public List<string> OutEnergyType { get; private set; } = new List<string>
    {
        "electrical"
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Generator");
    }
}
