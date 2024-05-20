using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TVSource : EnergySource
{
    public string Name { get; private set; } = "TV"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public IEnergyType InAcceptedEnergyType { get; private set; } = new ElectricalEnergy();

    public List<IEnergyType> OutEnergyType { get; private set; } = new List<IEnergyType>
    {
        new LightEnergy(),
        new SoundEnergy(),
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/TV");
    }
}
