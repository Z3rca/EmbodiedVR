using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Area4Elevator : MonoBehaviour
{
    public GameObject lowerDoor;
    public GameObject upperDoor;

    public LinearMapping linearMapping;
    
    public float upperBoundary;
    public float lowerBoundary;

    public float speed;

    private float posX;
    private float posZ;
    
    private float currentLinearMapping = float.NaN;


    // Start is called before the first frame update
    private void Start()
    {
        posX = gameObject.transform.position.x;
        posZ = gameObject.transform.position.z;
        
        lowerDoor.SetActive(false);
        upperDoor.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (currentLinearMapping != linearMapping.value)
        {
            currentLinearMapping = linearMapping.value;
        }

        if (currentLinearMapping > 0.95)
        {
            MoveUpwards();
        }
        
        if (currentLinearMapping < 0.05)
        {
            MoveDownwards();
        }
    }

    private void MoveUpwards()
    {
        if (gameObject.transform.position.y < upperBoundary - 0.58)
        {
            lowerDoor.SetActive(true);
            
            var posY = gameObject.transform.position.y;
            
            posY += speed * Time.deltaTime;
            
            // posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));

            gameObject.transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            upperDoor.SetActive(false);
        }
    }

    private void MoveDownwards()
    {
        if (gameObject.transform.position.y > lowerBoundary - 0.58)
        {
            upperDoor.SetActive(true);
            
            var posY = gameObject.transform.position.y;
            
            posY -= speed * Time.deltaTime;
            
            // posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));

            gameObject.transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            lowerDoor.SetActive(false);
        }
    }
    
}
