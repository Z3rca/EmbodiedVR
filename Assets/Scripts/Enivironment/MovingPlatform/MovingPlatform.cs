using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    [SerializeField] private float speed;

    private bool checkedIn;

    [SerializeField] private bool StartOnTriggerEnter;

    [SerializeField] private UnityEvent ActivationEvents;
    
    [SerializeField] private UnityEvent ExitEvents;

    public GameObject invisibleWallObject;


    private GameObject agent;
    
    
    private bool allowMoving;

    private bool AllowOuterImpactFactor=true;
    
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
        if (!AllowOuterImpactFactor)
            return;
        if (other.GetComponent<HybridCharacterController>())
        {
            other.GetComponent<HybridCharacterController>().AddOuterMovementImpact(direction,speed);
        }
    }

    public void DeactivateInvisibleWall()
    {
        invisibleWallObject.SetActive(false);
    }

    public void ActivatePlatformFunction()
    {
        invisibleWallObject.SetActive(true);
        AllowOuterImpactFactor = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<HybridCharacterController>())
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
        
        if (other.GetComponent<HybridCharacterController>())
        {
            agent = null;
            ExitEvents.Invoke();
            other.GetComponent<HybridCharacterController>().AddOuterMovementImpact(Vector3.zero, 0f);
        }
    }

    public void RemoveOuterMovementImpact()
    {
        AllowOuterImpactFactor = false;
    }


    private void OnDisable()
    {
        if (agent != null)
        {
            agent.GetComponent<HybridCharacterController>().AddOuterMovementImpact(Vector3.zero, 0f);
        }
       
    }
}
