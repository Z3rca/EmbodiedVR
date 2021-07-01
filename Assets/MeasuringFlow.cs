using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeasuringFlow : MonoBehaviour
{
    public UnityEvent whenMeasuringComplete;
    
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;
    
    private bool recordingStarted = false;
    private bool audioMeasured = false;

    private bool sicknessMeasured = false;
    
    private bool stabilityMeasured = true;
    
    // Start is called before the first frame update
    void Start()
    {
        startDataGathering();
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
        
        yield return new WaitForSeconds(1);
        
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(true);
        
        while(!sicknessMeasured)
        {
            yield return null;
        }
        
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(true);
        
        while(!stabilityMeasured)
        {
            stabilityFlow();
            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        
        posturalStabilityMeasuringTool.SetActive(false);
        whenMeasuringComplete.Invoke();
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

    public void sicknessButton()
    {
        sicknessMeasured = true;
    }
    
    public void stabilityFlow()
    {
        
    }
    
    
}
