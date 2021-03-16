using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PhysicalMovement : MonoBehaviour
{
    public Transform feet;
    public float sideWaySpeed;
    public float speed;
    private CharacterController controller;
    
    private Vector3 velocity;
    
    private Rigidbody rb;
    
    public float Gravity = -9.81f;

    public float groundDistance= 0.4f;
    
    public Vector3 gravitationForce;
    private float verticalVelocityForce;

    private bool isGrounded;
    public LayerMask groundMask;

    private float currentSpeed;
    private Vector2 direction;

    public GameObject puppet;
    private CCAnimator ccAnimator;


    public float speedFront;
    public float speedBack;
    public float speedLeft;
    public float speedRight;




    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        ccAnimator = puppet.GetComponent<CCAnimator>();
    }

    private void Update()
    {
        isGrounded = GroundCheck(feet.position, groundDistance);
        
      
      
        
        velocity = Vector3.up * verticalVelocityForce;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

      
//        Debug.Log(x + " " + z);
        direction.x = x;
        direction.y = z;


        Vector3 move = (transform.right * (x * sideWaySpeed) + (z>=0? transform.forward * (z * speed): transform.forward * (z * sideWaySpeed) ));
        
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
      currentSpeed = (controller.velocity.magnitude);
      
      verticalVelocityForce =isGrounded ?  -0.4f : verticalVelocityForce += Gravity * Time.deltaTime;
       // rb.velocity = transform.up * (Gravity * Time.deltaTime);
        
    }
}
