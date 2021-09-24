using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReachedRatingBoardTrigger : MonoBehaviour
{
    private HybridCharacterController playerBody;
    private ExperimentManager experimentManager;

    public UnityEvent OnEnterTrigger;
        
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HybridCharacterController>()==playerBody)
        {
            OnEnterTrigger.Invoke();   
        }
    }
}
