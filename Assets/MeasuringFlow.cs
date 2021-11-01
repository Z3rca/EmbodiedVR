using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MeasuringFlow : MonoBehaviour
{
    public UnityEvent whenSicknessButtonPressed;
    
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;

    public TMP_Text audioRunningText;
    
    public AudioSource audioInstruction21;
    public AudioSource audioInstruction22;
    public AudioSource audioInstruction23;
    public AudioSource audioInstruction24;

    public float recoveryTimeOfButton = 2f;
    
    private bool recordingStarted = false;
    private bool audioMeasured = false;

    public bool sicknessMeasured { get; set; } = false;
    
    private bool stabilityMeasured = false;

    private bool lastStage = false;

    private bool audioRecordingPassed;

    private bool _pressed;
    [SerializeField] private GameObject AcceptButton;
    [SerializeField] private Material ActiveOkayButtonMaterial;
    [SerializeField] private Material DeactivatedOkayButtonMaterial;

    private float _posturalStabilityMeasuringDuration=5f;

    public event Action MotionsicknessMeasurementStart;
    public event Action AudioRecordingStarted;
    public event Action AudioRecordingEnded;
    public event Action PosturalStabilityTestStarted;
    public event Action PosturalStabitityTestEnded;

    public event Action DataGatheringEnded;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
    }

    public void SetPosturalMeasuringDuration(float duration)
    {
        _posturalStabilityMeasuringDuration = duration;
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

        
        while(!audioMeasured||ExperimentManager.Instance.MicrophoneIsRecording())
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
        
        while(!stabilityMeasured)
        {
            yield return null;
        }
        PosturalStabitityTestEnded.Invoke();
        posturalStabilityMeasuringTool.SetActive(false);
        

        yield return new WaitForEndOfFrame();

        //TODO lastStage needs to be defined sensibly
        if (ExperimentManager.Instance!=null)
        {
            lastStage = ExperimentManager.Instance.LastTrail();
        }
        
        if (!lastStage)
        {
            audioInstruction22.Play();
            while (audioInstruction22.isPlaying)
                yield return null;
            
            audioInstruction23.Play();
        }
        else
        {
            audioInstruction24.Play();
            
        }

        yield return new WaitForEndOfFrame();

        if (ExperimentManager.Instance!=null)
        {
            Debug.Log("Data Gathering ends , measuring panel");
           
            DataGatheringEnded.Invoke();
        }
    }


    private void AudioButton()
    {
        if(audioMeasured)
            return;
        
        if (recordingStarted)
        {
            AudioRecordingEnded.Invoke();
            audioMeasured = true;
        }
        else
        {
            AudioRecordingStarted.Invoke();
            recordingStarted = true;
            audioRunningText.text = "Audio recording is in progress.";
        }
    }

    private void SicknessButton()
    {
        whenSicknessButtonPressed.Invoke();
    }

    private void StabilityFlow()
    {
        PosturalStabilityTestStarted.Invoke();

        StartCoroutine(WaitForPosturalStabiltyTestDuration(_posturalStabilityMeasuringDuration));
    }

    public void OkayButton()
    {
        if (_pressed)
            return;
        
        _pressed = true;
        AcceptButton.GetComponent<Renderer>().material = DeactivatedOkayButtonMaterial;
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

    public void RecoverButton()
    {
        StartCoroutine(delayedRecovery());
    }

    private IEnumerator delayedRecovery()
    {
        yield return new WaitForSeconds(recoveryTimeOfButton);
        _pressed = false;
        AcceptButton.GetComponent<Renderer>().material = ActiveOkayButtonMaterial;
    }

    private IEnumerator WaitForPosturalStabiltyTestDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        stabilityMeasured = true;
    }
    
}
