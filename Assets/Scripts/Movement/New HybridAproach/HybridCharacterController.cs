using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCharacterController : MonoBehaviour
{
    //GroundCheck Relevant
    public float Gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField]private Transform AdjustedPosition;
    private float groundCheckDistance= 0.4f;
    private float _verticalVelocityForce = 0;
    private bool isGrounded;
    
    public float ForwardSpeed;
    public float SideWaySpeed;
    public float BackwardSpeed;
    
    private float _speedFactor;

    private bool OrientationBasedOnCenter;
   
    private CharacterController _characterController;

    private float _currentSpeed;
    
    void Start()
    {
        _speedFactor = 1f;
        _characterController= GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck(transform.position, groundCheckDistance);
        _verticalVelocityForce =isGrounded ?  -4f : _verticalVelocityForce += Gravity * Time.deltaTime;
        Vector3 verticalVelocity = Vector3.up * _verticalVelocityForce;
        _characterController.Move(verticalVelocity*Time.deltaTime);
        
        
    }

    private void LateUpdate()
    {
        _currentSpeed = _characterController.velocity.magnitude;
    }

    public void MoveCharacter(Vector3 movementDirection)
    {
        
        var xDirection = transform.right;
        var zDirection = transform.forward;
        Vector3 move = (xDirection * (movementDirection.x * SideWaySpeed*_speedFactor) +
                        (movementDirection.y>=0? 
                            zDirection * (movementDirection.z * ForwardSpeed*_speedFactor): 
                            zDirection * (movementDirection.z * BackwardSpeed*_speedFactor) ));
        
        _characterController.Move(move*Time.deltaTime);
    }
    
    public void SetCharacterPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void SetAdjustmentPosition(Vector3 localPosition)
    {
        AdjustedPosition.transform.localPosition = localPosition;
        _characterController.center = AdjustedPosition.localPosition+(Vector3.up*_characterController.height/2);
    }

    public Vector3 GetAdjustedPosition()
    {
        return AdjustedPosition.transform.position;
    }

    public void AddLocalOffset(Vector3 offset)
    {
        this.transform.localPosition = offset;
    }

    public void SetSpeedFactor(float percentage)
    {
        _speedFactor = percentage;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }


    public void RotateCharacter(Quaternion rotation)
    {
        if (!OrientationBasedOnCenter)
        {
            this.transform.rotation = rotation;
        }
        else
        {
            AdjustedPosition.localRotation = rotation;
        }

    }

    public void SetOrientationBasedOnCharacter(bool state)
    {
        OrientationBasedOnCenter = state;
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
