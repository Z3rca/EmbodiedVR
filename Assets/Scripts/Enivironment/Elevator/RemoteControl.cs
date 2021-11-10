using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using Obi;
using RootMotion;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Debug = UnityEngine.Debug;

public class RemoteControl : MonoBehaviour
{
    public GameObject attachedCable;
    public  SteamVR_ActionSet LiftActionset;
    [SerializeField] private GameObject Joystick;

    public SteamVR_Action_Vector2 MovementInput = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("LiftControl", "Direction");

   // private ObiParticleAttachment _cableAttachment; 
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
   //     ObiParticleAttachment[] particleAttachments = attachedCable.GetComponents<ObiParticleAttachment>();

     //  foreach (var attachment in particleAttachments)
        {
         //   if (attachment.target == this.transform)
            {
       //         _cableAttachment = attachment;
          //      break;
            }
        }
    }


    public void ActivateActionSet(bool state)
    {
        Debug.Log("is now on" + state);
        if (state)
        {
            activated = true;

        //    GetComponent<ObiRigidbody>().kinematicForParticles = true;
            /*_cableAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Static;
            Debug.Log("cable attachment is " + _cableAttachment.attachmentType);*/
        }
        else
        {
          //  GetComponent<ObiRigidbody>().kinematicForParticles = false;
            activated = false;
            LiftActionset.Deactivate(hand);
//            interactable.attachedToHand.gameObject.transform.parent = Origin.transform;

        }
        
    }


    private IEnumerator ReactivateAtachment()
    {
        //_cableAttachment.attachmentType = ObiParticleAttachment.AttachmentType.Dynamic;

        yield return new WaitForSeconds(0.1f);

     //   _cableAttachment.enabled=true;
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
