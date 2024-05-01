using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionPlayerController : MonoBehaviour
{
    [SerializeField] float maxPushForce;
    Transform directionObject;
    Rigidbody rb;
    
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        directionObject = transform.Find("Direction");
        yield return null;
        // DEBUG:
        yield return new WaitForSeconds(2f);
        PushPlayer(1f);
    }

    public void PushPlayer(float pushCoefficient)
    {
        float pushForce = pushCoefficient * maxPushForce;
        Vector3 directionUnit = (directionObject.position - transform.position).normalized;
        Debug.Log(pushForce);
        Debug.Log(directionUnit);
        Debug.Log(pushForce * directionUnit);
        rb.AddForce(pushForce * directionUnit);
    }
}
