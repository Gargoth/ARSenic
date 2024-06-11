using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Used to trigger an event after a certain amount of time if the player is still inside a collider
/// </summary>
public class DelayedTrigger : MonoBehaviour
{
    [Tooltip("Amount of time to wait before triggering the event")] [SerializeField] float targetDuration = 2f;
    [NonSerialized] public UnityEvent TriggerEvent;
    Coroutine triggerCoroutine;

    void Awake()
    {
        TriggerEvent = new UnityEvent();
    }

    void Start()
    {
        // Make sure to cancel the coroutine if the stage is reset
        FrictionModuleManager.Instance.ResetActions.AddListener(CancelTriggerCoroutine);
    }

    void OnTriggerEnter(Collider other)
    {
        // Start countdown once player enters trigger collider
        if (other.gameObject.CompareTag("Player") && triggerCoroutine == null &&
            !FrictionModuleManager.Instance.CanPush)
        {
            Debug.Log("Player trigger enter" + name);
            triggerCoroutine = StartCoroutine(TriggerIEnumerator(targetDuration));
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop countdown once player exits trigger collider
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger exit" + name);
            CancelTriggerCoroutine();
        }
    }

    /// <summary>
    /// Stops the countdown to run the event
    /// Called when the stage is reset or when the player exits the collider
    /// </summary>
    public void CancelTriggerCoroutine()
    {
        if (triggerCoroutine != null)
        {
            StopCoroutine(triggerCoroutine);
            triggerCoroutine = null;
            Debug.Log("Stopping trigger coroutine");
        }
    }

    /// <summary>
    ///  Coroutine responsible for triggering the event
    /// </summary>
    /// <param name="duration">Amount of time to wait before triggering the event</param>
    IEnumerator TriggerIEnumerator(float duration)
    {
        Debug.Log("Starting Finishing Level Coroutine with duration " + duration);
        yield return new WaitForSeconds(duration);
        TriggerEvent.Invoke();
        Debug.Log("Ended Finishing Level Coroutine");
    }
}