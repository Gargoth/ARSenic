using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Enum used for all energy sources
/// </summary>
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

/// <summary>
/// Class used for all energy sources.
/// Requires an EnergyTile component to attach to.
/// Contains information on the models, the particle systems, and the energy type I/O
/// </summary>
[RequireComponent(typeof(EnergyTile))]
public class EnergySource : MonoBehaviour
{
    /// <summary>
    /// Handles color for each energy type
    /// Only particles with whose key exists here will be created
    /// </summary>
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
    [Tooltip("Model of an energy source. Do not set manually")] [field: SerializeField] protected GameObject EnergySourceModelPrefab { get; set; }
    public bool IsGenerator { get; protected set; }
    [Tooltip("List of all acceptable energy types. Do not set manually")] [field: SerializeField] public List<string> InEnergyType { get; private set; }
    [Tooltip("List of all output energy types. Do not set manually")] [field: SerializeField] public List<string> OutEnergyType { get; private set; }
    [Tooltip("List of all currently active particle systems. Do not set manually")] public List<GameObject> currentParticleSystems;
    [Tooltip("Easy reference to the energy source model. Do not set manually")] public GameObject Model { get; private set; }
    Transform modelTransform;

    void Awake()
    {
        List<GameObject> currentParticleSystems = new List<GameObject>();
    }

    IEnumerator Start()
    {
        // TODO: Docs
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

    /// <summary>
    /// Returns a transform positioned at the center of the model
    /// </summary>
    /// <returns>Transform positioned at the center of the model</returns>
    public Transform ModelCenterTransform()
    {
        if (modelTransform == null)
            modelTransform = new GameObject().transform;
        modelTransform.position = Model.GetComponentInChildren<Renderer>().bounds.center;
        return modelTransform;
    }

    /// <summary>
    /// Gets all colors of the output energy types
    /// </summary>
    /// <returns>List of energy colors</returns>
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

    /// <summary>
    /// Automatically sets the energy source fields based on the energy source type
    /// </summary>
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

    /// <summary>
    /// Checks if any of the input energy types are in the list of acceptable input energy types
    /// </summary>
    /// <param name="inputEnergyTypes">List of all input energy types</param>
    /// <returns>True if an input energy is valid, else false</returns>
    public virtual bool ReceiveEnergy(List<string> inputEnergyTypes)
    {
        if (InEnergyType != null)
            return Enumerable.Intersect(inputEnergyTypes, InEnergyType).Any();
        return false;
    }

    /// <summary>
    /// Sets the destination of the particle systems
    /// Usually the output of ModelCenterTransform
    /// </summary>
    /// <param name="target">Transform of the target</param>
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

    /// <summary>
    /// Handles the creation of the particle systems
    /// Called by listener
    /// </summary>
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

    /// <summary>
    /// Handles the teardown of the particle systems
    /// Called by listener
    /// </summary>
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