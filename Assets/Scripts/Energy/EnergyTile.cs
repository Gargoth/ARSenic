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
    [field: SerializeField] [CanBeNull] public EnergySource CurrentSource { get; set; }
    public UnityEvent OnPowerEvent { get; private set; }
    public UnityEvent OnDepowerEvent { get; private set; }

    [Tooltip("Level ends when this is powered")]
    [field: SerializeField]
    public bool IsTargetTile { get; private set; }

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

    public void UpdatePower()
    {
        if (IsPowered())
            OnPower();
        else
            OnDepower();
    }

    public bool IsPowered()
    {
        bool generatorCheck = CurrentSource != null && CurrentSource != null && CurrentSource.IsGenerator;

        if (generatorCheck)
            return true;

        bool prevPowerCheck = CurrentSource != null && PreviousTile != null && PreviousTile.CurrentSource != null;

        if (prevPowerCheck)
        {
            Debug.Log(PreviousTile.SendEnergy());
            Debug.Log(CurrentSource.InAcceptedEnergyType);
            
            if (PreviousTile.CurrentSource.OutEnergyType.Contains(CurrentSource.InAcceptedEnergyType))
                return true;
        }

        return false;
    }

    [CanBeNull]
    public List<string> SendEnergy()
    {
        if (IsPowered())
            return CurrentSource.OutEnergyType;
        return null;
    }

    void OnPower()
    {
        Debug.Log(name + " is powered");
        OnPowerEvent.Invoke();
    }

    void OnDepower()
    {
        Debug.Log(name + " is depowered");
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