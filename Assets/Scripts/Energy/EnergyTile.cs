using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnergyTile : MonoBehaviour
{
    [field: SerializeField] public EnergyTile PreviousTile { get; private set; }
    [field: SerializeField] public bool IsSelectable { get; private set; }
    public bool IsSelected { get; set; } = false;
    [field: SerializeField] [CanBeNull] public EnergySourceType StartWithSource { get; set; }
    [field: SerializeField] [CanBeNull] public EnergySource CurrentSource { get; set; }
    public UnityEvent OnPowerEvent { get; private set; }
    public UnityEvent OnDepowerEvent { get; private set; }

    [Tooltip("Level ends when this is powered")]
    [field: SerializeField]
    public bool IsFinalTile { get; private set; }

    void Awake()
    {
        OnPowerEvent = new UnityEvent();
        OnDepowerEvent = new UnityEvent();
    }

    void Start()
    {
        if (PreviousTile != null)
        {
            PreviousTile.OnPowerEvent.AddListener(UpdatePower);
            PreviousTile.OnDepowerEvent.AddListener(() => StartCoroutine(OnDepower()));
        }

        if (StartWithSource != EnergySourceType.NoSource)
        {
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

    public void UpdatePower()
    {
        if (IsPowered())
            StartCoroutine(OnPower());
        else
            StartCoroutine(OnDepower());
    }

    public bool IsPowered()
    {
        bool generatorCheck = CurrentSource != null && CurrentSource != null && CurrentSource.IsGenerator;

        if (generatorCheck)
            return true;

        bool prevPowerCheck = CurrentSource != null && PreviousTile != null && PreviousTile.CurrentSource != null;

        if (prevPowerCheck)
        {
            if (PreviousTile.CurrentSource.OutEnergyType.Contains(CurrentSource.InEnergyType))
                return true;
        }

        return false;
    }

    IEnumerator OnPower()
    {
        Debug.Log(name + " is powered");
        OnPowerEvent.Invoke();

        if (IsFinalTile)
        {
            EnergyModuleManager.Instance.FinishLevel();
        }

        if (PreviousTile != null)
            yield return new WaitUntil(() => PreviousTile.CurrentSource != null);
            yield return new WaitUntil(() => CurrentSource.Model!= null);
            PreviousTile.CurrentSource.SetParticleTarget(CurrentSource.ModelCenterTransform());
    }

    IEnumerator OnDepower()
    {
        Debug.Log(name + " is depowered");
        OnDepowerEvent.Invoke();
        if (PreviousTile != null)
            yield return new WaitUntil(() => PreviousTile.CurrentSource!= null);
            yield return new WaitUntil(() => PreviousTile.CurrentSource.Model!= null);
            PreviousTile.CurrentSource.SetParticleTarget(PreviousTile.CurrentSource.ModelCenterTransform());
    }

    public void AddComponentListeners()
    {
        OnPowerEvent.AddListener(CurrentSource.TurnOn);
        OnDepowerEvent.AddListener(CurrentSource.TurnOff);
    }

    public void RemoveComponentListeners()
    {
        OnPowerEvent.RemoveListener(CurrentSource.TurnOn);
        OnDepowerEvent.RemoveListener(CurrentSource.TurnOff);
    }

    public void HandleClick(BaseEventData baseEventData)
    {
        EnergyModuleManager.Instance.SelectTile(baseEventData);
    }
}