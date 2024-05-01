using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionPlayerController : MonoBehaviour
{
    [SerializeField] float maxPushForce;
    Vector3 initialPos;
    Quaternion initialRot;
    Transform directionObject;
    Rigidbody rb;
    
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        directionObject = transform.Find("Direction");
        yield return new WaitForEndOfFrame();
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    public void PushPlayer(float pushCoefficient)
    {
        float pushForce = pushCoefficient * maxPushForce;
        Vector3 directionUnit = (directionObject.position - transform.position).normalized;
        Debug.Log("Player pushed to direction with force: " + pushForce);
        rb.AddForce(pushForce * directionUnit);
    }

    public void ResetPlayer()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
    }
}
