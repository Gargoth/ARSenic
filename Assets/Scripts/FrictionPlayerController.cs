using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionPlayerController : MonoBehaviour
{
    [SerializeField] Mesh cubeMesh;
    [SerializeField] Mesh sphereMesh;
    [SerializeField] float maxPushForce;
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
        SetInitialState();

		// testing
		// yield return new WaitForSeconds(1f);
		// PushPlayer(1f);
    }

    public void SetInitialState()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    public void PushPlayer(float pushCoefficient)
    {
        rb.isKinematic = false;
        float pushForce = pushCoefficient * maxPushForce;
        Vector3 directionUnit = (directionObject.position - transform.position).normalized;
        Debug.Log("Player pushed to direction with force: " + pushForce);
        rb.AddForce(pushForce * directionUnit);
    }

    public void ResetPlayer()
    {
        rb.isKinematic = true;
        transform.position = FrictionStageManager.Instance.GetSpawnPos();
        transform.rotation = initialRot;
    }

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
