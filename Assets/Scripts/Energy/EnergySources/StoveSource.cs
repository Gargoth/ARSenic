using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StoveSource : EnergySource
{
    public string Name { get; private set; } = "Stove"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public IEnergyType InAcceptedEnergyType { get; private set; } = new HeatEnergy();

    public List<IEnergyType> OutEnergyType { get; private set; } = new List<IEnergyType>
    {
        new ChemicalEnergy(),
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Stove with food");
    }
}
