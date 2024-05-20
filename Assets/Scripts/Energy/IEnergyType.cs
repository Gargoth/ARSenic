using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnergyType
{
    LightEnergy,
    SoundEnergy,
    HeatEnergy,
    ElectricalEnergy,
    MechanicalEnergy,
    ChemicalEnergy
}

public interface IEnergyType
{
    string Name { get; }
    Color Color { get; set; }
    float Speed { get; set; }
}

public class LightEnergy : IEnergyType
{
    public string Name { get; } = "Light";
    public Color Color { get; set; } = Color.yellow;
    public float Speed { get; set; } = 1f;
}

public class SoundEnergy : IEnergyType
{
    public string Name { get; } = "Sound";
    public Color Color { get; set; } = Color.cyan;
    public float Speed { get; set; } = 1f;
}

public class HeatEnergy : IEnergyType
{
    public string Name { get; } = "Heat";
    public Color Color { get; set; } = Color.red;
    public float Speed { get; set; } = 1f;
}

public class ElectricalEnergy : IEnergyType
{
    public string Name { get; } = "Electrical";
    public Color Color { get; set; } = Color.magenta;
    public float Speed { get; set; } = 1f;
}

public class MechanicalEnergy : IEnergyType
{
    public string Name { get; } = "Mechanical";
    public Color Color { get; set; } = Color.blue;
    public float Speed { get; set; } = 1f;
}

public class ChemicalEnergy : IEnergyType
{
    public string Name { get; } = "Chemical";
    public Color Color { get; set; } = Color.green;
    public float Speed { get; set; } = 1f;
}
