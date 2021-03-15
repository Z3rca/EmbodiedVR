using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PhysicalMovement : MonoBehaviour
{
    public Transform feet;
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
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = GroundCheck(feet.position, groundDistance);
        
      
        verticalVelocityForce =isGrounded ?  -0.4f : verticalVelocityForce += Gravity * Time.deltaTime;
        
        velocity = Vector3.up * verticalVelocityForce;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = (transform.right * x + transform.forward * z);
        
        controller.Move(((move*speed)+velocity)*Time.deltaTime);


       // controller.Move(*Time.deltaTime);
        
        
     
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
        Debug.Log(controller.velocity.magnitude*Time.deltaTime);
       // rb.velocity = transform.up * (Gravity * Time.deltaTime);
        
    }
}
