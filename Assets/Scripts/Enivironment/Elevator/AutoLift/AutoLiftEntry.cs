using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoLiftEntry : MonoBehaviour
{
    public UnityEvent LiftEvent;
    private Elevator elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            LiftEvent.Invoke();
        }
    }
    
    
}
