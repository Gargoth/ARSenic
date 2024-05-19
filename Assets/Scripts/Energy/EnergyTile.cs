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
    [field: SerializeField] public bool IsGenerator { get; private set; }
    [CanBeNull] public EnergyComponent CurrentComponent { get; set; }
    public UnityEvent OnPowerEvent { get; }
    public UnityEvent OnDepowerEvent { get; }

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
        if (IsGenerator && CurrentComponent != null || CurrentComponent.ReceiveEnergy(PreviousTile.SendEnergy()))
            return true;
        return false;
    }

    [CanBeNull]
    public IEnergyType SendEnergy()
    {
        if (IsPowered())
            return CurrentComponent.OutEnergyType;
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
        OnPowerEvent.AddListener(CurrentComponent.TurnOn);
        OnDepowerEvent.AddListener(CurrentComponent.TurnOff);
    }

    void RemoveComponentListeners()
    {
        OnPowerEvent.RemoveListener(CurrentComponent.TurnOn);
        OnDepowerEvent.RemoveListener(CurrentComponent.TurnOff);
    }

    public void HandleClick(BaseEventData baseEventData)
    {
        EnergyModuleManager.Instance.SelectTile(baseEventData);
    }
}