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
    private float _currentCharacterSpeed;

    private bool _isAdjustingToCamera;
    


    private InputController _inputController;
    private HybridCharacterController _characterController;
    private HybridCameraController _cameraController;
    private HybridRemoteTransformConroller _remoteTransformConroller;
    private PuppetController _puppetController;


    [SerializeField] private bool startWithThirdPerson;
    [SerializeField] private bool rotationIsBasedOnAdjustedCharacterPosition;
    
    [Header("Position Readjustment")]
    private float currentPuppetToPlayerOffset;
    private bool _isInsideDistanceThreshold;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _remoteTransformConroller = GetComponentInChildren<HybridRemoteTransformConroller>();
        _characterController = GetComponentInChildren<HybridCharacterController>();
        _cameraController = GetComponentInChildren<HybridCameraController>();
        _inputController = GetComponent<InputController>();
        _puppetController = GetComponentInChildren<PuppetController>();

        _inputController.OnNotifyControlStickMovedObservers += MoveAvatar;
        _inputController.OnNotifySwitchButtonPressedObserver += SwitchView;
        _inputController.OnNotifyRotationPerformed += RotateAvatar;

        _currentRotation = transform.rotation;

        _currentlyInThirdPerson = startWithThirdPerson;

        
        _characterController.SetOrientationBasedOnCharacter(rotationIsBasedOnAdjustedCharacterPosition);
    }


    private void LateUpdate()
    {
        _currentRemoteFeetGuess = _remoteTransformConroller.GetLocalRemoteFeetPositionGuess();

        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();

        _currentCharacterSpeed = _characterController.GetCurrentSpeed();
        
        _puppetController.SetCurrentSpeed(_currentCharacterSpeed);
        
       

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
            _cameraController.SwitchPerspective(true);
        }
        
        _currentlyInThirdPerson = !_currentlyInThirdPerson;
        
    }
    
    
    
    

    private void RotateAvatar(Quaternion rotation)
    {
        _currentRotation *= rotation;
        _cameraController.SetPosition(_currentGeneralCharacterPosition);
        _cameraController.RotateCamera(_currentRotation);
        _characterController.RotateCharacter(_currentRotation);
        _puppetController.RotateAvatar(_currentRotation);
        _remoteTransformConroller.RotateRemoteTransforms(_currentRotation);
        
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
