using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MeasuringFlow : MonoBehaviour
{
    public UnityEvent whenSicknessButtonPressed;

    public GameObject welcomeScreen;
    public GameObject audioMeasuringTool;
    public GameObject motionSicknessMeasuringTool;
    public GameObject posturalStabilityMeasuringTool;
    
    
    public AudioSource audioInstruction22;
    public AudioSource audioInstruction23;
    public AudioSource audioInstruction24;

    public float recoveryTimeOfButton = 2f;
    
    private bool audioRecordingStarted = false;
    private bool audioMeasured = false;
    private bool welcomeCompleted;

    private bool sicknessMeasured;
    
    private bool stabilityMeasured = false;

    private bool lastStage = false;

    private bool audioRecordingPassed;

    private bool visualizeCircle;

    private bool _pressed;
    private bool _posturalTestInProgress;
    [SerializeField] private GameObject AcceptButton;
    [SerializeField] private Material ActiveOkayButtonMaterial;
    [SerializeField] private Material DeactivatedOkayButtonMaterial;

    [SerializeField] private float posturalStabilityMeasuringDuration=5f;
    [SerializeField] private float delayPosturalStabilityStart=2f;
    
    
    //Image of Audio recording
    [SerializeField] private float audioPulseSpeed=5f;
    [SerializeField] private Image audioRecordingCircle;
    [SerializeField] private Image audioMicrophoneSymbol;
    [SerializeField] private GameObject audioCircleLogo;
    
    //Image of Postural Stability Measurement
    
    [SerializeField] private float posturePulseSpeed=5f;
    [SerializeField] private Image postureCircle;
    [SerializeField] private Image postureSymbol;
    [SerializeField] private GameObject postureCircleLogo;
    
    public event Action MotionsicknessMeasurementStart;
    public event Action AudioRecordingStarted;
    public event Action AudioRecordingEnded;
    public event Action PosturalStabilityTestStarted;
    public event Action PosturalStabitityTestEnded;

    public event Action DataGatheringEnded;

    public event Action DataGatheringStarted;
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
    }

    public void FinishMotionsicknessRating()
    {
        sicknessMeasured =true;
    }
    public void SetPosturalMeasuringDuration(float duration)
    {
        posturalStabilityMeasuringDuration = duration;
    }

    public void StartDataGathering()
    {
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        //welcome screen
        welcomeScreen.SetActive(true);
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);
        
        while (!welcomeCompleted)
        {
            yield return null;
        }
        
        //audio measuring
        welcomeScreen.SetActive(false);
        audioMeasuringTool.SetActive(true);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);
        float factor = -1f;
        while (!audioRecordingStarted)
        {
            yield return null;
        }
        audioCircleLogo.SetActive(true);
        
        while(!audioMeasured||ExperimentManager.Instance.MicrophoneIsRecording())
        {
            if (audioMicrophoneSymbol.color.a <= 0.1f)
            {
                factor = 1;
            }

            if (audioMicrophoneSymbol.color.a >= 1)
            {
                factor =-1;
            }
            
            audioMicrophoneSymbol.color += factor*Color.black*0.1f*audioPulseSpeed*Time.deltaTime;
            audioRecordingCircle.fillAmount = ExperimentManager.Instance.GetRemainingTimePercentageOfAudioRecord();
            yield return null;
        }
        audioCircleLogo.SetActive(false);
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
        
        if (audioRecordingStarted)
        {
            AudioRecordingEnded.Invoke();
            audioMeasured = true;
        }
        else
        {
            AudioRecordingStarted.Invoke();
            audioRecordingStarted = true;
        }
    }

    private void SicknessButton()
    {
        whenSicknessButtonPressed.Invoke();
    }

    private void StabilityFlow()
    {
        if (_posturalTestInProgress)
            return;
        _posturalTestInProgress = true;
        StartCoroutine(WaitForPosturalStabiltyTestDuration(posturalStabilityMeasuringDuration, delayPosturalStabilityStart));
    }

    public void OkayButton()
    {
        if (_pressed)
            return;
        
        _pressed = true;
        AcceptButton.GetComponent<Renderer>().material = DeactivatedOkayButtonMaterial;
        if (welcomeScreen.activeSelf)
        {
            WelcomeButton();
        }
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

    private void WelcomeButton()
    {
        welcomeCompleted = true;
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

    private IEnumerator WaitForPosturalStabiltyTestDuration(float duration, float delay)
    {
        
        postureCircleLogo.SetActive(true);
        
        yield return new WaitForSeconds(delay);
        StartCoroutine(VisualizedPostureProgress(duration));
        PosturalStabilityTestStarted.Invoke();

        yield return new WaitForSeconds(duration);
        stabilityMeasured = true;
        _posturalTestInProgress = false;
        visualizeCircle = false;
    }

    private IEnumerator VisualizedPostureProgress(float duration)
    {
        if(visualizeCircle)
            yield return null;
        
        visualizeCircle = true;
        
        postureCircle.fillAmount = 0f;
        float remainingDuration = duration;
        float factor = 0f;
        while (!stabilityMeasured)
        {
            remainingDuration -= Time.deltaTime;
            if (postureSymbol.color.a <= 0.1f)
            {
                factor = 1;
            }

            if (postureSymbol.color.a >= 1)
            {
                factor =-1;
            }
            
            postureSymbol.color += factor*Color.black*0.1f*posturePulseSpeed*Time.deltaTime;
            
            
            
           
            postureCircle.fillAmount =1 -(remainingDuration / duration) ;
            yield return new WaitForEndOfFrame();
        }
    }

    public void DeactivateBoard()
    {
        //this is called after set all texts to a certain language
        welcomeScreen.SetActive(false);
        audioMeasuringTool.SetActive(false);
        motionSicknessMeasuringTool.SetActive(false);
        posturalStabilityMeasuringTool.SetActive(false);
    }
}
