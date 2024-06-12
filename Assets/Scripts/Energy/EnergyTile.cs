using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Class used for energy tiles
/// Handles creation and teardown of the energy sources
/// Handles adding and removing listeners
/// </summary>
public class EnergyTile : MonoBehaviour
{
    [field: SerializeField] public EnergyTile PreviousTile { get; set; }
    [field: SerializeField] public bool IsSelectable { get; private set; }
    public bool IsSelected { get; set; } = false;
    [field: SerializeField] [CanBeNull] public EnergySourceType StartWithSource { get; set; }
    [field: SerializeField] [CanBeNull] public EnergySource CurrentSource { get; set; }
    public UnityEvent OnPowerEvent { get; private set; }
    public UnityEvent OnDepowerEvent { get; private set; }
    void Awake()
    {
        OnPowerEvent = new UnityEvent();
        OnDepowerEvent = new UnityEvent();
    }

    void Start()
    {
        if (PreviousTile != null)
        {
            // Attach listeners
            PreviousTile.OnPowerEvent.AddListener(UpdatePower);
            PreviousTile.OnDepowerEvent.AddListener(() => StartCoroutine(OnDepower()));
        }

        if (StartWithSource != EnergySourceType.NoSource)
        {
            // Create new source
            EnergySource newSource = gameObject.AddComponent(typeof(EnergySource)) as EnergySource;
            CurrentSource = newSource;
            newSource.EnergySourceType = StartWithSource;
            AddComponentListeners();
        }
    }

    void OnDrawGizmos()
    {
        if (PreviousTile != null)
        {
            Gizmos.color = Color.red;
            Vector3 prevTileCenter = PreviousTile.GetComponent<Renderer>().bounds.center;
            Vector3 ourTileCenter = GetComponent<Renderer>().bounds.center;
            Gizmos.DrawLine(prevTileCenter, ourTileCenter);
        }
    }

    /// <summary>
    /// Public function for updating energy source state
    /// </summary>
    public void UpdatePower()
    {
        if (IsPowered())
            StartCoroutine(OnPower());
        else
            StartCoroutine(OnDepower());
    }

    /// <summary>
    /// Checks if the tile is powered
    /// Powered if the current source is a generator or if the previous tile is powered and the current source uses the same energy type as the previous tile's output
    /// </summary>
    /// <returns>True if powered, else false</returns>
    public bool IsPowered()
    {
        bool generatorCheck = CurrentSource != null && CurrentSource != null && CurrentSource.IsGenerator;

        if (generatorCheck)
            return true;

        bool prevPowerCheck = CurrentSource != null && PreviousTile != null && PreviousTile.CurrentSource != null;

        if (prevPowerCheck)
        {
            if (Enumerable.Intersect(PreviousTile.CurrentSource.OutEnergyType, CurrentSource.InEnergyType).Any())
                return true;
        }

        return false;
    }

    /// <summary>
    /// Update energy source particle system target
    /// Called from UpdatePower
    /// </summary>
    IEnumerator OnPower()
    {
        Debug.Log(name + " is powered");
        OnPowerEvent.Invoke();

        if (PreviousTile != null)
            yield return new WaitUntil(() => PreviousTile.CurrentSource != null);
            yield return new WaitUntil(() => CurrentSource.Model!= null);
            PreviousTile?.CurrentSource?.SetParticleTarget(CurrentSource?.ModelCenterTransform());
    }

    /// <summary>
    /// Remove energy source particle system target
    /// Called from UpdatePower
    /// </summary>
    IEnumerator OnDepower()
    {
        Debug.Log(name + " is depowered");
        OnDepowerEvent.Invoke();
        if (PreviousTile != null)
            yield return new WaitUntil(() => PreviousTile.CurrentSource!= null);
            yield return new WaitUntil(() => PreviousTile?.CurrentSource?.Model!= null);
            PreviousTile.CurrentSource.SetParticleTarget(PreviousTile.CurrentSource.ModelCenterTransform());
    }

    /// <summary>
    /// Add listeners for OnPowerEvent and OnDepowerEvent
    /// </summary>
    public void AddComponentListeners()
    {
        OnPowerEvent.AddListener(CurrentSource.TurnOn);
        OnDepowerEvent.AddListener(CurrentSource.TurnOff);
    }

    /// <summary>
    /// Remove listeners for OnPowerEvent and OnDepowerEvent
    /// </summary>
    public void RemoveComponentListeners()
    {
        OnPowerEvent.RemoveListener(CurrentSource.TurnOn);
        OnDepowerEvent.RemoveListener(CurrentSource.TurnOff);
    }

    /// <summary>
    /// Click handler to be used for the event system
    /// </summary>
    /// <param name="baseEventData"></param>
    public void HandleClick(BaseEventData baseEventData)
    {
        EnergyModuleManager.Instance.SelectTile(baseEventData);
    }
}