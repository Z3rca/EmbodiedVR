using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoLiftEntry : MonoBehaviour
{
    public UnityEvent LiftEvent;
    private Elevator elevator;
    
    [SerializeField]private float delayUntilStart;

    private GameObject agent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            agent = other.gameObject;
            StartWithDelay(delayUntilStart);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == agent)
        {
            agent = null;
        }
    }

    private void StartWithDelay(float delay)
    {
        StartCoroutine(WaitUntilStart(delay));
    }
    IEnumerator WaitUntilStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (agent != null)
        {
            LiftEvent.Invoke();
        }
    }
    
    
}
