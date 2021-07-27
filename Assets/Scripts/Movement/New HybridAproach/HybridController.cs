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

    private InputController _inputController;
    private HybridCharacterController _characterController;

    private HybridCameraController _cameraController;

    private HybridRemoteTransformConroller _remoteTransformConroller;


    [SerializeField] private bool startWithThirdPerson;
    // Start is called before the first frame update
    void Start()
    {
        _remoteTransformConroller = GetComponentInChildren<HybridRemoteTransformConroller>();
        _characterController = GetComponentInChildren<HybridCharacterController>();
        _cameraController = GetComponentInChildren<HybridCameraController>();
        _inputController = GetComponent<InputController>();
        Debug.Log(_inputController);

        _inputController.OnNotifyControlStickMovedObservers += MoveAvatar;
        _inputController.OnNotifySwitchButtonPressedObserver += SwitchView;
        _inputController.OnNotifyRotationPerformed += RotateAvatar;

        _currentRotation = transform.rotation;

        _currentlyInThirdPerson = startWithThirdPerson;
    }


    private void LateUpdate()
    {
        
    }
    
    
    private void isInsideThreshold()
    {
        
        //float distance = Vector3.Distance(_remoteTransformConroller.RemoteFeetPositionGuess.transform.position,_characterController);
        //Debug.Log(distance);
        
        //_isInThreshold= distance<ReadjustmentThreshold;

    }

    private void MoveAvatar(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 MovementDirection = new Vector3(input.x, 0f, input.y);
            _characterController.MoveCharacter(MovementDirection);
        }
        
        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();
        _currentCharacterFeetPosition= _characterController.GetCharacterFeetPosition();
        
        _remoteTransformConroller.SetPosition(_currentCharacterFeetPosition);
            
        _cameraController.SetPosition(_currentCharacterFeetPosition);
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

    private void ReadjustPlayer()
    {
        
    }

    private void RotateAvatar(Quaternion rotation)
    {
        _currentRotation *= rotation;
        _cameraController.SetPosition(_currentCharacterFeetPosition);
        _cameraController.RotateCamera(_currentRotation);
        _characterController.RotateCharacter(_currentRotation);
        _remoteTransformConroller.RotateRemoteTransforms(_currentRotation);


        Debug.Log( rotation);
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
