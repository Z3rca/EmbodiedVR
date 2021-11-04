using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CloseDoorTrigger : MonoBehaviour
{
    private ExperimentManager experimentManager;
    
    private HybridCharacterController playerBody;

    public UnityEvent OnExitTrigger;

    private bool _wasTriggered;

    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            experimentManager = ExperimentManager.Instance;
            experimentManager.startedExperiment += OnExperimentStarted;
        }
        
        
        
    }


    private void OnExperimentStarted(object sender, StartExperimentArgs eventArgs)
    {
        playerBody = eventArgs.CharacterController;
        if (playerBody == null)
        {
            Debug.Log("Player body couln't be assigned");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HybridCharacterController>()==playerBody)
        {
            if (_wasTriggered)
                return;
            
            _wasTriggered = true;
            OnExitTrigger.Invoke();   
        }
    }
}
