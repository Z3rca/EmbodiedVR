using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionsicknessRatingLogic : MonoBehaviour
{
    void Start()
    {
        ExperimentManager.Instance.startedExperiment += OnStartedExperiment;
    }


    void OnStartedExperiment(object sender, StartExperimentArgs startExperimentArgs)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
