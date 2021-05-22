using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialInteractionTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            TutorialManager.Instance.ReachedInteractionArea();
        }
        
    }
}
