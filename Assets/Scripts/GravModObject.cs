using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravModObject : MonoBehaviour
{
    public float gravityForce;
    public float floatForce;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.down * (gravityForce - floatForce), ForceMode.Acceleration);
    }
}
