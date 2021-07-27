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

    private bool _isAdjustingToCamera;
    


    private InputController _inputController;
    private HybridCharacterController _characterController;
    private HybridCameraController _cameraController;
    private HybridRemoteTransformConroller _remoteTransformConroller;


    [SerializeField] private bool startWithThirdPerson;
    
    [Header("Position Readjustment")]
    [Range(0f, 1f)] public float DistanceThresholdForAdjusting = 0.4f;
    [Range(0f, 1f)]public float timeUntilRegainControl = 0.5f;
    private float currentPuppetToPlayerOffset;
    private bool _isInsideDistanceThreshold;
    
    
    
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
        _currentRemoteFeetGuess = _remoteTransformConroller.GetRemoteFeetPositionGuess();
        if (isInsideDistanceThreshold())
        {
            
         
            _isInsideDistanceThreshold=true;
        }
        else
        {
           
            _isInsideDistanceThreshold = false;
            ReadjustPlayer();
        }

        
        if (_isAdjustingToCamera)
        {
            
        }
        else
        {
           
        } 
           
    }
    
    
    private bool isInsideDistanceThreshold()
    {
        
        float distance = Vector3.Distance(_remoteTransformConroller.RemoteFeetPositionGuess.transform.position,_currentCharacterFeetPosition);
        
        Debug.Log(distance);
        
        if (distance < DistanceThresholdForAdjusting)
        {
            return true;
        }
        else
        {
            return false;
        }
       
        

    }

    private void MoveAvatar(Vector2 input)
    {
        if (_isAdjustingToCamera) return;
        
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
        if (_isAdjustingToCamera) return;
        
        _isAdjustingToCamera = true;
        
        Debug.Log("readjustment needed");
        
        StartCoroutine(CharacterAdjustment());
        
        
       
    }
    
    
    private IEnumerator CharacterAdjustment()
    {
        Debug.Log("waiting until again in threshold");
        
        _characterController.SetCharacterPosition(_currentRemoteFeetGuess);
        _cameraController.SetPosition(_currentRemoteFeetGuess);
        _remoteTransformConroller.SetPosition(_currentRemoteFeetGuess);
        _remoteTransformConroller.ReadjustingState(true);
        
        yield return new WaitUntil(() => _isInsideDistanceThreshold);
        _remoteTransformConroller.ReadjustingState(false);
        Debug.Log("is In Threshold");
        yield return new WaitForSeconds(timeUntilRegainControl);
        Debug.Log("Regain Control");
        
        
        //cameraController.AddOffset(remoteVR.RemoteFootPositon.transform.localPosition);
        //yield return new WaitForSeconds(2f);
        
        _isAdjustingToCamera = false;
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
