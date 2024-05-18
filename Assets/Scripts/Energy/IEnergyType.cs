using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergyType
{
    string Name { get; }
    Color Color { get; set; }
    float Speed { get; set; }
}
