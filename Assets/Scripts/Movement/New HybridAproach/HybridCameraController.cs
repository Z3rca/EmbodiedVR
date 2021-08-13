using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HybridCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private GameObject CameraArm;
    private Vector3 targetPosition;
    private bool _thirdPersonWasActivated;
    private bool _fadingInProgres;
    [SerializeField] private float _cameraDistance;
    
    [SerializeField] private StencilWallDection stecilWallDectector;
    
    public delegate void OnFadingCompleted();
    public event OnFadingCompleted OnNotifyFadedCompletedObervers;

    private void Start()
    {
        targetPosition = this.transform.position;
    }

    public void RotateCamera(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void SwitchPerspective(bool toThirdPerson)
    {
        if (toThirdPerson)
        {
            CameraArm.transform.localPosition = Vector3.forward * -_cameraDistance;
            
            if (stecilWallDectector != null)
            {
                stecilWallDectector.IgnoreMask(false);
            }
           
        }
        else
        {
            CameraArm.transform.localPosition=Vector3.zero;
            
            if (stecilWallDectector != null)
            {
                stecilWallDectector.IgnoreMask(true);
            }
        }
        
    }
    
    public void Fading(float FadeOutDuration,float FadeInDuration, float FadeDuration)
    {
        
        Debug.Log("fading");
        _fadingInProgres = true;
        StartCoroutine(FadeOutFadeIn(FadeOutDuration,FadeInDuration,FadeDuration));
        
    }
    private IEnumerator FadeOutFadeIn(float FadeOut=0.25f, float FadeIn=0.25f, float FadeTime =.1f)
    {
        SteamVR_Fade.Start(Color.black,FadeOut);
        yield return new WaitForSeconds(FadeOut);
        yield return new WaitForSeconds(FadeTime);
        SteamVR_Fade.Start(Color.clear,FadeIn);
        _fadingInProgres = false;
        
        OnNotifyFadedCompletedObervers.Invoke();
    }
    
    
    
    

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
    
    
}
