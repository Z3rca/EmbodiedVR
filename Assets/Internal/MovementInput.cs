using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovementInput : MonoBehaviour
{
    private bool _disableSideWayMovement;
    
    public SteamVR_ActionSet actionSetEnable;
    public SteamVR_Action_Vector2 Input;

    private NavMeshMovement Mover;
    

    private bool UseIsPressed; 
    
    private bool InputAllowed;
    


    private Vector2 movementInput;
    
    // Start is called before the first frame update
    void Start()
    {
        InputAllowed = true;
        actionSetEnable.Activate();
        Mover = GetComponent<NavMeshMovement>();

        // Use.AddOnActiveChangeListener,SteamVR_Input_Sources.Any);
        
        //Use.AddOnStateDownListener(TriggerUp, SteamVR_Input_Sources.Any);
        UseIsPressed = false;

       
    }

    // Update is called once per frame
    void Update()
    {

        movementInput = Input.GetAxis(SteamVR_Input_Sources.Any);
        
        MovementData movementData= new MovementData();
        movementData.input = movementInput;
        //TODO still missing inputData.timeStamp;

        if (_disableSideWayMovement)
        {
            movementInput.x = 0;
        }
        
        if(InputAllowed)
            Mover.Move(movementInput);
        
    }
    


    public void AllowInput(bool state)
    {
        InputAllowed= state;
    }



    public void DisableSideWayMovement(bool state)
    {
        _disableSideWayMovement = state;
    }

    private void SetUseState(bool state=true)
    {
        UseIsPressed = state;
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }
}
