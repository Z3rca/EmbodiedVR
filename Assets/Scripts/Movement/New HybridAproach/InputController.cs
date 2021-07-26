using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class InputController : MonoBehaviour
{
    public SteamVR_Action_Vector2 MovementInput;
    public SteamVR_Action_Vector2 RotationInput;
    public SteamVR_Action_Boolean rotateLeft;
    public SteamVR_Action_Boolean rotateRight;
    public SteamVR_Action_Boolean switchPerspective;
    public SteamVR_ActionSet actionSetEnable;
    

    public bool SnapTurn;

    [Range(0.1f, 45)] public float SetRotationImpuls;
    [Range(0.0f, 1)] public float SetAutoRotationImpulsDuration = 0.5f;

    private Vector2 movementInput;
    private Quaternion targetRotation;
    private bool rotationApplied;
    private float rotationImpuls;
    private bool _allowRotation=true;
    public delegate void OnButtonPressed();
    public event OnButtonPressed OnNotifyLeftTurnButtonPressedObserver;
    public event OnButtonPressed OnNotifyRightTurnButtonPressedObserver;
    public event OnButtonPressed OnNotifySwitchButtonPressedObserver;

    public delegate void OnMovementInputReceived(Vector2 movementInput);
    public event OnMovementInputReceived OnNotifyControlStickMovedObservers;
    
    public delegate void OnRotationPerformed(Quaternion rotation);
    public event OnRotationPerformed OnNotifyRotationPerformed;
    
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
        OnNotifyControlStickMovedObservers?.Invoke(movementInput);

        if (rotateLeft.state || rotateRight.state)
        {
            if (!SnapTurn)
            {
                rotationImpuls *= Time.deltaTime;
            }
            if (rotateLeft.state)
            {
                rotationImpuls = -SetRotationImpuls;
                OnNotifyLeftTurnButtonPressedObserver?.Invoke();
            }
            if (rotateRight.state)
            {
                rotationImpuls = SetRotationImpuls;
                OnNotifyRightTurnButtonPressedObserver?.Invoke();
            }
            if (!rotating)
            {
                rotating = true;
                StartCoroutine(PerformRotationImpuls());
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (rotateLeft.state || rotateRight.state)
        {
            if (rotateLeft.state)
            {
                rotationImpuls = -SetRotationImpuls;
                OnNotifyLeftTurnButtonPressedObserver?.Invoke();
            }

            if (rotateRight.state)
            {
                rotationImpuls = SetRotationImpuls;
                OnNotifyRightTurnButtonPressedObserver?.Invoke();
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
        }else
        {
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
            Vector3 eulerRotation = new Vector3();
            eulerRotation= Vector3.ProjectOnPlane(targetRotation.eulerAngles, Vector3.forward);
            eulerRotation.x = 0f;
            targetRotation = Quaternion.Euler(eulerRotation);
            targetRotation *= Quaternion.Euler(0, rotationImpuls, 0);
            OnNotifyRotationPerformed?.Invoke(targetRotation);
            yield return new WaitForSeconds(SetAutoRotationImpulsDuration);
        }
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
        OnNotifySwitchButtonPressedObserver?.Invoke();
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
    
}
