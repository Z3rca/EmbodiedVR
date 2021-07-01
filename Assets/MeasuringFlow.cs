using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringFlow : MonoBehaviour
{
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;
    
    private bool recordingStarted = false;
    private bool audioMeasured = false;

    private bool sicknessMeasured = false;
    
    private bool stabilityMeasured = false;
    
    // Start is called before the first frame update
    void Start()
    {
        flow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDataGathering()
    {
        StartCoroutine(flow());

    }

    IEnumerator flow()
    {
        audioMeasuringTool.SetActive(true);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);
        
        while(!audioMeasured)
        {
            yield return null;
        }
        
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(true);
        
        while(!sicknessMeasured)
        {
            sicknessFlow();
            yield return null;
        }
        
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(true);
        
        while(!stabilityMeasured)
        {
            stabilityFlow();
            yield return null;
        }
        
        posturalStabilityMeasuringTool.SetActive(false);
    }


    public void audioButton()
    {
        if (recordingStarted == true)
        {
            //TODO stop recording
            audioMeasured = true;
        }
        else
        {
            recordingStarted = true;
            //TODO start recording
        }
    }

    public void sicknessFlow()
    {
        
    }
    
    public void stabilityFlow()
    {
        
    }
    
    
}
