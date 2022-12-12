using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    [SerializeField] private ElevatorCheck elevatorCheck;
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject MiddlePosition;
    [SerializeField] private GameObject LowerBoundary;
    [SerializeField] private List<GameObject> Doors;


    private GameObject TargetPosition;
    private bool upward;
    public float speed;
    public bool startsUpward;

    private bool isUp;
    
    private Vector3 _boundedPosition;

    private bool reachedPosition;
    private bool moving;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var door in Doors)
        {
            door.SetActive(false);
        }

        if (startsUpward)
            elevator.transform.position = UpperBoundary.transform.position;
        else
        {
            elevator.transform.position = LowerBoundary.transform.position;
        }
        _boundedPosition = elevator.transform.position;
        
        
    }

    private void Update()
    {
      
        
        if (reachedPosition && elevatorCheck.IsAvatarIsInside())
        {
            if ((elevatorCheck.isUpstairs()&&isUp)||(!elevatorCheck.isUpstairs()&&!isUp))
                return;
            StartLift();
        }
    }

    void FixedUpdate()
    {
        elevator.transform.position = _boundedPosition;
    }


    private bool CheckForUpward()
    {
        bool up = false;
        if (elevator.transform.position.y > LowerBoundary.transform.position.y)
        {
            Debug.Log("down" + this.gameObject.name);
            up = false;
            TargetPosition = LowerBoundary;
        }
        else if (elevator.transform.position.y < UpperBoundary.transform.position.y)
        {
            up = true;
            Debug.Log("up"+this.gameObject.name);
            TargetPosition = UpperBoundary;
        }

        return up;
    }
    public void StartLift()
    {
        Debug.Log("... start lift");
        if (moving)
            return;
        
        moving = true;
        upward = CheckForUpward();
        
        StartCoroutine(StartElevatorScript(TargetPosition));
    }

    private IEnumerator StartElevatorScript(GameObject Target)
    {
        reachedPosition = false;
        foreach (var door in Doors)
        {
            door.SetActive(true);
        }
        Debug.Log("middleman" + upward);
        if(MiddlePosition!=null)
            yield return MoveInDirection(speed, MiddlePosition.transform.position);
        
        reachedPosition = false;
        yield return new WaitForSeconds(2f);
        yield return MoveInDirection(speed, Target.transform.position);
        Debug.Log("finished lift");
        
        foreach (var door in Doors)
        {
            door.SetActive(false);
        }
        moving = false;
    }

    private IEnumerator MoveInDirection(float speed, Vector3 targetPosition)
    {
        moving = true;
        

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
                    (LowerBoundary.transform.position.y), 
                    (UpperBoundary.transform.position.y));
            }
            
            

            _boundedPosition = new Vector3(elevator.transform.position.x, posY, elevator.transform.position.z);


            if (upward)
            {
                if (posY >= targetPosition.y)
                {
                    reachedPosition = true;
                    isUp = true;
                }
            }
            else
            {
                if (posY <= targetPosition.y)
                {
                    reachedPosition = true;
                    isUp = false;
                }
            }


            
            yield return new WaitForFixedUpdate();
        }
        
      
        
    }
}
