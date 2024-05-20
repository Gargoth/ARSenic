using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HumanSource : EnergySource
{
    public string Name { get; private set; } = "Human"; // NOTE: Is this needed?
    public bool IsGenerator { get; private set; } = true;
    [CanBeNull] public IEnergyType InAcceptedEnergyType { get; private set; } = new ChemicalEnergy();

    public List<IEnergyType> OutEnergyType { get; private set; } = new List<IEnergyType>
    {
        new MechanicalEnergy(),
    };

    void Awake()
    {
        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/Human");
    }
}
