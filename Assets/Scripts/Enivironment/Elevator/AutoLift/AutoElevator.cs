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

    public float speed;
    
    private Vector3 _boundedPosition;

    private bool reachedPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public void StartLift()
    {
        StartCoroutine(StartElevatorScript());
    }

    private IEnumerator StartElevatorScript()
    {
        reachedPosition = false;
        yield return MoveInDirection(speed, true, MiddlePosition.transform.position);
        Debug.Log("finished middle step");
        reachedPosition = false;
        yield return new WaitForSeconds(2f);
        yield return MoveInDirection(speed, true, UpperBoundary.transform.position);
        Debug.Log("finished lift");
    }

    private IEnumerator MoveInDirection(float speed, bool upward, Vector3 targetPosition)
    {
        while (!reachedPosition)
        {
            float posY=elevator.transform.position.y;

            if (upward)
            {
                posY += speed * Time.deltaTime;
            }
            
            posY =  Mathf.Clamp(posY,  
                (LowerBoundary.transform.position.y), 
                (UpperBoundary.transform.position.y));

            _boundedPosition = new Vector3(LowerBoundary.transform.position.x, posY, LowerBoundary.transform.position.z);

           elevator.transform.position = _boundedPosition;

            if (posY >= targetPosition.y)
            {
                reachedPosition = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
