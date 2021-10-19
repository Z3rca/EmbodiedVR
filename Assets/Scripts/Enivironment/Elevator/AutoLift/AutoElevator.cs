using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject MiddlePosition;
    [SerializeField] private GameObject LowerBoundary;
    [SerializeField] private List<GameObject> Doors;

    public bool upward;
    public float speed;
    
    private Vector3 _boundedPosition;

    private bool reachedPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var door in Doors)
        {
            door.SetActive(false);
        }
        _boundedPosition = elevator.transform.position;
        
        
    }



    void FixedUpdate()
    {
        elevator.transform.position = _boundedPosition;
    }

    
    
    public void StartLift()
    {
        StartCoroutine(StartElevatorScript());
    }

    private IEnumerator StartElevatorScript()
    {
        reachedPosition = false;
        foreach (var door in Doors)
        {
            door.SetActive(true);
        }
        
        if(MiddlePosition!=null)
            yield return MoveInDirection(speed, MiddlePosition.transform.position);
        
        reachedPosition = false;
        yield return new WaitForSeconds(2f);
        yield return MoveInDirection(speed, UpperBoundary.transform.position);
        Debug.Log("finished lift");
        
        foreach (var door in Doors)
        {
            door.SetActive(false);
        }
    }

    private IEnumerator MoveInDirection(float speed, Vector3 targetPosition)
    {
        while (!reachedPosition)
        {
            float posY=_boundedPosition.y;

            if (upward)
            {
                posY += speed * Time.deltaTime;
                posY =  Mathf.Clamp(posY,  
                    (LowerBoundary.transform.position.y), 
                    (UpperBoundary.transform.position.y));
            }
            else
            {
                posY -= speed * Time.deltaTime;
                posY =  Mathf.Clamp(posY,  
                    (UpperBoundary.transform.position.y), 
                    (LowerBoundary.transform.position.y));
            }
            
            

            _boundedPosition = new Vector3(elevator.transform.position.x, posY, elevator.transform.position.z);


            if (upward)
            {
                if (posY >= targetPosition.y)
                {
                    reachedPosition = true;
                }
            }
            else
            {
                if (posY <= targetPosition.y)
                {
                    reachedPosition = true;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
