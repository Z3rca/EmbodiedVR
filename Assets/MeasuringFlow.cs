using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MeasuringFlow : MonoBehaviour
{
    public UnityEvent whenMeasuringComplete;

    public UnityEvent whenSicknessButtonPressed;
    
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;

    public TMP_Text audioRunningText;
    
    public AudioSource audioSource;
    public AudioClip dataGatheringOver;
    public AudioClip proceedToNextArea;
    public AudioClip callExperimenter;
    
    private bool recordingStarted = false;
    private bool audioMeasured = false;

    public bool sicknessMeasured { get; set; } = false;
    
    private bool stabilityMeasured = false;

    private bool lastStage = false;


    public event Action MotionsicknessMeasurementStart;
    public event Action AudioRecordingStarted;
    public event Action AudioRecordingEnded;
    public event Action PosturalStabilityTestStarted;
    public event Action PosturalStabitityTestEnded;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
    }

    public void StartDataGathering()
    {
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        audioMeasuringTool.SetActive(true);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);

        
        while(!audioMeasured)
        {
            yield return null;
        }
        
        AudioRecordingEnded.Invoke();
        
        yield return new WaitForSeconds(1);
        
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(true);
        MotionsicknessMeasurementStart.Invoke();
        
        while(!sicknessMeasured)
        {
            yield return null;
        }
        
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(true);
        PosturalStabilityTestStarted.Invoke();
        while(!stabilityMeasured)
        {
            StabilityFlow();
            yield return null;
        }
        
        
        yield return new WaitForSeconds(5);
        
        posturalStabilityMeasuringTool.SetActive(false);
        PosturalStabitityTestEnded.Invoke();

        yield return new WaitForEndOfFrame();
        whenMeasuringComplete.Invoke();

        //TODO lastStage needs to be defined sensibly
        if (ExperimentManager.Instance!=null)
        {
            lastStage = ExperimentManager.Instance.StationIndex > ExperimentManager.Instance.StationOrder.Count;
        }
        
        if (!lastStage)
        {
            audioSource.clip = dataGatheringOver;
            audioSource.Play();
            audioSource.clip = proceedToNextArea;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = callExperimenter;
            audioSource.Play();
            
        }

        yield return new WaitForEndOfFrame();

        if (ExperimentManager.Instance!=null)
        {
            ExperimentManager.Instance.DataGatheringEnds();
        }
    }


    private void AudioButton()
    {
        
        if (recordingStarted)
        {
            //TODO stop recording
            audioMeasured = true;
        }
        else
        {
            AudioRecordingStarted.Invoke();
            recordingStarted = true;
            audioRunningText.text = "Audio recording is in progress.";
            //TODO start recording
        }
    }

    private void SicknessButton()
    {
        whenSicknessButtonPressed.Invoke();
    }

    private void StabilityFlow()
    {
     
        stabilityMeasured = true;
        //TODO Start Postural stability test
    }

    public void OkayButton()
    {
        if (audioMeasuringTool.activeSelf)
        {
            AudioButton();
        } else if (motionSicknessMeasuringTool.activeSelf)
        {
            SicknessButton();
        } else if (posturalStabilityMeasuringTool.activeSelf)
        {
            StabilityFlow();
        }

    }
    
}
