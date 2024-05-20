using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class EnergySource : MonoBehaviour
{
    public string Name { get; private set; } // NOTE: Is this needed?
    [field: SerializeField] protected GameObject EnergySourceModelPrefab { get; set; }
    public bool IsGenerator { get; private set; }
    [CanBeNull] public IEnergyType InAcceptedEnergyType { get; private set; }
    [CanBeNull] public IEnergyType InEnergyType { get; set; }
    public List<IEnergyType> OutEnergyType { get; private set; }
    GameObject model;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        model = Instantiate(EnergySourceModelPrefab, transform);
    }

    void OnDestroy()
    {
        Debug.Log("Energy source destroyed, destroying attached model");
        Destroy(model);
    }

    public virtual bool ReceiveEnergy(List<IEnergyType> inputEnergyTypes)
    {
        return inputEnergyTypes.Contains(InAcceptedEnergyType);
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
