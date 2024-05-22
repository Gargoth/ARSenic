using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum EnergySourceType
{
    NoSource,
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
    static Dictionary<string, Color> _energyTypeColor = new Dictionary<string, Color>()
    {
        { "Light", Color.yellow },
        { "Heat", Color.red },
        { "Sound", Color.magenta },
        { "Mechanical", Color.gray },
        { "Electrical", Color.blue },
        { "Chemical", Color.green },
    };

    public string Name { get; protected set; } // NOTE: Is this needed?
    [field: SerializeField] public EnergySourceType EnergySourceType { get; set; }
    [field: SerializeField] protected GameObject EnergySourceModelPrefab { get; set; }
    public bool IsGenerator { get; protected set; }
    [field: SerializeField] public List<string> InEnergyType { get; private set; }
    [field: SerializeField] public List<string> OutEnergyType { get; private set; }
    public List<GameObject> currentParticleSystems;
    public GameObject Model { get; private set; }
    Transform modelTransform;

    void Awake()
    {
        List<GameObject> currentParticleSystems = new List<GameObject>();
    }

    IEnumerator Start()
    {
        Debug.Log(EnergySourceType.HumanSource.ToString());
        Debug.Log(name + " waiting for EnergySourceType != null");
        yield return new WaitUntil(() => EnergySourceType != null);
        Debug.Log(name + " EnergySourceType = " + EnergySourceType);
        InitializeEnergySourceFields();

        EnergySourceModelPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/" + Name);
        yield return new WaitUntil(() => EnergySourceModelPrefab != null);
        Model = Instantiate(EnergySourceModelPrefab, transform);
        GetComponent<EnergyTile>().UpdatePower();
    }

    public Transform ModelCenterTransform()
    {
        if (modelTransform == null)
            modelTransform = new GameObject().transform;
        modelTransform.position = Model.GetComponentInChildren<Renderer>().bounds.center;
        return modelTransform;
    }

    List<Color> GetOutColors()
    {
        List<Color> outColors = new List<Color>();
        foreach (string outEnergy in OutEnergyType)
        {
            if (_energyTypeColor.ContainsKey(outEnergy))
                outColors.Add(_energyTypeColor[outEnergy]);
        }

        return outColors;
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
                InEnergyType = new List<string> { "Light" };
                OutEnergyType = new List<string> { "Electrical" };
                break;
            case (EnergySourceType.HumanSource):
                Name = "Human";
                IsGenerator = false;
                InEnergyType = new List<string> { "Chemical" };
                OutEnergyType = new List<string> { "Mechanical" };
                break;
            case (EnergySourceType.StoveSource):
                Name = "Stove";
                IsGenerator = false;
                InEnergyType = new List<string> { "Heat", "Electrical" };
                OutEnergyType = new List<string> { "Chemical" };
                break;
            case (EnergySourceType.TVSource):
                Name = "TV";
                IsGenerator = false;
                InEnergyType = new List<string> { "Electrical" };
                OutEnergyType = new List<string>
                {
                    "Light",
                    "Sound"
                };
                break;
            case (EnergySourceType.GeneratorSource):
                Name = "Generator";
                IsGenerator = false;
                InEnergyType = new List<string> { "Mechanical" };
                OutEnergyType = new List<string> { "Electrical" };
                break;
        }
    }

    void OnDestroy()
    {
        TurnOff();
        Destroy(Model);
    }

    public virtual bool ReceiveEnergy(List<string> inputEnergyTypes)
    {
        if (InEnergyType != null)
            return Enumerable.Intersect(inputEnergyTypes, InEnergyType).Any();
        return false;
    }

    public virtual void SetParticleTarget(Transform target)
    {
        if (currentParticleSystems == null)
            currentParticleSystems = new List<GameObject>();

        foreach (GameObject particleSystem in currentParticleSystems)
        {
            particleAttractorLinear attractor = particleSystem.GetComponentInChildren<particleAttractorLinear>();
            attractor.target = target;
        }
    }

    public virtual void TurnOn()
    {
        GameObject particleSystemPrefab = Resources.Load<GameObject>("Prefabs/Energy Sources/EnergySource Particles");
        foreach (Color outColor in GetOutColors())
        {
            GameObject newParticleSystemGameObject = Instantiate(particleSystemPrefab, transform);
            if (currentParticleSystems == null)
                currentParticleSystems = new List<GameObject>();
            currentParticleSystems.Add(newParticleSystemGameObject);
            newParticleSystemGameObject.transform.position = ModelCenterTransform().position;

            particleAttractorLinear attractor =
                newParticleSystemGameObject.GetComponentInChildren<particleAttractorLinear>();
            attractor.target = Model.transform.GetChild(0);

            ParticleSystem particleSystem = attractor.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            particleSystemMain.startColor = outColor;
        }
    }

    public virtual void TurnOff()
    {
        if (currentParticleSystems == null || currentParticleSystems.Count == 0)
            return;

        for (int i = currentParticleSystems.Count - 1; i >= 0; i--)
        {
            Destroy(currentParticleSystems[i]);
            currentParticleSystems.RemoveAt(i);
        }
    }
}