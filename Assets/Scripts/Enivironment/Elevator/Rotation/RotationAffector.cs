using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class RotationAffector : MonoBehaviour
{

    public Rigidbody rb;
    public float speed;

    private Vector3 lastPosition;

    private Vector3 velocity;

    private HybridCharacterController movement;

    private bool active;
    private bool movementAssigned;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementAssigned)
            return;

        if (active)
        {
            velocity = transform.forward;
            movement.AddOuterMovementImpact(transform.forward,speed);
        }
        else
        {
            movement.AddOuterMovementImpact(Vector3.zero, 0f);
        }

        //velocity = lastPosition - transform.position;
        //speed = (velocity).magnitude;
    }
    


    public void SetPhysicalMovement(HybridCharacterController phyM)
    {
        movement = phyM;
        movementAssigned = true;

    }

    public void SetActive(bool state)
    {
        active = state;
    }

    
}
