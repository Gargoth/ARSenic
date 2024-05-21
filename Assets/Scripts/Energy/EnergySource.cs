using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum EnergySourceType
{
    SunSource,
    SolarPanelSource,
    HumanSource,
    StoveSource,
    TVSource,
    GeneratorSource,
}

[RequireComponent(typeof(EnergyTile))]
public class EnergySource : MonoBehaviour
{
    public string Name { get; protected set; } // NOTE: Is this needed?
    [field: SerializeField] public EnergySourceType EnergySourceType { get; set; }
    [field: SerializeField] protected GameObject EnergySourceModelPrefab { get; set; }
    public bool IsGenerator { get; protected set; }
    [field: SerializeField] [CanBeNull] public string InEnergyType { get; private set; }
    [field: SerializeField] public List<string> OutEnergyType { get; private set; }
    GameObject model;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => EnergySourceType != null);
        InitializeEnergySourceFields();

        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/" + Name);
        yield return new WaitUntil(() => EnergySourceModelPrefab != null);
        model = Instantiate(EnergySourceModelPrefab, transform);
        GetComponent<EnergyTile>().UpdatePower();
    }

    void InitializeEnergySourceFields()
    {
        switch (EnergySourceType)
        {
            case (EnergySourceType.SunSource):
                Name = "Sun";
                IsGenerator = true;
                InEnergyType = null;
                OutEnergyType = new List<string>
                {
                    "Light", "Heat"
                };
                break;
            case (EnergySourceType.SolarPanelSource):
                Name = "SolarPanel";
                IsGenerator = false;
                InEnergyType = "Light";
                OutEnergyType = new List<string>
                {
                    "Electrical"
                };
                break;
            case (EnergySourceType.HumanSource):
                Name = "Human";
                IsGenerator = false;
                InEnergyType = "Chemical";
                OutEnergyType = new List<string>
                {
                    "Mechanical"
                };
                break;
            case (EnergySourceType.StoveSource):
                Name = "Stove";
                IsGenerator = false;
                InEnergyType = "Heat";
                OutEnergyType = new List<string>
                {
                    "Chemical"
                };
                break;
            case (EnergySourceType.TVSource):
                Name = "TV";
                IsGenerator = false;
                InEnergyType = "Electrical";
                OutEnergyType = new List<string>
                {
                    "Light",
                    "Sound"
                };
                break;
            case (EnergySourceType.GeneratorSource):
                Name = "Generator";
                IsGenerator = false;
                InEnergyType = "Mechanical";
                OutEnergyType = new List<string>
                {
                    "Electrical"
                };
                break;
        }
    }

    void OnDestroy()
    {
        Destroy(model);
    }

    public virtual bool ReceiveEnergy(List<string> inputEnergyTypes)
    {
        if (InEnergyType != null)
            return inputEnergyTypes.Contains(InEnergyType);
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