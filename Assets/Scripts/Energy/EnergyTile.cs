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
    [CanBeNull] public EnergySource CurrentSource { get; set; }
    public UnityEvent OnPowerEvent { get; private set; }
    public UnityEvent OnDepowerEvent { get; private set; }
    [Tooltip("Level ends when this is powered")] [field: SerializeField] public bool IsTargetTile { get; private set; }
    
    public bool IsGenerator
    {
        get
        {
            if (CurrentSource == null || !CurrentSource.IsGenerator)
                return false;
            return true;
        }
    }

    void Awake()
    {
        OnPowerEvent = new UnityEvent();
        OnDepowerEvent = new UnityEvent();
    }

    void Start()
    {
        if (PreviousTile != null)
        {
            PreviousTile.OnPowerEvent.AddListener(TryPower);
            PreviousTile.OnDepowerEvent.AddListener(OnDepower);
        }
    }

    void OnDrawGizmos()
    {
        if (PreviousTile != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(PreviousTile.transform.position, transform.position);
        }
    }

    public void TryPower()
    {
        if (IsPowered())
            OnPower();
    }

    public bool IsPowered()
    {
        if (IsGenerator && CurrentSource != null || CurrentSource.ReceiveEnergy(PreviousTile.SendEnergy()))
            return true;
        return false;
    }

    [CanBeNull]
    public List<IEnergyType> SendEnergy()
    {
        if (IsPowered())
            return CurrentSource.OutEnergyType;
        return null;
    }

    void OnPower()
    {
        OnPowerEvent.Invoke();
    }

    void OnDepower()
    {
        OnDepowerEvent.Invoke();
    }

    void AddComponentListeners()
    {
        OnPowerEvent.AddListener(CurrentSource.TurnOn);
        OnDepowerEvent.AddListener(CurrentSource.TurnOff);
    }

    void RemoveComponentListeners()
    {
        OnPowerEvent.RemoveListener(CurrentSource.TurnOn);
        OnDepowerEvent.RemoveListener(CurrentSource.TurnOff);
    }

    public void HandleClick(BaseEventData baseEventData)
    {
        EnergyModuleManager.Instance.SelectTile(baseEventData);
    }
}