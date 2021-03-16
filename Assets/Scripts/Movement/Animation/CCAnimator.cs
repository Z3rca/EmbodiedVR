using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAnimator : MonoBehaviour
{
    private Animator animator;
    private bool isMoving;

    public float maximumForward;
    public float maximumBackward;
    public float maximumSideward;


    private float animationSpeed;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void LateUpdate()
    {
        //animationSpeed = animator.GetCurrentAnimatorStateInfo().
        //Debug.Log(animationSpeed = animator.GetCurrentAnimatorClipInfo().);
    }

    public void ApplyAnimation(Vector2 direction, float speed)
    {
        
        if(Mathf.Abs(direction.x) > 0.01f || Mathf.Abs(direction.y) > 0.01f)
        {
            
            animator.SetBool("IsMoving",true);
            ;
            
            
            if (speed > maximumForward)
            {
                animator.SetFloat("Velocity", speed);
            }
            else
            {
                animator.SetFloat("Velocity", speed);
            }
            
            animator.SetFloat("X",direction.x);
            
            
            animator.SetFloat("Z", direction.y);
            
        }
        else
        {
            animator.SetBool("IsMoving",false);
            animator.SetFloat("Velocity", 0f);
            animator.SetFloat("X", 0f);
            animator.SetFloat("Z", 0f);
        }
        


    }
    
}
