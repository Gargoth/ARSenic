using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravModObject : MonoBehaviour
{
    public float floatForce = 0;
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(-Physics.gravity*floatForce, ForceMode.Acceleration);
    }
}
