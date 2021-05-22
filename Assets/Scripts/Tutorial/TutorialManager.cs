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
        yield return new WaitUntil(() => perspectiveSwitchWasDone);
        EnableInteractionArea();

    }

    public void ReachedInteractionArea()
    {
        audioController.MovementAudioClip();
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
