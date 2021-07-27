using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCharacterController : MonoBehaviour
{
    //GroundCheck Relevant
    public float Gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField]private Transform CharacterFeetPosition;
    private float groundCheckDistance= 0.4f;
    private float _verticalVelocityForce = 0;
    private bool isGrounded;
    
    public float ForwardSpeed;
    public float SideWaySpeed;
    
    private float _speedFactor;
   
    private CharacterController _characterController;
    
    void Start()
    {
        _speedFactor = 1f;
        _characterController= GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck(CharacterFeetPosition.position, groundCheckDistance);
        _verticalVelocityForce =isGrounded ?  -4f : _verticalVelocityForce += Gravity * Time.deltaTime;
        Vector3 verticalVelocity = Vector3.up * _verticalVelocityForce;
        _characterController.Move(verticalVelocity*Time.deltaTime);
        
    }

    public void MoveCharacter(Vector3 movementDirection)
    {
        
        var xDirection = transform.right;
        var zDirection = transform.forward;
        Vector3 move = (xDirection * (movementDirection.x * SideWaySpeed*_speedFactor) +
                        (movementDirection.y>=0? 
                            zDirection * (movementDirection.z * ForwardSpeed*_speedFactor): 
                            zDirection * (movementDirection.z * SideWaySpeed*_speedFactor) ));
        
        _characterController.Move(move*Time.deltaTime);
    }

    public void SetSpeedFactor(float percentage)
    {
        _speedFactor = percentage;
    }


    public void RotateCharacter(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }


    public Vector3 GetCharacterFeetPosition()
    {
        return CharacterFeetPosition.transform.position;
    }
    
    public Vector3 GetGeneralCharacterPosition()
    {
        return this.transform.position;
    }
    
    
    
    private bool GroundCheck(Vector3 position, float radius)
    {
        if (Physics.CheckSphere(position, radius,groundMask))
        {
            return true;
        }
        return false;
    }
    
    
}
