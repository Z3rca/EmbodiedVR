using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    
    public RemoteControl remoteControl;
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject UpperBoundary;
    [SerializeField] private GameObject LowerBoundary;

    [SerializeField] private float Speed;

    private float _inputX;
    private float _inputY;

    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        remoteControl.OnInputReceiver += Move;
        targetPos = elevator.transform.position;
    }
    


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        elevator.transform.position = targetPos; 
    }

    // Update is called once per frame


    private void Move(object sender, InputArgs input)
    {
        _inputX = input.inputValue.x;
        _inputY = input.inputValue.y;
        
        var position = elevator.transform.position;
        float posY = position.y;
        float posX = position.x;
        float posZ = position.z;
        posY += (_inputY * Speed * Time.deltaTime);
        posY =  Mathf.Clamp(posY,  
            (LowerBoundary.transform.position.y), 
            (UpperBoundary.transform.position.y));
        
         targetPos= new Vector3(posX, posY, posZ);;
    }

    
}
