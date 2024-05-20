using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedTrigger : MonoBehaviour
{
    [SerializeField] float targetDuration = 2f;
    [NonSerialized] public UnityEvent TriggerEvent;
    Coroutine triggerCoroutine;

    void Awake()
    {
        TriggerEvent = new UnityEvent();
    }

    void Start()
    {
        FrictionModuleManager.Instance.ResetActions.AddListener(CancelTriggerCoroutine);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggerCoroutine == null &&
            !FrictionModuleManager.Instance.CanPush)
        {
            Debug.Log("Player trigger enter" + name);
            triggerCoroutine = StartCoroutine(TriggerIEnumerator(targetDuration));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger exit" + name);
            CancelTriggerCoroutine();
        }
    }

    public void CancelTriggerCoroutine()
    {
        if (triggerCoroutine != null)
        {
            StopCoroutine(triggerCoroutine);
            triggerCoroutine = null;
            Debug.Log("Stopping trigger coroutine");
        }
    }

    IEnumerator TriggerIEnumerator(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        TriggerEvent.Invoke();
        Debug.Log("Ended Finishing Level Coroutine");
    }
}