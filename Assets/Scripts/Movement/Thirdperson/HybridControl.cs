using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HybridControl : MonoBehaviour
{
    
    private VRMovement InputController;
    private CameraController cameraController;
    private PhysicalMovement physicalMovement;
    [SerializeField] private  bool ThirdPerson=true;
    
    [Header("Rotation Settings")]
    public bool FadingDuringRotation;
    [Range(0f,1f)] public float FadeOutDuration;
    [Range(0f,1f)] public float FadeDuration;
    [Range(0f,1f)] public float FadeInDuration;
    public bool AllowRotationDuringFirstperson;
    
    [Header("Switch View Settings")]
    [SerializeField] private  bool AllowViewSwitch;
    public bool FadingBetweenViews;
    [Range(0f, 1f)] public float SwitchFadeOutDuration;
    [Range(0f,1f)] public float SwitchFadeDuration;
    [Range(0f,1f)] public float SwitchFadeInDuration;
    [Range(0f, 1f)] public float MovementReductionDuringFirstPerson;

    private bool _fadingInProgres;

    private void Start()
    {
        InputController = GetComponent<VRMovement>();
        cameraController = GetComponent<CameraController>();
        physicalMovement = GetComponent<PhysicalMovement>();


        InputController.notifyLeftButtonPressedObserver += Fading;
        InputController.notifyRightButtonPressedObserver += Fading;

        InputController.notifySwitchButtonPressedObserver += SwitchPerspective;
        
        //cameraController = GetComponentInChildren<CameraController>();
       if(!ThirdPerson)
            InputController.AllowRotation(AllowRotationDuringFirstperson);
    }


    private void Update()
    {
        cameraController.RotateCamera(InputController.GetRotation());
    }



    public void SwitchPerspective()
    {
        if (AllowViewSwitch)
        {
            ThirdPerson =!ThirdPerson;
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
