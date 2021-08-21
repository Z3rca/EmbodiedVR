using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataRecorder : MonoBehaviour
{
    
    
    void Start()
    {
        ExperimentManager.Instance.startedExperiment += OnStartedExperiment;
        ExperimentManager.Instance.OnPakourBegin += OnPakourBegin;
    }


    void OnStartedExperiment(object sender, StartExperimentArgs startExperimentArgs)
    {
        
    }

    void OnPakourBegin(object sender, ParkourBeginArgs parkourBeginArgs)
    {
        parkourBeginArgs.Condition.ToString()
    }
    
    void OnPakourFinished(object sender, ParkourBeginArgs parkourBeginArgs)
    {
        parkourBeginArgs.Condition.ToString()
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
