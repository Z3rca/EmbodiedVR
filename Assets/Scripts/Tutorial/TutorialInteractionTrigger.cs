﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialInteractionTrigger : MonoBehaviour
{
    public bool goalBox = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            Debug.Log("reached area");
            if (goalBox)
            {
                TutorialManager.Instance.ReachedSecondInteractionArea();
            }
            else
            {
                TutorialManager.Instance.ReachedInteractionArea();
            }
            
        }
        
    }
}
