using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Area4Elevator : MonoBehaviour
{

    public GameObject elevator;

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
        
    }

    // Update is called once per frame
    void Update()
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
        if (elevator.transform.position.y <= upperBoundary)
        {
            float posY = elevator.transform.position.y;
            
            posY += speed * Time.deltaTime;

            elevator.transform.position = new Vector3(posX, posY, posZ);
        }
    }
    
    public void MoveDownwards()
    {
        if (elevator.transform.position.y >= lowerBoundary)
        {
            float posY = elevator.transform.position.y;
            
            posY -= speed * Time.deltaTime;

            elevator.transform.position = new Vector3(posX, posY, posZ);
        }
    }
}
