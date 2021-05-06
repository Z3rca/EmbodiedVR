using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class VRMovement : MonoBehaviour
{

    public GameObject Body;

    public GameObject Head;
    
    public SteamVR_Action_Vector2 MovementInput;
    public SteamVR_Action_Vector2 RotationInput;
    public SteamVR_Action_Boolean rotateLeft;
    public SteamVR_Action_Boolean rotateRight;
    public SteamVR_Action_Boolean switchPerspective;
    public SteamVR_ActionSet actionSetEnable;

    public bool SnapTurn;

    [Range(0.1f, 45)] public float SetRotationImpuls;

    private Vector2 movementInput;
    private Quaternion targetRotation;
    private bool rotationApplied;
    private float rotationImpuls;
    private bool _allowRotation=true;
    public delegate void OnButtonPressed();
    public event OnButtonPressed notifyLeftButtonPressedObserver;
    public event OnButtonPressed notifyRightButtonPressedObserver;
    public event OnButtonPressed notifySwitchButtonPressedObserver;
    
    private void Awake()
    {
        actionSetEnable.Activate();   
        movementInput = new Vector2();
    }

    // Start is called before the first frame update
    void Start()
    {
        switchPerspective.AddOnStateDownListener(SwitchPerspective,SteamVR_Input_Sources.Any);
        
        if (SnapTurn)
        {
            rotateLeft.AddOnStateUpListener(RotateLeft, SteamVR_Input_Sources.Any);
            rotateRight.AddOnStateUpListener(RotateRight, SteamVR_Input_Sources.Any);
        }
        else
        {
            rotateLeft.AddOnStateDownListener(RotateLeft,SteamVR_Input_Sources.Any);
            rotateRight.AddOnStateDownListener(RotateRight,SteamVR_Input_Sources.Any);
            rotateLeft.AddOnStateUpListener(RotateZero, SteamVR_Input_Sources.Any);
            rotateRight.AddOnStateUpListener(RotateZero, SteamVR_Input_Sources.Any);
        }
        
    }


    private void Update()
    {
        movementInput = MovementInput.GetAxis(SteamVR_Input_Sources.Any);
        
    }
    
    private void FixedUpdate()
    {
        

        
            targetRotation = transform.rotation;
            targetRotation *= Quaternion.Euler(0,rotationImpuls,0);
            Vector3 eulerRotation = new Vector3();
            eulerRotation= Vector3.ProjectOnPlane(targetRotation.eulerAngles, Vector3.forward);
            eulerRotation.x = 0f;
            eulerRotation += rotationImpuls*Vector3.forward;
            targetRotation = Quaternion.Euler(eulerRotation);
            targetRotation *= Quaternion.Euler(0, rotationImpuls, 0);


            Body.transform.rotation = targetRotation;
            
        if (rotationApplied&& SnapTurn)
        {
            rotationImpuls = 0f;
            rotationApplied = false;
        }
        else
        {
            rotationApplied = false;
        }
    }

    private void LateUpdate()
    {
        Head.transform.position = Vector3.Lerp(Head.transform.position,Body.transform.position, 10*Time.deltaTime);
    }

    public void RotateLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (_allowRotation)
        {
            notifyLeftButtonPressedObserver?.Invoke();
            rotationImpuls = -SetRotationImpuls;
            if (!SnapTurn)
            {
                rotationImpuls *= Time.deltaTime;
            }
            rotationApplied = true; 
        }
    }
    
    public void RotateRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (_allowRotation)
        {
            notifyRightButtonPressedObserver?.Invoke();
            rotationImpuls = SetRotationImpuls;
            if (!SnapTurn)
            {
                rotationImpuls *= Time.deltaTime;
            }
            rotationApplied = true;
        }
    }

    public void RotateZero(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        rotationImpuls = 0;
    }
    
    public void SwitchPerspective(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        notifySwitchButtonPressedObserver?.Invoke();
    }


    public void AllowRotation(bool state)
    {
        _allowRotation = state;
    }

    public void AllowMovement(bool state)
    {
        
    }
    
    
    public Vector2 GetCurrentInput()
    {
        return movementInput;
    }
    

    public Quaternion GetRotation()
    {
        return targetRotation;
    }

    /*public GameObject GetOrientation()
    {
        return Orientation;
    }*/
}
