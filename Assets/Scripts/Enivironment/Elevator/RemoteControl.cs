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

    public SteamVR_Action_Vector2 MovementInput = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("LiftControl", "Direction");
    
    
    private Vector2 movementInput;

    public bool AllowInputX;
    public bool AllowInputY;
    
    private float InputValue;

    private bool activated;
    private SteamVR_Input_Sources hand;
    
    private Interactable interactable;
    public event EventHandler<InputArgs> OnInputReceiver;
    
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        
    }


    public void ActivateActionSet(bool state)
    {
        Debug.Log("is now on" + state);
        if (state)
        {
            activated = true;
           
        }
        else
        {
           
            activated = false;
            LiftActionset.Deactivate(hand);
//            interactable.attachedToHand.gameObject.transform.parent = Origin.transform;

        }
        
    }




    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (interactable.attachedToHand)
            {
               // Origin= interactable.attachedToHand.gameObject.transform.parent.parent.gameObject;

               // interactable.attachedToHand.gameObject.transform.parent = RemoteTransform.transform;
                hand = interactable.attachedToHand.handType;
                Debug.Log(hand);
                LiftActionset.Activate(hand);
                
            }
            
            Vector2 input = new Vector2();
            input = MovementInput.GetAxis(hand);
            
            Vector3  resultMovement= new Vector3();
            if (AllowInputY)
            {
                resultMovement += new Vector3(0, 0, input.y);
            }
            else
            {
                input.y = 0;
            }
            if (AllowInputX)
            {
                resultMovement += new Vector3(input.x, 0,0 );
            }
            else
            {
                input.x = 0;
            }
            
            Joystick.transform.localPosition = resultMovement*0.015f;

            InputArgs args = new InputArgs();

            args.inputValue = input;


            OnInputReceiver?.Invoke(this, args);
            
        }
    }

    
}



public class InputArgs : EventArgs
{
    public Vector2 inputValue;
}
