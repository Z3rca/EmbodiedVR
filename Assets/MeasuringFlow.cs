using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringFlow : MonoBehaviour
{
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void measureAudio()
    {
        audioMeasuringTool.SetActive(true);
    }
    
    void measureMotionSickness()
    {
        motionSicknessMeasuringTool.SetActive(true);
    }

    void measurePosturalStability()
    {
        posturalStabilityMeasuringTool.SetActive(true);
    }
}
