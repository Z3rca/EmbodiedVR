using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionsicknessRatingLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ExperimentManager.Instance.startExperiment += OnStartExperiment;
    }


    void OnStartExperiment(object sender, StartExperimentArgs startExperimentArgs)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
