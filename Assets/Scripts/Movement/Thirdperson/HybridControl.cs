using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HybridControl : MonoBehaviour
{
    
    private VRMovement InputController;
    private CameraController cameraController;
    private PhysicalMovement physicalMovement;
    private ControllerRepresentations controllerRepresentations;
    [SerializeField] private RemoteVR remoteVR;
    [SerializeField] private VRIK puppetIK;
    [SerializeField] private bool ThirdPerson;
    [SerializeField] private bool ShowControllerHelp;
    
    [Header("Rotation Settings")]
    public bool FadingDuringRotation;
    [Range(0f,1f)] public float FadeOutDuration;
    [Range(0f,1f)] public float FadeDuration;
    [Range(0f,1f)] public float FadeInDuration;
    public bool AllowRotationDuringFirstperson;
    
    [Header("Switch View Settings")]
    public bool _allowViewSwitch;
    public bool FadingBetweenViews;
    [Range(0f, 1f)] public float SwitchFadeOutDuration;
    [Range(0f,1f)] public float SwitchFadeDuration;
    [Range(0f,1f)] public float SwitchFadeInDuration;
    [Range(0f, 1f)] public float MovementReductionDuringFirstPerson;
    private bool _fadingInProgres;
    
    
    private bool _temporaryIkLocomotion;
    
    [Header("Position Readjustment")][Range(0f, 1f)] public float ReadjustmentThreshold = 0.4f;
    private float currentPuppetToPlayerOffset;
    
    
    public EventHandler<SwitchPerspectiveEventArgs> NotifyPerspectiveSwitch;
    

    private void Start()
    {
        InputController = GetComponent<VRMovement>();
        cameraController = GetComponent<CameraController>();
        physicalMovement = GetComponent<PhysicalMovement>();
        controllerRepresentations = GetComponent<ControllerRepresentations>();
        if (InputController.Body.GetComponent<VRIK>() != null)
        {
            puppetIK = InputController.Body.GetComponent<VRIK>();
        }
        


        InputController.notifyLeftButtonPressedObserver += Fading;
        InputController.notifyRightButtonPressedObserver += Fading;

        InputController.notifySwitchButtonPressedObserver += SwitchPerspective;
        
        //cameraController = GetComponentInChildren<CameraController>();
       if(!ThirdPerson)
            InputController.AllowRotation(AllowRotationDuringFirstperson);

       if (ShowControllerHelp)
       {
           ShowControllers(true);
       }
       else
       {
           ShowControllers(true);
       }
    }


    private void Update()
    {
        cameraController.RotateCamera(InputController.GetRotation());

        if (_temporaryIkLocomotion)
        {
            this.transform.position = remoteVR.RemoteFootPositon.transform.position;
        }

        currentPuppetToPlayerOffset = Vector3.Distance(remoteVR.RemoteFootPositon.transform.position,
            physicalMovement.feet.transform.position);
        
        Debug.Log(currentPuppetToPlayerOffset);
        
        if(currentPuppetToPlayerOffset>ReadjustmentThreshold)
        {
            Debug.Log( remoteVR.RemoteFootPositon.transform.position + " " +physicalMovement.feet.transform.position+ " " + currentPuppetToPlayerOffset);
            AdjustPuppetPosition();
        }
        
        
    }



    private void AdjustPuppetPosition()
    {
       StartCoroutine(puppetAdjusting());
    }

    private IEnumerator puppetAdjusting()
    {
        AdjustPuppetPosition(true);
        while (currentPuppetToPlayerOffset > ReadjustmentThreshold)
        {
            yield return new WaitForEndOfFrame();
        }
        
       // AdjustPuppetPosition(false);
        
        puppetIK.solver.Reset();

        //physicalMovement.feet.position = remoteVR.RemoteFootPositon.transform.position;

    }


    public void WeightIKLocomotion(float value)
    {
        puppetIK.solver.locomotion.weight = value;
    }

    private void AdjustPuppetPosition(bool state)
    {
        _temporaryIkLocomotion = state;
        
        InputController.SetAdjustmentStatus(state);
        physicalMovement.SetAdjustmentStatus(state);
        
        if (state)
        {
            puppetIK.solver.locomotion.weight = 1;
        }
        else
        {
            puppetIK.solver.locomotion.weight = 0;
        }
        
    }
    
    
    

    public void SwitchPerspective()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }
        
        if (_allowViewSwitch)
        {
            ThirdPerson =!ThirdPerson;
            var eventArgs = new SwitchPerspectiveEventArgs
            {
                switchToThirdPerson = this.ThirdPerson
            };
            NotifyPerspectiveSwitch?.Invoke(this,eventArgs);    
            
            if(FadingBetweenViews)
                StartCoroutine(FadeOutFadeIn(SwitchFadeOutDuration,SwitchFadeInDuration,SwitchFadeDuration));
       
            cameraController.SetThirdPerson(ThirdPerson);
            cameraController.SwitchPerspective();
            if (!ThirdPerson)
            {
                physicalMovement.SetSpeedFactor(MovementReductionDuringFirstPerson);
                InputController.AllowRotation(AllowRotationDuringFirstperson);
            }
            else
            {
                physicalMovement.SetSpeedFactor(1);
                InputController.AllowRotation(true);
            }
        }
    }

    public void Fading()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }
        
        if(FadingDuringRotation)
            StartCoroutine(FadeOutFadeIn(FadeOutDuration,FadeInDuration,FadeDuration));
    }
    
    public void Fading(float FadeOutDuration,float FadeInDuration, float FadeDuration)
    {
        if (FadingDuringRotation)
        {
            Debug.Log("fading");
            _fadingInProgres = true;
            StartCoroutine(FadeOutFadeIn(FadeOutDuration,FadeInDuration,FadeDuration));
        }
    }


    public bool GetThirdPerson()
    {
        return ThirdPerson;
    }


    public void AllowMovement(bool state)
    {
        physicalMovement.MovementIsAllowed = state;
        
    }

    public void ShowControllers(bool state)
    {
        controllerRepresentations.ShowController(state);
    }

    public void HighLightControlSwitchButton(bool state)
    {
        controllerRepresentations.HighLightPerspectiveChangeButton(state);
    }


    private IEnumerator FadeOutFadeIn(float FadeOut=0.25f, float FadeIn=0.25f, float FadeTime =.1f)
    {
            SteamVR_Fade.Start(Color.black,FadeOut);
            yield return new WaitForSeconds(FadeOut);
            yield return new WaitForSeconds(FadeTime);
            SteamVR_Fade.Start(Color.clear,FadeIn);
            _fadingInProgres = false;
    }
    
    public bool FadingInProgress
    {
        get => _fadingInProgres;
    }
    
    
}


public class SwitchPerspectiveEventArgs : EventArgs
{
    public bool switchToThirdPerson;
}

