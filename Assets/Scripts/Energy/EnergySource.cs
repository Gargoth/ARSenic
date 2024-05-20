using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(EnergyTile))]
public abstract class EnergySource : MonoBehaviour
{
    public string Name { get; protected set; } // NOTE: Is this needed?
    [field: SerializeField] protected GameObject EnergySourceModelPrefab { get; set; }
    public bool IsGenerator { get; protected set; }
    [CanBeNull] public string InAcceptedEnergyType { get; private set; }
    [CanBeNull] public string InEnergyType { get; set; }
    public List<string> OutEnergyType { get; private set; }
    GameObject model;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        model = Instantiate(EnergySourceModelPrefab, transform);
        GetComponent<EnergyTile>().UpdatePower();
    }

    void OnDestroy()
    {
        Debug.Log("Energy source destroyed, destroying attached model");
        Destroy(model);
    }

    public virtual bool ReceiveEnergy(List<string> inputEnergyTypes)
    {
        if (InAcceptedEnergyType != null)
            return inputEnergyTypes.Contains(InAcceptedEnergyType);
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