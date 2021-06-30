using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringFlow : MonoBehaviour
{
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;
    
    private bool recordingStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void measureAudio()
    {
        audioMeasuringTool.SetActive(true);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);
    }
    
    public void measureMotionSickness()
    {
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(true);
        posturalStabilityMeasuringTool.SetActive(false);
    }

    public void measurePosturalStability()
    {
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(true);
    }

    public void audioFlow()
    {
        
        if (recordingStarted == true)
        {
            //stop recording
            measureMotionSickness();
        }
        else
        {
            recordingStarted = true;
            //start recording
        }
    }
}
