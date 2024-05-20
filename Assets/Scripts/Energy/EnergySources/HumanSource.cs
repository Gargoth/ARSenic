using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HumanSource : EnergySource
{
    public string Name { get; private set; } = "Human"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public string InAcceptedEnergyType { get; private set; } = "chemical";

    public List<string> OutEnergyType { get; private set; } = new List<string>
    {
        "mechanical"
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Human");
    }
}
