using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PuppetController : MonoBehaviour
{

    private VRIK vriK;

    private CCAnimator ccAnimator;

    [SerializeField] private GameObject avatar;

    [SerializeField]private Animator _animator;

    private float _currentSpeed;

    private void Start()
    {
        ccAnimator = GetComponent<CCAnimator>();
        ccAnimator.SetAnimator(_animator);
    }
    
    private void FixedUpdate()
    {
        avatar.transform.localPosition = Vector3.zero;
    }
    


    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }


    public void RotateAvatar(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }
    public void MovePuppet(Vector3 direction)
    {
        //Debug.Log(direction + " " + _currentSpeed);
        ccAnimator.ApplyAnimation(direction, _currentSpeed);
    }
    
    
    
    

}
