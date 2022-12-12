using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralBodyTrigger : MonoBehaviour
{
    public UnityEvent TriggeredEvent;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            TriggeredEvent.Invoke();
        }
    }
}
