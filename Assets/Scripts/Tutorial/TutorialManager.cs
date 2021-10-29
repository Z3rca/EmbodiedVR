using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public static TutorialManager Instance { get; private set; }
    private TutorialAudioDialogController audioController;
    public HybridController HybridController;

    public GameObject InteractionAreaShine;
    public GameObject BoxAreaShine;
    public GameObject ExitAreaShine;
    public GameObject Ball;

    public bool success = false;

    private bool _isFirstPersonCondition;
    
    private bool FamilarizationIsRunning;
    private bool MovmentIntroIsRunning;

    private bool _thirdPersonIsActive;

    private bool ThirdPersonIsActive;
    
    

    public GameObject Door;
    public GameObject Door2;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void StartTutorial()
    {
        
        if (ExperimentManager.Instance != null)
        {
            HybridController = ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>();
        }


        if (ExperimentManager.Instance.GetCondition() == ExperimentManager.Condition.FirstPerson ||
            ExperimentManager.Instance.GetCondition() == ExperimentManager.Condition.Bodiless)
        {
            _isFirstPersonCondition = true;
        }
        
        audioController = GetComponent<TutorialAudioDialogController>();
        
        HybridController.OnNotifyPerspectiveSwitchObservers += PerspectiveSwitchWasPerformend;
        HybridController.ShowControllers(true);
        StartFamilarization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartFamilarization()
    {
        
        StartCoroutine(FamilarizationRoutine());
    }

    

    private IEnumerator FamilarizationRoutine()
    {
        Debug.Log("start tutorial");
        yield return new WaitForEndOfFrame();
        HybridController.AllowViewSwitch(false);
        HybridController.AllowInput(false);
        HybridController.Fading(0f,2f,2f);
        yield return new WaitUntil(() =>!HybridController.IsFadingInProgress());   
        audioController.AudioClip1();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        
        Debug.Log("finished Introduction");
        
        if (!_isFirstPersonCondition)
        {
            audioController.AudioClip2();
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
            yield return new WaitForSeconds(1);
            audioController.AudioClip3();
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
            yield return new WaitForSeconds(1);
            audioController.AudioClip4();
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
            
            HybridController.AllowViewSwitch(true);
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
            HybridController.ShowControllers(true);
            HybridController.HighLightControlSwitchButton(true);
            yield return new WaitUntil(() => _thirdPersonIsActive);
            HybridController.HighLightControlSwitchButton(false);
        }
        else
        {
            HybridController.ShowControllers(true);
        }
        
        yield return new WaitForSeconds(1);
        
        HybridController.HighLightRotationButton(true);
        audioController.AudioClip5();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        
        //TODO: here only rotation should be enabled
        HybridController.AllowInput(true);
        
        
        
        
        HybridController.HighLightMovementButton(true);
        audioController.AudioClip6();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        
        //TODO: here rest of movement should be enabled
        Debug.Log("enabled Movement");
        
        yield return new WaitForSeconds(6);
        
        //Dont show Controllers
        EnableInteractionArea();
        audioController.AudioClip7();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        
        
        

    }

    public void ReachedInteractionArea()
    {
        StartCoroutine(ReachedInteractionAreaRoutine());
    }

    public IEnumerator ReachedInteractionAreaRoutine()
    {
        HybridController.HighLightMovementButton(false);
        HybridController.HighLightRotationButton(false);
        
        Ball.SetActive(true);
        
        if (!_isFirstPersonCondition)
        {
            HybridController.ShowControllers(true);
            HybridController.HighLightControlSwitchButton(true);
            audioController.AudioClip8();
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
            yield return new WaitUntil(() => !_thirdPersonIsActive);
            HybridController.HighLightControlSwitchButton(false);
            yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        }
        
        audioController.AudioClip9();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        HybridController.HighLightGraspButtons(true);

        
        
    }

    public void ReachedSecondInteractionArea()
    {
        StartCoroutine(ReachedInteractionAreaRoutine());
    }

    public IEnumerator ReachedSecondInteractionAreaRoutine()
    {
        audioController.AudioClip12();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
    }

    public void ThrownBallInBox()
    {
        //audioController.FinishedTask();  //Well done.
        EnableExitArea();
        audioController.AudioClip13();
        // now you can go go through the door and finish the tutorial section.  
        Door.SetActive(false);
        Door2.SetActive(false);

        success = true;
        ExperimentManager.Instance.SetisInTutorial(false);


        StartCoroutine(StopHighlighting(0.3f));


    }

    private IEnumerator StopHighlighting(float time)
    {
        HybridController.HighLightMovementButton(false);
        HybridController.HighLightRotationButton(false);
        HybridController.HighLightGraspButtons(false);
        HybridController.StopHighlighting();
        yield return new WaitForSeconds(time);
        HybridController.ShowControllers(false);
    }
  

    public void BallWasTaken()
    {
       
        if (!_isFirstPersonCondition)
        {
            HybridController.HighLightMovementButton(true);
            HybridController.HighLightRotationButton(true);
            HybridController.ShowControllers(true);
            HybridController.HighLightControlSwitchButton(true);
            Ball.GetComponent<Rigidbody>().useGravity = true;
            
            audioController.AudioClip10(); 
        }
        
        
        audioController.AudioClip11(); 
        EnableBoxArea();
    }
    
    private void PerspectiveSwitchWasPerformend(bool state)
    {
        _thirdPersonIsActive = state;
    }

    public void EnableInteractionArea()
    {
        InteractionAreaShine.SetActive(true);
        BoxAreaShine.SetActive(false);
        ExitAreaShine.SetActive(false);
    }
    
    
    public void EnableBoxArea()
    {
        InteractionAreaShine.SetActive(false);
        BoxAreaShine.SetActive(true);
        ExitAreaShine.SetActive(false);
    }
    
    public void EnableExitArea()
    {
        InteractionAreaShine.SetActive(false);
        BoxAreaShine.SetActive(false);
        ExitAreaShine.SetActive(true);
    }
}
