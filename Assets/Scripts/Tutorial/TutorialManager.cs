using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TutorialManager : MonoBehaviour
{
    
    public static TutorialManager Instance { get; private set; }
    private TutorialAudioDialogController audioController;
    public HybridController HybridController;

    public GameObject InteractionAreaShine;
    public GameObject BoxAreaShine;
    public GameObject ExitAreaShine;
    public GameObject Ball;

    private bool finalizedTutorial = false;

    private bool _isFirstPersonCondition;
    
    private bool FamilarizationIsRunning;
    private bool MovmentIntroIsRunning;

    private bool _thirdPersonIsActive;

    private bool ThirdPersonIsActive;

    private bool _CubeIsInHand;

    private bool interactionAreaIsActive;

    private bool InteractionState;

    private bool _reachedSecondArea;

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


    public bool GetIsTutorialFinished()
    {
        return finalizedTutorial;
    }

    public bool GetInteractionState()
    {
        return InteractionState;
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
        yield return new WaitUntil(() => !audioController.GetActive());
        
        Debug.Log("finished Introduction");
        
        if (!_isFirstPersonCondition)
        {
            audioController.AudioClip2();
            yield return new WaitUntil(() => !audioController.GetActive());
            yield return new WaitForSeconds(1);
            audioController.AudioClip3();
            yield return new WaitUntil(() => !audioController.GetActive());
            yield return new WaitForSeconds(1);
            audioController.AudioClip4();
            HybridController.HighLightControlSwitchButton(true);
            yield return new WaitUntil(() => !audioController.GetActive());
            
            HybridController.AllowViewSwitch(true);
            yield return new WaitUntil(() => !audioController.GetActive());
            HybridController.ShowControllers(true);
            
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
        HybridController.AllowRotation(true);
        yield return new WaitUntil(() => !audioController.GetActive());
        
        //TODO: here only rotation should be enabled
        Debug.Log("enabled Movement");
        HybridController.AllowInput(true);
        
        HybridController.HighLightMovementButton(true);
        audioController.AudioClip6();
        yield return new WaitUntil(() => !audioController.GetActive());
        
        //TODO: here rest of movement should be enabled
       
        
        yield return new WaitForSeconds(4);
        
        EnableInteractionArea();
        audioController.AudioClip7();
        yield return new WaitUntil(() => !audioController.GetActive());
        interactionAreaIsActive = true;




    }

    public void ReachedInteractionArea()
    {
        if (InteractionState)
            return;
        
        StartCoroutine(ReachedInteractionAreaRoutine());
    }

    public IEnumerator ReachedInteractionAreaRoutine()
    {
        yield return new WaitUntil(()=> interactionAreaIsActive);
        
        HybridController.HighLightMovementButton(false);
        HybridController.HighLightRotationButton(false);

        GameObject cube;
        Ball.SetActive(true);
        var cubeHovering = Ball.GetComponentInChildren<IgnoreHovering>();
        


        if (!_isFirstPersonCondition)
        {
            HybridController.ShowControllers(true);
            HybridController.HighLightControlSwitchButton(true);
            
            audioController.AudioClip8();
            yield return new WaitUntil(() => !audioController.GetActive());
            yield return new WaitUntil(() => !_thirdPersonIsActive);
            
            HybridController.HighLightControlSwitchButton(false);
        }
        
        audioController.AudioClip9();
        yield return new WaitUntil(() => !audioController.GetActive());
        
        Destroy(cubeHovering);
        InteractionState = true;
        HybridController.HighLightGraspButtons(true);

        
        
    }

    public void ReachedSecondInteractionArea()
    {
        /*if (_reachedSecondArea)
            return;*/
        
        StartCoroutine(ReachedSecondInteractionAreaRoutine());
    }

    public IEnumerator ReachedSecondInteractionAreaRoutine()
    {
        audioController.AudioClip12();
        _reachedSecondArea = true;
        yield return new WaitUntil(() => !audioController.GetActive());
        _reachedSecondArea = false;
    }

    public void ThrownBallInBox()
    {
        FinishTutorial();
    }
    

    public void BallWasTaken()
    {
        
        if (_CubeIsInHand)
            return;
        
        InteractionState = false;
        _CubeIsInHand = true;
        
        if (!_isFirstPersonCondition)
        {
            HybridController.HighLightMovementButton(true);
            HybridController.HighLightRotationButton(true);
            HybridController.ShowControllers(true);
            HybridController.HighLightControlSwitchButton(true);

            audioController.AudioClip10(); 
        }
        
        
        audioController.AudioClip11(); 
        EnableBoxArea();
    }

    public void BallWasLost()
    {
        _CubeIsInHand = false;
        InteractionState = false;
    }

    public bool CubeIsInHand()
    {
        return _CubeIsInHand;
    }

    public bool ReachedSecondArea()
    {
        return _reachedSecondArea;
    }

    public void ReachedSecondArea(bool state)
    {
        _reachedSecondArea = state;
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

    private void FinishTutorial()
    {
        EnableExitArea();
        audioController.AudioClip13();

        Door.SetActive(false);
        Door2.SetActive(false);
        finalizedTutorial = true;
        ExperimentManager.Instance.SetisInTutorial(false);
        
        HybridController.StopHighlighting();
        

    }
    public void StopAllDialogue()
    {
        Debug.Log("stopp everything");
        audioController.ForceStopAllAudio();
        ExitAreaShine.SetActive(false);
        HybridController.StopHighlighting();
        if (HybridController.IsEmbodiedCondition())
        {
            HybridController.ShowControllers(false);
        }
    }
}
