using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LiveDataRecorder : MonoBehaviour
{

    private float _frameRate;
    private bool _isRecording;
    private List<LiveDataFrame> _dataFrames;
    
    
    private HybridCharacterController _characterController;
    private HybridRemoteTransformConroller _remoteController;
    private PuppetController _puppetController;
    private InputController _inputController;
    
    private Transform _hmd;
    private Transform _leftController;
    private Transform _rightController;
 


    private string HeldObjectInLeftHand;
    private string HeldObjectInRightHand;

    private Transform Character;
    private Vector3 _adjustedCharacterPosition;
    private bool _isInThirdPerson;
    [SerializeField] private Vector2 _movementInput;
    [SerializeField] private Vector3 _rotationInput;

    [SerializeField] public Vector3[] limbPositions;
    [SerializeField] public Quaternion[] limbRotations;


    private bool _hitSomething;
    
    private Transform Puppet;
    private Transform PuppetHead;

    public void Initialize(int framesPerSecond=90)
    {
        //assign Transforms
        if (SteamVR.active)
        {
            SetFrameRate(framesPerSecond);
            _hmd = Player.instance.hmdTransform;
            _leftController = Player.instance.leftHand.transform;
            _rightController = Player.instance.rightHand.transform;

            _characterController = ExperimentManager.Instance._playerCharacterController;
            Character = _characterController.transform;
            ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>().OnNotifyPerspectiveSwitchObservers+=
                PerspectiveWasSwitched;
            _remoteController = ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>()
                .GetRemoteTransformController();
            _inputController = ExperimentManager.Instance.SelectedAvatar.GetComponent<InputController>();

            _movementInput = new Vector2();
            _rotationInput = new Vector3();
            _inputController.OnNotifyControlStickMovedObservers += ReadInput;

            _inputController.OnNotifyRotationPerformed += RotationPerceived;
            
            Puppet = _remoteController.RemoteFeetPositionGuess;
            PuppetHead = _remoteController.RemoteHMD;

            _puppetController = ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>()
                .GetPuppetController();

            
        }
        
    }

    public void StartRecording()
    {
        if (_isRecording)
        {
            return;
        }
        else
        {
            _isRecording = true;
            _dataFrames = new List<LiveDataFrame>();
            StartCoroutine(Recording());
        }
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    public void ClearData()
    {
        _dataFrames.Clear();
    }

    public void SaveData()
    {
        if (_isRecording)
        {
            StopRecording();
        }
        DataSavingManager.Instance.SaveList(_dataFrames, ExperimentManager.Instance.GetParticipantID()+ "_" + ExperimentManager.Instance.GetCondition()+"_"+ExperimentManager.Instance.StationIndex+ "_lv ");
    }

    public void SetFrameRate(int frames)
    {
        _frameRate = 1f / frames;
    }

    public void PerspectiveWasSwitched(bool state)
    {
        _isInThirdPerson = state;
    }

    private void RotationPerceived(Quaternion rotation)
    {
        _rotationInput = rotation.eulerAngles;
    }

    private void ReadInput(Vector2 movementInput)
    {
        _movementInput = movementInput;
    }

    private IEnumerator Recording()
    {
        while (_isRecording)
        {
            LiveDataFrame dataFrame = new LiveDataFrame();
            dataFrame.TimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
            //HMD
            dataFrame.HMDPositionGlobal = _hmd.position;
            dataFrame.HMDPositionLocal = _hmd.localPosition;
            dataFrame.HMDRotationGlobal = _hmd.rotation;
            dataFrame.HMDRotationLocal = _hmd.localRotation;
            dataFrame.NoseVector = _hmd.forward;
            
            //Controller
            dataFrame.LeftHandGlobalPosition = _leftController.position;
            dataFrame.LeftHandLocalPositon = _leftController.localPosition;
            dataFrame.LeftHandGlobalRotation = _leftController.rotation;
            dataFrame.LeftHandLocalRotation = _leftController.localRotation;

            dataFrame.RightGlobalPosition = _rightController.position;
            dataFrame.RightHandLocalPositon = _rightController.localPosition;
            dataFrame.RightGlobalRotation = _rightController.rotation;
            dataFrame.RightHandLocalRotation = _rightController.localRotation;

            dataFrame.MovementInput = _movementInput;
            dataFrame.RotationInput = _rotationInput;
            
            //Character
            dataFrame.CharacterControllerPosition = _characterController.GetAdjustedPosition();
            dataFrame.CharacterControllerRotation = _characterController.transform.rotation;
            dataFrame.isThirdPerson = _isInThirdPerson;
            dataFrame.AdjustedCharacterPosition = _characterController.GetAdjustedPosition();
            
            //Puppet
            dataFrame.PuppetPosition = _remoteController.RemoteFeetPositionGuess.position;
            dataFrame.PuppetRotation = _remoteController.RemoteFeetPositionGuess.rotation;

            dataFrame.PuppetPuppetHeadPositon = _remoteController.RemoteHMD.transform.position;
            dataFrame.PuppetPuppetHeadRotation = _remoteController.RemoteHMD.transform.rotation;

            dataFrame.LimbPositions = _puppetController.GetLimbPositions();
            dataFrame.LimbRotations = _puppetController.GetLimbRotations();
            
            _dataFrames.Add(dataFrame);
            if (Vector3.Magnitude(_rotationInput) > 0f)
            {
                _rotationInput=Vector3.zero;
            }
            
            
            //Eyetracking

            RaycastHit hit;

            if(Physics.Raycast(_hmd.transform.position, _hmd.transform.forward, out hit,30f))
            {
                
            };
            
           
            
            
            yield return new WaitForSeconds(_frameRate);
        }
    }
}
