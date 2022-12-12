using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    
    public RemoteControl remoteControl;
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject LowerBoundary;

    [SerializeField] private float Speed;

    private float _inputX;
    private float _inputY;

    private Vector3 targetPos;

    private bool upward;
    private bool isMoving;
    
    // Start is called before the first frame update
    void Start()
    {
        remoteControl.OnInputReceiver += Move;
        targetPos = elevator.transform.position;
    }
    


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        if (elevator.transform.position != targetPos)
        {
            isMoving = true;
            elevator.transform.position = targetPos;
        }
        else
        {
            isMoving = false;
        }
    }

    // Update is called once per frame


    private void Move(object sender, InputArgs input)
    {
        isMoving = true;
        _inputX = input.inputValue.x;
        _inputY = input.inputValue.y;

        
        var position = elevator.transform.position;
        float posY = position.y;
        float posX = position.x;
        float posZ = position.z;
        posY += (_inputY * Speed * Time.deltaTime);
        posY =  Mathf.Clamp(posY,  
            (LowerBoundary.transform.position.y), 
            (UpperBoundary.transform.position.y));
        
         targetPos= new Vector3(posX, posY, posZ);;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            if (isMoving)
            {
                if (upward)
                {
                    other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(Vector3.up, Speed);
                }
                else
                {
                    other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(Vector3.down, Speed);
                } 
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(Vector3.zero, 0f);
        }
    }
}
