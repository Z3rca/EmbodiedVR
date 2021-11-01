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
        _cameraDistance = 5f;
    }

    public void RotateCamera(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public bool IsFadingInProgress()
    {
        return _fadingInProgres;
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

    public void Fading(float duration, bool fadeout, bool fadeOverlay=false)
    {
        _fadingInProgres = true;
        if (fadeout)
        {
            SteamVR_Fade.Start(Color.black, duration, fadeOverlay); 
        }
        else
        {
            SteamVR_Fade.Start(Color.clear, duration,fadeOverlay); 
        }
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

    public void SetCameraDistance(float distance)
    {
        _cameraDistance = distance;
        var newlocalPosition = CameraArm.transform.localPosition;
        newlocalPosition = new Vector3(newlocalPosition.x,newlocalPosition.y,newlocalPosition.z -distance);
        CameraArm.transform.localPosition = newlocalPosition;
    }
    
    
    
    

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
    
    
}
