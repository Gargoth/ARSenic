using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedTrigger : MonoBehaviour
{
    [SerializeField] float targetDuration = 2f;
    [NonSerialized] public UnityEvent triggerEvent;
    Coroutine triggerCoroutine;
    
    void Start() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggerCoroutine == null && !FrictionModuleManager.Instance.CanPush)
        {
            Debug.Log("Player trigger enter");
            triggerCoroutine = StartCoroutine(triggerIEnumerator(targetDuration));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger exit");
            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
                triggerCoroutine = null;
            }
        }
    }

    IEnumerator triggerIEnumerator(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        triggerEvent.Invoke();
        Debug.Log("Ended Finishing Level Coroutine");
    }
}
