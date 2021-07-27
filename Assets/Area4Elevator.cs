using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Area4Elevator : MonoBehaviour
{

    public GameObject elevator;

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
    void Start()
    {
        posX = elevator.transform.position.x;
        posZ = elevator.transform.position.z;
        
        lowerDoor.SetActive(false);
        lowerDoor.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
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

    public void MoveUpwards()
    {
        if (elevator.transform.position.y != upperBoundary)
        {
            lowerDoor.SetActive(true);
            
            float posY = elevator.transform.position.y;
            
            posY += speed * Time.deltaTime;
            
            posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));

            elevator.transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            upperDoor.SetActive(false);
        }
    }
    
    public void MoveDownwards()
    {
        if (elevator.transform.position.y != lowerBoundary)
        {
            upperDoor.SetActive(true);
            
            float posY = elevator.transform.position.y;
            
            posY -= speed * Time.deltaTime;
            
            posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));

            elevator.transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            lowerDoor.SetActive(false);
        }
    }
    
}
