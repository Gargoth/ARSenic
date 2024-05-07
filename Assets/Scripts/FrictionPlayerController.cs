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
        transform.position = initialPos;
        transform.rotation = initialRot;
        rb.isKinematic = true;
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
}
