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
        
        audioController = GetComponent<TutorialAudioDialogController>();
        StartFamilarization();
        HybridController.OnNotifyPerspectiveSwitchObservers += PerspectiveSwitchWasPerformend;
        HybridController.ShowControllers(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
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
        HybridController.AllowMovement(false);
        //HybridController.Fading(0f,2f,2f);
        //yield return new WaitUntil(() =>!HybridController.FadingInProgress);      //currently disabled
        audioController.FamilarizationAudioClip();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        Debug.Log("finished Introduction");
        audioController.SwitchViewButtonAudioClip();
        HybridController.AllowViewSwitch(true);
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        HybridController.ShowControllers(true);
        HybridController.HighLightControlSwitchButton(true);
        yield return new WaitUntil(() => _thirdPersonIsActive);
        HybridController.HighLightControlSwitchButton(false);
        audioController.MovementAudioClip();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        //Dont show Controllers
        EnableInteractionArea();
        HybridController.AllowMovement(true);
        Debug.Log("enabled Movement");

    }

    public void ReachedInteractionArea()
    {
        StartCoroutine(ReachedInteractionAreaRoutine());
    }

    public IEnumerator ReachedInteractionAreaRoutine()
    {
        HybridController.ShowControllers(true);
        HybridController.HighLightControlSwitchButton(true);
        audioController.InteractionAudioClip();
        yield return new WaitUntil(() => !_thirdPersonIsActive);
        HybridController.ShowControllers(false);
        HybridController.HighLightControlSwitchButton(false);
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        audioController.PickBallAudioClip();
        Ball.SetActive(true);
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
    }

    public void ReachedSecondInteractionArea()
    {
       
        audioController.ThrowBallInBoxAudioClip();
    }

    public void ThorwnBallInBox()
    {
        //audioController.FinishedTask();  //Well done.
        EnableExitArea();
        audioController.ExitTutorialAudioClip();
        // now you can go go through the door and finish the tutorial section.  
        Door.SetActive(false);
        Door2.SetActive(false);
        HybridController.ShowControllers(false);
    }


  

    public void BallWasTaken()
    {
        HybridController.ShowControllers(true);
        HybridController.HighLightControlSwitchButton(true);
        Ball.GetComponent<Rigidbody>().useGravity = true;
        audioController.ThrowBallInBoxInstructionAudioClip(); //now throw the ball in the box to your right.
        EnableBoxArea();
    }
    
    private void PerspectiveSwitchWasPerformend(bool state)
    {
        _thirdPersonIsActive = state;
        Debug.Log(_thirdPersonIsActive);
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
