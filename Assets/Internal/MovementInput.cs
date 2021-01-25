using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovementInput : MonoBehaviour
{
    public bool DisableSideWayMovement;
    
    public SteamVR_ActionSet actionSetEnable;
    public SteamVR_Action_Vector2 Input;

    private NavMeshMovement Mover;
    

    private bool UseIsPressed; 
    
    private bool InputAllowed;
    


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

        Vector2 Movement = Input.GetAxis(SteamVR_Input_Sources.Any);

        if (DisableSideWayMovement)
        {
            Movement.x = 0;
        }
        
        if(InputAllowed)
            Mover.Move(Movement);
        
    }
    


    public void AllowInput(bool state)
    {
        InputAllowed= state;
    }

   

    

    private void SetUseState(bool state=true)
    {
        UseIsPressed = state;
    }
}
