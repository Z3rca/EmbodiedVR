using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralTrigger : MonoBehaviour
{
    public UnityEvent TriggeredEvent;


    private void OnTriggerEnter(Collider other)
    {
        TriggeredEvent?.Invoke();

    }
}
