using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class EnergyComponent : MonoBehaviour
{
    public string Name { get; private set; } // NOTE: Is this needed?
    [SerializeField] GameObject componentModelPrefab;
    public bool IsGenerator { get; private set; }
    public IEnergyType InAcceptedEnergyType { get; private set; }
    [CanBeNull] public IEnergyType InEnergyType { get; set; }
    public IEnergyType OutEnergyType { get; private set; }

    public virtual bool ReceiveEnergy(IEnergyType energyType)
    {
        if (energyType == InAcceptedEnergyType)
            return true;
        return false;
    }

    public virtual void TurnOn()
    {
        // TODO: Implementation
        // Enable particles
    }

    public virtual void TurnOff()
    {
        // TODO: Implementation
        // Disable particles
    }
}