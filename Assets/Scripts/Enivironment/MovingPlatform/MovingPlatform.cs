using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    [SerializeField] private float speed;

    private bool checkedIn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += direction * (Time.deltaTime * speed);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            other.GetComponent<PhysicalMovement>().AddOuterMovementImpact(direction,speed);
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
