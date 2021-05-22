using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public static TutorialManager Instance { get; private set; }
    private TutorialAudioDialogController audioController;
    public HybridControl HybridControl;

    public GameObject InteractionAreaLock;
    public GameObject InteractionAreaArrow;
    public GameObject PathArrow;

    private bool FamilarizationIsRunning;
    private bool MovmentIntroIsRunning;

    private bool perspectiveSwitchWasDone;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponent<TutorialAudioDialogController>();
        StartTutorial();
        HybridControl.NotifyPerspectiveSwitch += PerspectiveSwitchWasPerformend;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartTutorial()
    {
        
        StartCoroutine(FamilarizationRoutine());
    }

    

    private IEnumerator FamilarizationRoutine()
    {
        HybridControl.AllowViewSwitch = false;
        HybridControl.AllowMovement(false);
        HybridControl.Fading(0f,2f,2f);
        yield return new WaitUntil(() =>!HybridControl.FadingInProgress);
        audioController.FamilarizationAudioClip();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        Debug.Log("finished Introduction");
        audioController.MovementAudioClip();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        HybridControl.AllowViewSwitch = true;
        //Show Controllers - View Switch Button
        yield return new WaitUntil(() => perspectiveSwitchWasDone);
        //Dont show Controllers
        EnableInteractionArea();

    }

    public void ReachedInteractionArea()
    {
        audioController.InteractionAudioClip();
        //Show Controllers - ViewSwitchButton
        audioController.MovementAudioClip();
        //pick up ball instruction on controller 
        //Pick up ball  Audio Instruction
        audioController.ThrowBallInBox(); //now throw the ball in the box to your right.
    }

    public void ReachedSecondInteractionArea()
    {
        //Show Controller, View Switch button
    }

    public void ThorwnBallInBox()
    {
        audioController.FinishedTask();  //Well done.
        audioController.ExitTutorial();// now you can go go through the door and finish the tutorial section.  
    }




    private void PerspectiveSwitchWasPerformend(object sender, EventArgs eventArgs)
    {
        perspectiveSwitchWasDone = true;
    }

    public void EnableInteractionArea()
    {
        InteractionAreaLock.SetActive(false);
        InteractionAreaArrow.SetActive(true);
        PathArrow.SetActive(true);
    }
}
