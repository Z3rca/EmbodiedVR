﻿using System;
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


    private float _maximumSpeed;
    private float _currentSpeed;
    

    private Vector3 _currentDirection;
    

    private void Start()
    {
        ccAnimator = GetComponent<CCAnimator>();
        ccAnimator.SetAnimator(_animator);
        vriK = GetComponent<VRIK>();

        _maximumSpeed = ccAnimator.maximumForward;

    }
    
    private void FixedUpdate()
    {
        avatar.transform.localPosition = Vector3.zero;

        if (_currentSpeed < 0.01f)
        {
          //  ccAnimator.ApplyAnimation(Vector3.zero, 0f);
        }
        else
        {
            avatar.transform.localEulerAngles = Vector3.zero;
            ccAnimator.ApplyAnimation(_currentDirection, _currentSpeed);
        }
        //locomotion Effect for standing and readjustment.
        vriK.solver.locomotion.weight =1-  _currentSpeed / _maximumSpeed;

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
        _currentDirection = direction;
    }
    
    
    
    
    

}
