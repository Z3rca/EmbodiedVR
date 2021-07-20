using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Daydream;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

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


    private HybridControl hybridControl;

    public bool SnapTurn;

    [Range(0.1f, 45)] public float SetRotationImpuls;
    [Range(0.0f, 1)] public float SetAutoRotationImpulsDuration = 0.5f;

    private Vector2 movementInput;
    private Quaternion targetRotation;
    private bool rotationApplied;
    private float rotationImpuls;
    private bool _allowRotation=true;
    public delegate void OnButtonPressed();
    public event OnButtonPressed notifyLeftButtonPressedObserver;
    public event OnButtonPressed notifyRightButtonPressedObserver;
    public event OnButtonPressed notifySwitchButtonPressedObserver;
    
    private bool rotating;
    
    [SerializeField] private RemoteVR remoteVR;
    [SerializeField] private bool _readjustBodyToCenter;
    private bool _readjusted;
    private bool temporaryIK;

    private bool _allowInput;
    

    private void Awake()
    {
        actionSetEnable.Activate();   
        movementInput = new Vector2();
    }

    // Start is called before the first frame update
    void Start()
    {
        hybridControl = GetComponent<HybridControl>();
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


        if (remoteVR == null)
        {
            _readjustBodyToCenter = false;
        }
        
    }


    private void Update()
    {
        movementInput = MovementInput.GetAxis(SteamVR_Input_Sources.Any);
        
    }
    
    private void FixedUpdate()
    {
        
        
        if (rotateLeft.state || rotateRight.state)
        {
            if (rotateLeft.state)
            {
                rotationImpuls = -SetRotationImpuls;
                notifyLeftButtonPressedObserver?.Invoke();
            }

            if (rotateRight.state)
            {
                rotationImpuls = SetRotationImpuls;
                notifyRightButtonPressedObserver?.Invoke();
            }
                
            
            if (!SnapTurn)
            {
                rotationImpuls *= Time.deltaTime;
            }
            
            if (!rotating)
            {
                rotating = true;
                StartCoroutine(PerformRotationImpuls());
            }
        }

        if (!(rotateLeft.state || rotateRight.state))
        {
            //Debug.Log("released input");
            rotating = false;
        }

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


    private IEnumerator PerformRotationImpuls()
    {
        while (rotating)
        {
            
            targetRotation = transform.rotation;
            //targetRotation *= Quaternion.Euler(0,rotationImpuls,0);
            Vector3 eulerRotation = new Vector3();
            eulerRotation= Vector3.ProjectOnPlane(targetRotation.eulerAngles, Vector3.forward);
            eulerRotation.x = 0f;
            //eulerRotation += rotationImpuls*Vector3.up;
//            Debug.Log(eulerRotation);
            targetRotation = Quaternion.Euler(eulerRotation);
            targetRotation *= Quaternion.Euler(0, rotationImpuls, 0);
            Body.transform.rotation = targetRotation;
            yield return new WaitForSeconds(SetAutoRotationImpulsDuration);
        }
    } 
    
    public void SetAdjustmentStatus(bool state)
    {
        _readjustBodyToCenter = state;
    }
    private void LateUpdate()
    {
        
        
        if (!_readjustBodyToCenter)
        {
            Head.transform.position = Body.transform.position;
        }

    }


    private void temporaryIKLocomotion( GameObject Head, GameObject Body)
    {
    }

    

    public void RotateLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
    }
    
    public void RotateRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
      
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

    public void AllowInput(bool state)
    {
        _allowInput = state;
    }
    
    
    public Vector2 GetCurrentInput()
    {
        if (_allowInput)
        {
            return movementInput;
        }
        else
        {
            return Vector2.zero;
        }
        
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
