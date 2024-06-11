using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionPlayerController : MonoBehaviour
{
    [SerializeField] Mesh cubeMesh;
    [SerializeField] Mesh sphereMesh;
    [Tooltip("Force exerted when push progress reaches maximum.")] [SerializeField] float maxPushForce;
    Vector3 initialPos;
    Quaternion initialRot;
    Transform directionObject;
    MeshFilter meshFilter;
    Rigidbody rb;
    BoxCollider boxCollider;
    SphereCollider sphereCollider;
    
    IEnumerator Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        directionObject = transform.Find("Direction");
        yield return new WaitForEndOfFrame();
        Debug.Log("Adding ResetPlayer Listener");
        FrictionModuleManager.Instance.ResetActions.AddListener(ResetPlayer);
        SetInitialState();
    }

    /// <summary>
    /// Set fields relating to initial player state
    /// </summary>
    public void SetInitialState()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    /// <summary>
    /// Use push coefficient and max push force to push player
    /// </summary>
    /// <param name="pushCoefficient">Multiplier to max push force for applied force. Range from 0 to 1.</param>
    public void PushPlayer(float pushCoefficient)
    {
        rb.isKinematic = false;
        float pushForce = pushCoefficient * maxPushForce;
        Vector3 directionUnit = (directionObject.position - transform.position).normalized;
        rb.AddForce(pushForce * directionUnit);
    }

    /// <summary>
    /// Set player back to initial state
    /// </summary>
    void ResetPlayer()
    {
        Debug.Log("Resetting player");
        rb.isKinematic = true;
        transform.localPosition = initialPos;
        transform.localRotation = initialRot;
    }

    /// <summary>
    /// Handles mesh and collider switching based on desired player shape
    /// </summary>
    /// <param name="isCube">True if cube, false if sphere</param>
    public void ToggleShape(bool isCube)
    {
        if (isCube)
        {
            boxCollider.enabled = true;
            sphereCollider.enabled = false;
            meshFilter.mesh = cubeMesh;
        }
        else
        {
            boxCollider.enabled = false;
            sphereCollider.enabled = true;
            meshFilter.mesh = sphereMesh;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 direction = (transform.position - transform.Find("Direction").position).normalized;
        Gizmos.DrawLine(transform.position, transform.position - direction*0.1f);
    }
}
