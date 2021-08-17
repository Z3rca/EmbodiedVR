using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAnimator : MonoBehaviour
{
    private Animator _animator;
    private bool isMoving;

    public float maximumForward;
    public float maximumBackward;
    public float maximumSideward;


    private float animationSpeed;

    private bool _isEmbodiedCondition;


    public void SetIsEmbodiedCondition(bool state)
    {
        _isEmbodiedCondition = state;
    }
    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void ApplyAnimation(Vector3 direction, float speed)
    {
        if(Mathf.Abs(direction.x) > 0.01f || Mathf.Abs(direction.z) > 0.01f)
        {
            
            _animator.SetBool("IsMoving",true);
            ;
            
            
            if (speed > maximumForward)
            {
                _animator.SetFloat("Velocity", speed);
            }
            else
            {
                _animator.SetFloat("Velocity", speed);
            }
            
            _animator.SetFloat("X",direction.x);
            
            
            _animator.SetFloat("Z", direction.z);
            
        }
        else
        {
            _animator.SetBool("IsMoving",false);
            _animator.SetFloat("Velocity", 0f);
            _animator.SetFloat("X", 0f);
            _animator.SetFloat("Z", 0f);
        }
        


    }
    
}
