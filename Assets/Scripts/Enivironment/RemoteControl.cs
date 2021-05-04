using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RemoteControl : MonoBehaviour
{
    
    
    public  SteamVR_ActionSet LiftActionset;
    [SerializeField] private GameObject Joystick;
    
    private SteamVR_Action_Vector2 MovementInput;

    public UnityEvent<Vector2> OutputEvent;
    
    private Vector2 movementInput;

    public bool AllowInputX;
    public bool AllowInputY;


    private float InputValue;

    private bool activated;
    private Hand hand;
    
    
    public event EventHandler<InputArgs> OnInputReceiver;

    // Start is called before the first frame update
    void Start()
    {
        MovementInput = SteamVR_Input.GetAction<SteamVR_Action_Vector2>(LiftActionset.GetShortName(), "Direction");
    }


    public void ActivateActionSet(bool state)
    {
        Debug.Log("is now on" + state);
        if (state == true)
        {
            LiftActionset.Activate();
            activated = true;
            hand = this.gameObject.GetComponent<Interactable>().attachedToHand;
            
        }
        else
        {
            activated = false;
            LiftActionset.Deactivate();
        }
        
    }




    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            
            movementInput = MovementInput.axis;
            Vector3  resultMovement= new Vector3();
            if (AllowInputY)
            {
                resultMovement += new Vector3(0, 0, movementInput.y);
            }
            else
            {
                movementInput.y = 0;
            }
            if (AllowInputX)
            {
                resultMovement += new Vector3(movementInput.x, 0,0 );
            }
            else
            {
                movementInput.x = 0;
            }
            
            Joystick.transform.localPosition = resultMovement*0.015f;

            InputArgs args = new InputArgs();

            args.inputValue = movementInput;


            OnInputReceiver?.Invoke(this, args);
            
        }
    }

    
}



public class InputArgs : EventArgs
{
    public Vector2 inputValue;
}
