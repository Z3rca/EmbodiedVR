using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject LowerBoundary;

    [SerializeField] private float Speed;

    private float posY;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    


    private void FixedUpdate()
    {
        
        
        
        if (Input.GetKey(KeyCode.B))
        {
            var position = elevator.transform.position;
            float input = 1f;
            posY = position.y;
            float posX = position.x;
            float posZ = position.z;
            posY += (input * Speed * Time.deltaTime);
            posY =  Mathf.Clamp(posY,  
                (LowerBoundary.transform.position.y), 
                (UpperBoundary.transform.position.y));
            elevator.transform.position = new Vector3(posX, posY, posZ);;
        }
        
        if (Input.GetKey(KeyCode.C))
        {
            var position = elevator.transform.position;
            float input = -1;
            posY = position.y;
            float posX = position.x;
            float posZ = position.z;
            posY += (input * Speed * Time.deltaTime);
            posY =  Mathf.Clamp(posY,  
                (LowerBoundary.transform.position.y), 
                (UpperBoundary.transform.position.y));
            elevator.transform.position = new Vector3(posX, posY, posZ);;
        }
    }
}
