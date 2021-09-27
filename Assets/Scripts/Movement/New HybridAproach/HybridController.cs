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
    
    private bool _fadingInProgress;
    

    private InputController _inputController;
    private HybridCharacterController _characterController;
    private HybridCameraController _cameraController;
    private HybridRemoteTransformConroller _remoteTransformConroller;
    private PuppetController _puppetController;


   
    [Header("General Settings Settings")]
    [SerializeField] private bool EmbodiedCondition;
    [SerializeField] private bool startWithThirdPerson;
    [SerializeField]private bool AllowMovementDuringFirstperson;
    [SerializeField]private bool AllowRotationDuringFirstperson;
    [SerializeField]private bool AllowSwitchingViews;
    
    [Header("Camera Settings")]
    [Range(0f,10f)] [SerializeField] private float CameraDistance;
    
    [Header("Rotation Settings")]
    [SerializeField] private bool rotationIsBasedOnAdjustedCharacterPosition;
    public bool FadingDuringRotation;
    [Range(0f,1f)] public float FadeOutDuration;
    [Range(0f,1f)] public float FadeDuration;
    [Range(0f,1f)] public float FadeInDuration;
    
    [SerializeField] private bool changeRotationToHeadRotationAfterPerspectiveSwitch;



    [Header("Perspective Switch Settings")] 
    private bool _switchingViewIsCurrentlyAllowed;
    
    
    [Header("Position Readjustment")]
    private float currentPuppetToPlayerOffset;
    private bool _isInsideDistanceThreshold;
    
    [Header("Tutorials")] 
    private ControllerRepresentations _controllerRepresentations;
    [SerializeField] private bool ShowControllerHelp;

    private bool _inputIsAllowed;

    private BlobController _blobController;
    
    
    public delegate void OnPerspectiveSwitchPerformed(bool state);
    public event OnPerspectiveSwitchPerformed OnNotifyPerspectiveSwitchObservers;
    
    
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

        _cameraController.OnNotifyFadedCompletedObervers += FadingCompleted;
        
        
        //_cameraController.SetCameraDistance(CameraDistance);

        _characterController.OnNotifyImpactObservers += ApplyOuterImpact;

        _currentRotation = transform.rotation;
        
        _currentlyInThirdPerson = startWithThirdPerson;
        

        if (_puppetController.transform.GetComponent<BlobController>()!=null)
        {
            _blobController = _puppetController.transform.GetComponent<BlobController>();
        }
        
        _puppetController.IsEmobdiedCondition(EmbodiedCondition);
        
        _characterController.SetOrientationBasedOnCharacter(rotationIsBasedOnAdjustedCharacterPosition);
        SwitchView(startWithThirdPerson);
        
        if (!EmbodiedCondition&& _blobController!=null)
        {
            _blobController.DeactivateHandsOnStartWorkaround(startWithThirdPerson);
        }
        
        
        
    }

    public HybridCharacterController GetHybridChracterController()
    {
        return _characterController;
    }

    private void Update()
    {
        _remoteTransformConroller.SetPosition(_characterController.GetGeneralCharacterPosition());
        _cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());
        
        
    }


    private void FixedUpdate()
    {
        _currentCharacterSpeed = _characterController.GetCurrentSpeed();
        _puppetController.SetCurrentSpeed(_currentCharacterSpeed);
    }

    private void LateUpdate()
    {
        _currentRemoteFeetGuess = _remoteTransformConroller.GetLocalRemoteFeetPositionGuess();

        _currentRemoteForwardGuess = _remoteTransformConroller.GetRemoteFowardGuess();

        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();

        
        
        

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


    public void ApplyOuterImpact(Vector3 impactDirection, float velocity)
    {
        _characterController.ApplyOuterImpact(impactDirection,velocity);
        _puppetController.SetForcedAnimationForSeconds(0.25f);
        _cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());
        _remoteTransformConroller.SetPosition(_characterController.GetGeneralCharacterPosition());
        _puppetController.SetPosition(_characterController.GetAdjustedPosition());
        
    }
    private void MoveAvatar(Vector2 input)
    {
        
       
        //_cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());

        

       

        Vector3 MovementDirection = new Vector3();
        if (_inputIsAllowed&& !(!AllowMovementDuringFirstperson&& !_currentlyInThirdPerson))
        { 
            MovementDirection = new Vector3(input.x, 0f, input.y);
        }
        
        
           
        _characterController.MoveCharacter(MovementDirection);
        _puppetController.SetCurrentSpeed(_currentCharacterSpeed);
        _puppetController.MovePuppet(MovementDirection);
        
        
        _cameraController.SetPosition(_characterController.GetGeneralCharacterPosition());
        _remoteTransformConroller.SetPosition(_characterController.GetGeneralCharacterPosition());
        _characterController.SetAdjustmentPosition(_currentRemoteFeetGuess);
        _puppetController.SetPosition(_characterController.GetAdjustedPosition());
        _currentGeneralCharacterPosition = _characterController.GetGeneralCharacterPosition();

    }
    
    public void AllowInput(bool state)
    {
        _inputIsAllowed = state;
    }
    public void AllowViewSwitch(bool state)
    {
        _switchingViewIsCurrentlyAllowed = state;
    }

    private void SwitchView()
    {
        if (!_switchingViewIsCurrentlyAllowed)
            return;

        if (!AllowSwitchingViews)
            return;

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
        
        OnNotifyPerspectiveSwitchObservers?.Invoke(_currentlyInThirdPerson);
        
    }
    
    //Tutorial Related
    public void ShowControllers(bool state)
    {
        _controllerRepresentations.ShowController(state);
    }
    
    
    public void HighLightControlSwitchButton(bool state)
    {
        _controllerRepresentations.HighLightPerspectiveChangeButtons(state);
    }
    
    public void HighLightMovementButton(bool state)
    {
        _controllerRepresentations.HighlightMovementStick(state);
    }

    public void HighLightRotationButton(bool state)
    {
        _controllerRepresentations.HighlightRotationStick(state);
    }
    
    public void HighLightGraspButtons(bool state)
    {
        _controllerRepresentations.HighLightGraspButtons(state);
    }


    
    //Perspective and Fading
    
    private void SwitchView(bool ToThirdPerson)
    {
        if (!AllowSwitchingViews)
            return;
        
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

    public bool IsCurrentlyInThirdperson()
    {
        return _currentlyInThirdPerson;
    }
    public bool IsFadingInProgress()
    {
        return _fadingInProgress;
    }
    public void FadingCompleted()
    {
        _fadingInProgress=false;
    }
    public void Fading(float FadeOutDuration,float FadeInDuration, float FadeDuration)
    {
        _fadingInProgress = true;
        _cameraController.Fading(FadeOutDuration,FadeInDuration,FadeDuration);
    }


    public HybridRemoteTransformConroller GetRemoteTransformController()
    {
        return _remoteTransformConroller;
    }
    
    
    
    private void RotateAvatar(Quaternion rotation)
    {
        
        if (!_currentlyInThirdPerson)
        {
            if (!AllowRotationDuringFirstperson)
            {
                return;
            }
        }

        _currentRotation *= rotation;
        _cameraController.SetPosition(_currentGeneralCharacterPosition);
       SetRotation(_currentRotation);
    }

    private void SetRotation(Quaternion rotation,bool isTeleport=false)
    {
       
            _cameraController.RotateCamera(rotation);
            _characterController.RotateCharacter(rotation);
            _puppetController.RotateAvatar(rotation);
            _remoteTransformConroller.RotateRemoteTransforms(rotation);
    }


    private void SetPosition(Vector3 position)
    {
        _characterController.transform.position = position;
        _remoteTransformConroller.transform.position = position;
        _cameraController.transform.position = position;
        _puppetController.transform.position = position;
    }


    public bool IsSwitchingViewAllowed()
    {
        return AllowSwitchingViews;
    }
    
    public void TeleportToPosition(Transform TeleportTransform)
    {
        StartCoroutine(TeleportProcess(TeleportTransform));
    }

    private IEnumerator TeleportProcess(Transform TeleportTransform)
    {
        //ugliest Teleport ever
        _characterController.GetComponent<CharacterController>().enabled = false;
        _characterController.transform.localPosition = Vector3.zero;
        _remoteTransformConroller.transform.localPosition = Vector3.zero;
        _puppetController.transform.localPosition = Vector3.zero;
        _cameraController.transform.localPosition = Vector3.zero;
        this.transform.position = TeleportTransform.position;
        _currentRotation = TeleportTransform.rotation;

        SetRotation(_currentRotation, true);
        //_cameraController.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1);
        
        _characterController.GetComponent<CharacterController>().enabled = true;
    }


    private Quaternion GetCurrentRotation()
    {
        return _currentRotation;
    }
    
}
