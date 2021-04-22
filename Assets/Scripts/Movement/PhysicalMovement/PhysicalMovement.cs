using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Valve.VR;

public class PhysicalMovement : MonoBehaviour
{
    public Transform feet;
    public float sideWaySpeed;
    public float speed;
    private CharacterController controller;
    
    private Vector3 velocity;
    
    
    public float Gravity = -9.81f;
    public LayerMask groundMask;
    public float groundCheckDistance= 0.4f;
    
    private float verticalVelocityForce;

    private bool isGrounded;
 

    private float currentSpeed;
    private Vector2 direction;
    private Quaternion orientation;

    public GameObject puppet;
    private CCAnimator ccAnimator;


    private VRMovement vrMovement;


    
    private void Start()
    {
       
        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
       
        ccAnimator = puppet.GetComponent<CCAnimator>();
        vrMovement = GetComponent<VRMovement>(); 
    }

    private void Update()
    {
        isGrounded = GroundCheck(feet.position, groundCheckDistance);


        direction = vrMovement.GetCurrentInput();


        //orientation = vrMovement.GetRotation();
        
        velocity = Vector3.up * verticalVelocityForce;


//        Debug.Log(x + " " + z);



        Vector3 move = (transform.right * (direction.x * sideWaySpeed) + (direction.y>=0? transform.forward * (direction.y * speed): transform.forward * (direction.y * sideWaySpeed) ));
        
        controller.Move(((move)+velocity)*Time.deltaTime);


       // controller.Move(*Time.deltaTime);

       ccAnimator.ApplyAnimation(direction, currentSpeed);

       puppet.transform.position = feet.transform.position;

       
    }
    
    
    private bool GroundCheck(Vector3 position, float radius)
    {
        if (Physics.CheckSphere(position, radius,groundMask))
        {
            //Debug.Log(position + " grounded");
            return true;
        }
       
        return false;
    }
    
    private void FixedUpdate()
    {
        transform.rotation = vrMovement.GetRotation();
      currentSpeed = (controller.velocity.magnitude);
      
      verticalVelocityForce =isGrounded ?  -0.4f : verticalVelocityForce += Gravity * Time.deltaTime;
       // rb.velocity = transform.up * (Gravity * Time.deltaTime);
        
    }
}
