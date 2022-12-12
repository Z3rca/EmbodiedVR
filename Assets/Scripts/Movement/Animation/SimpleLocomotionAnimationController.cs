using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleLocomotionAnimationController: MonoBehaviour
{
    private CharacterController characterController;
    

    private MovementData movementData;
    private Animator animator;
    public GameObject camera;
    private float lastSpeed;
    private float currentSpeed;
    private float smoothedSpeed;
    private MovementSystemManager movementSystemManager;

    private Vector3 currentPositionOnGround;

    private Quaternion currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        movementSystemManager = MovementSystemManager.Instance; 
        movementSystemManager.InputEvent += ReadInputAsAnimation;
        animator = this.gameObject.GetComponent<Animator>();
        lastSpeed= new float();
        currentPositionOnGround = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = currentPositionOnGround;
      //  this.transform.rotation = currentRotation;
    }

    private void LateUpdate()
    {
        //animator.SetFloat("Velocity", Mathf.Lerp(animator.velocity.magnitude,lastSpeed,Time.deltaTime));
       // lastSpeed = animator.velocity.magnitude;
    }


    private void ReadInputAsAnimation(object sender, MovementData movementData)
    {
        currentSpeed = movementData.speed;
        currentPositionOnGround = movementData.WorldPosition;
        currentRotation = movementData.RotationInWorld;
        smoothedSpeed = Mathf.Lerp(currentSpeed, lastSpeed, Time.deltaTime);
//           Debug.Log("received input " + movementData.speed );
        if (movementData.input != Vector2.zero)
        {
            
            //animator.speed = movementData.speed;
            animator.SetFloat("Velocity",smoothedSpeed);
    //        Debug.Log("anim" + animator.GetFloat("Velocity"));
            animator.SetBool("IsMoving",true);
            animator.SetFloat("X", movementData.input.x);
            animator.SetFloat("Z", movementData.input.y);
            
        }
        else
        {
            animator.SetFloat("X", 0f);
            animator.SetFloat("Z", 0f);
            animator.SetFloat("Velocity", 0f);
            GetComponent<Animator>().SetBool("IsMoving",false);
        }

        lastSpeed = currentSpeed;
    }
    
    
    
    
    
}
