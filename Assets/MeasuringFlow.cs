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
    
    // Start is called before the first frame update
    private void Start()
    {
        StartDataGathering();
    }

    private void StartDataGathering()
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
            StabilityFlow();
            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        
        posturalStabilityMeasuringTool.SetActive(false);
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
