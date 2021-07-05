using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CloseDoorTrigger : MonoBehaviour
{
    private ExperimentManager experimentManager;
    
    private PhysicalMovement playerBody;

    public UnityEvent OnExitTrigger;


    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            experimentManager = ExperimentManager.Instance;
            experimentManager.startExperiment += OnExperimentStart;
        }
        
        
        
    }


    private void OnExperimentStart(object sender, EventArgs eventArgs)
    {
        playerBody = experimentManager.SelectedAvatar.GetComponentInChildren<PhysicalMovement>();
        if (playerBody == null)
        {
            Debug.Log("Player body couln't be assigned");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerBody.gameObject)
        {
            OnExitTrigger.Invoke();   
        }
    }
}
