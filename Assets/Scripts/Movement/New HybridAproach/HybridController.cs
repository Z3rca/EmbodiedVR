using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridController : MonoBehaviour
{
    private bool _currentlyInThirdPerson;
    private Quaternion _currentRotation;
    private Vector3 _currentCharacterFeetPosition;
    private Vector3 _currentGeneralCharacterPosition;
    private Vector3 _currentRemoteFeetGuess;
    private Quaternion _currentRemoteForwardGuess;
    private float _currentCharacterSpeed;

    private bool _isAdjustingToCamera;
    


    private InputController _inputController;
    private HybridCharacterController _characterController;
    private HybridCameraController _cameraController;
    private HybridRemoteTransformConroller _remoteTransformConroller;
    private PuppetController _puppetController;


    [SerializeField] private bool startWithThirdPerson;
    
    
    
    [Header("Rotation Settings")]
    [SerializeField] private bool rotationIsBasedOnAdjustedCharacterPosition;
    public bool FadingDuringRotation;
    [Range(0f,1f)] public float FadeOutDuration;
    [Range(0f,1f)] public float FadeDuration;
    [Range(0f,1f)] public float FadeInDuration;
    [SerializeField]private bool AllowRotationDuringFirstperson;
    [SerializeField] private bool changeRotationToHeadRotationAfterPerspectiveSwitch;
    
    
    
    
    
    [Header("Position Readjustment")]
    private float currentPuppetToPlayerOffset;
    private bool _isInsideDistanceThreshold;
    
    [Header("Tutorials")] 
    private ControllerRepresentations _controllerRepresentations;
    [SerializeField] private bool ShowControllerHelp;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _remoteTransformConroller = GetComponentInChildren<HybridRemoteTransformConroller>();
        _characterController = GetComponentInChildren<HybridCharacterController>();
        _cameraController = GetComponentInChildren<HybridCameraController>();
        _inputController = GetComponent<InputController>();
        _puppetController = GetComponentInChildren<PuppetController>();
        _controllerRepresentations = GetComponent<ControllerRepresentations>();
        
        _inputController.OnNotifyControlStickMovedObservers += MoveAvatar;
        _inputController.OnNotifySwitchButtonPressedObserver += SwitchView;
        _inputController.OnNotifyRotationPerformed += RotateAvatar;

        _currentRotation = transform.rotation;
        
        _currentlyInThirdPerson = startWithThirdPerson;
        SwitchView(startWithThirdPerson);
        
        _characterController.SetOrientationBasedOnCharacter(rotationIsBasedOnAdjustedCharacterPosition);

        if (ShowControllerHelp)
        {
            _controllerRepresentations.ShowController(true);
        }
        
        
    }


    private void LateUpdate()
    {
        _currentRemoteFeetGuess = _remoteTransformConroller.GetLocalRemoteFeetPositionGuess();

        _currentRemoteForwardGuess = _remoteTransformConroller.GetRemoteFowardGuess();

        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();

        _currentCharacterSpeed = _characterController.GetCurrentSpeed();
        
        _puppetController.SetCurrentSpeed(_currentCharacterSpeed);

        if (ShowControllerHelp)
        {
            UpdateControllerTransforms();
        }
        
    }
    
    void UpdateControllerTransforms()
    {
        _controllerRepresentations.LeftController.transform.localPosition =
            _remoteTransformConroller.LocalLeft.localPosition;
        _controllerRepresentations.RightController.transform.localPosition =
            _remoteTransformConroller.LocalRight.localPosition;

        _controllerRepresentations.LeftController.transform.localRotation =
            _remoteTransformConroller.LocalLeft.localRotation;
        _controllerRepresentations.RightController.transform.localRotation =
            _remoteTransformConroller.LocalRight.localRotation;
    }
   
    

    private void MoveAvatar(Vector2 input)
    {
        
       
        _cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());
        if (_isAdjustingToCamera) return;
        
        if (input != Vector2.zero)
        {
            Vector3 MovementDirection = new Vector3(input.x, 0f, input.y);
            _characterController.MoveCharacter(MovementDirection);
            _puppetController.SetCurrentSpeed(_currentCharacterSpeed);
            _puppetController.MovePuppet(MovementDirection);
            _cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());
            _remoteTransformConroller.SetPosition(_characterController.GetGeneralCharacterPosition());
        }
        
        _characterController.SetAdjustmentPosition(_currentRemoteFeetGuess);
        _puppetController.SetPosition(_characterController.GetAdjustedPosition());
        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();

    }

    private void SwitchView()
    {

        if (_currentlyInThirdPerson)
        {
            _cameraController.SwitchPerspective(false);
           
        }
        else
        {
            if (changeRotationToHeadRotationAfterPerspectiveSwitch)
            {
                var rot = _currentRemoteForwardGuess;
                _cameraController.SetPosition(_currentGeneralCharacterPosition);
                SetRotation(rot);
            }
            
            _cameraController.SwitchPerspective(true);
            
        }
        
        _currentlyInThirdPerson = !_currentlyInThirdPerson;
        
    }
    
    //Tutorial Related
    public void ShowControllers(bool state)
    {
        _controllerRepresentations.ShowController(state);
    }
    
    
    public void HighLightControlSwitchButton(bool state)
    {
        _controllerRepresentations.HighLightPerspectiveChangeButton(state);
    }

    
    
    
    private void SwitchView(bool ToThirdPerson)
    {
        if (!ToThirdPerson)
        {
            _cameraController.SwitchPerspective(false);
            _currentlyInThirdPerson = ToThirdPerson;
        }
        else
        {
            if (changeRotationToHeadRotationAfterPerspectiveSwitch)
            {
                var rot = _currentRemoteForwardGuess;
                _cameraController.SetPosition(_currentGeneralCharacterPosition);
                SetRotation(rot);
            }
            
            _cameraController.SwitchPerspective(true);
            _currentlyInThirdPerson = ToThirdPerson;
        }
    }
    
    
    
    
    
    private void RotateAvatar(Quaternion rotation)
    {
        Debug.Log(AllowRotationDuringFirstperson && !_currentlyInThirdPerson);
        if (AllowRotationDuringFirstperson && !_currentlyInThirdPerson)
            return;
        
        _currentRotation *= rotation;
        _cameraController.SetPosition(_currentGeneralCharacterPosition);
       SetRotation(_currentRotation);
    }

    private void SetRotation(Quaternion rotation)
    {
        _cameraController.RotateCamera(rotation);
        _characterController.RotateCharacter(rotation);
        _puppetController.RotateAvatar(rotation);
        _remoteTransformConroller.RotateRemoteTransforms(rotation);

    }


    private Quaternion GetCurrentRotation()
    {
        return _currentRotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
