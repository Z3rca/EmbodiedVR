﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject MiddlePosition;
    [SerializeField] private GameObject LowerBoundary;
    [SerializeField] private GameObject Door;

    public float speed;
    
    private Vector3 _boundedPosition;

    private bool reachedPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Door.SetActive(false);
        _boundedPosition = elevator.transform.position;
    }

    // Update is called once per frame

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
        Door.SetActive(true);
        yield return MoveInDirection(speed, true, MiddlePosition.transform.position);
        Debug.Log("finished middle step");
        reachedPosition = false;
        yield return new WaitForSeconds(2f);
        yield return MoveInDirection(speed, true, UpperBoundary.transform.position);
        Debug.Log("finished lift");
        Door.SetActive(false);
    }

    private IEnumerator MoveInDirection(float speed, bool upward, Vector3 targetPosition)
    {
        while (!reachedPosition)
        {
            float posY=_boundedPosition.y;

            if (upward)
            {
                posY += speed * Time.deltaTime;
            }
            
            posY =  Mathf.Clamp(posY,  
                (LowerBoundary.transform.position.y), 
                (UpperBoundary.transform.position.y));

            _boundedPosition = new Vector3(LowerBoundary.transform.position.x, posY, LowerBoundary.transform.position.z);

         

            if (posY >= targetPosition.y)
            {
                reachedPosition = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
