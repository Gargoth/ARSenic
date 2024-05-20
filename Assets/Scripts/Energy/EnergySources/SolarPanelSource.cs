using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SolarPanelSource : EnergySource
{
    public string Name { get; private set; } = "SolarPanel"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public IEnergyType InAcceptedEnergyType { get; private set; } = new LightEnergy();

    public List<IEnergyType> OutEnergyType { get; private set; } = new List<IEnergyType>
    {
        new ElectricalEnergy(),
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Solar Panel");
    }
}
