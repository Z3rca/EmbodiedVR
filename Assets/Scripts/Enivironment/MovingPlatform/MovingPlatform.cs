using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    
    [SerializeField] private GameObject ActivationTrigger;
    
    [SerializeField] private Vector3 direction;

    [SerializeField] private float speed;

    private bool checkedIn;

    [SerializeField] private bool StartOnTriggerEnter;

    [SerializeField] private UnityEvent ActivationEvents;
    
    [SerializeField] private UnityEvent ExitEvents;


    private GameObject agent;
    
    
    private bool allowMoving;
    
    // Start is called before the first frame update
    void Start()
    {
        if (StartOnTriggerEnter)
        {
            allowMoving = false;
        }
        else
        {
            allowMoving = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(allowMoving)
            this.transform.position += direction * (Time.deltaTime * speed);
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(direction,speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<PhysicalMovement>())
        {
            agent = other.gameObject;
            if (StartOnTriggerEnter)
            {
                allowMoving = true;
            }
            
           ActivationEvents.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        
        if (other.GetComponent<PhysicalMovement>())
        {
            agent = null;
            ExitEvents.Invoke();
            other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(Vector3.zero, 0f);
        }
    }


    private void OnDisable()
    {
        if (agent != null)
        {
            agent.GetComponent<PhysicalMovement>().AddOuterMovementImpact(Vector3.zero, 0f);
        }
       
    }
}
