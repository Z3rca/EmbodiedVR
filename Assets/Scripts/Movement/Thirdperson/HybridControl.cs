using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HybridControl : MonoBehaviour
{
    private VRMovement InputController;
    public CameraController cameraController;

    public bool FadingBetweenViews;

    [Range(0f,1f)] public float FadeDuration;
    [Range(0f,1f)] public float FadeInDuration;
    [Range(0f,1f)] public float FadeOutDuration;
    
    [Range(0f,1f)] public float SwitchFadeDuration;
    [Range(0f,1f)] public float SwitchFadeInDuration;
    [Range(0f, 1f)] public float SwitchFadeOutDuration;
    private void Start()
    {
        InputController = GetComponent<VRMovement>();


        InputController.notifyLeftButtonPressedObserver += Fading;
        InputController.notifyRightButtonPressedObserver += Fading;

        InputController.notifySwitchButtonPressedObserver += SwitchPerspective;
        //cameraController = GetComponentInChildren<CameraController>();
    }


    private void Update()
    {
        
        
        cameraController.RotateCamera(InputController.GetRotation());
    
    }



    public void SwitchPerspective()
    {
        StartCoroutine(FadeOutFadeIn(SwitchFadeOutDuration,SwitchFadeInDuration,SwitchFadeDuration));
        
        cameraController.SwitchPerspective();
        
        //SteamVR_Fade.Start(Color.black,0f);
    }

    public void Fading()
    { 
        StartCoroutine(FadeOutFadeIn(FadeOutDuration,FadeInDuration,FadeDuration));
    }
    
    
    private IEnumerator FadeOutFadeIn(float FadeOut=0.25f, float FadeIn=0.25f, float FadeTime =.1f)
    {
        SteamVR_Fade.Start(Color.black,FadeOut);
        yield return new WaitForSeconds(FadeTime);
        SteamVR_Fade.Start(Color.clear,FadeIn);
    }
}
