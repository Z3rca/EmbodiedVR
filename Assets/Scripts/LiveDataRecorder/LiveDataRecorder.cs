using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

public class LiveDataRecorder : MonoBehaviour
{

    private float _frameRate;
    private bool _isRecording;
    private List<LiveDataFrame> _dataFrames;
    
    
    private HybridCharacterController _characterController;
    private HybridRemoteTransformConroller _remoteController;
    private PuppetController _puppetController;
    private InputController _inputController;
    private ControllerRepresentations _controllerRepresentations;
    
    private Transform _hmd;
    private Hand _leftHand;
    private Hand _rightHand;
 


    private string HeldObjectInLeftHand;
    private string HeldObjectInRightHand;

    private Transform Character;
    private Vector3 _adjustedCharacterPosition;
    private bool _isInThirdPerson;
    private Vector2 _movementInput;
    private Vector3 _rotationInput;

    private Vector3[] limbPositions;
    private Quaternion[] limbRotations;


    private bool _hitSomething;
    private bool _recordEyetData;
    
    private Transform Puppet;
    private Transform PuppetHead;

    public void Initialize(int framesPerSecond=90)
    {
        //assign Transforms
        if (SteamVR.active)
        {
            SetFrameRate(framesPerSecond);
            _hmd = Player.instance.hmdTransform;
            _leftHand = Player.instance.leftHand;
            _rightHand = Player.instance.rightHand;

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

            _controllerRepresentations = ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>()
                .GetControllerRepresentations();
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
        DataSavingManager.Instance.SaveList(_dataFrames, ExperimentManager.Instance.GetParticipantID()+ "_" + ExperimentManager.Instance.GetCondition()+"_S"+ExperimentManager.Instance.GetStationID()+"_I"+ExperimentManager.Instance.StationIndex+"_lv");
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
            dataFrame.LeftHandGlobalPosition = _leftHand.transform.position;
            dataFrame.LeftHandLocalPositon = _leftHand.transform.localPosition;
            dataFrame.LeftHandGlobalRotation = _leftHand.transform.rotation;
            dataFrame.LeftHandLocalRotation = _leftHand.transform.localRotation;

            dataFrame.RightGlobalPosition = _rightHand.transform.position;
            dataFrame.RightHandLocalPositon = _rightHand.transform.localPosition;
            dataFrame.RightGlobalRotation = _rightHand.transform.rotation;
            dataFrame.RightHandLocalRotation = _rightHand.transform.localRotation;

            dataFrame.ControllersAreShown = _controllerRepresentations.ControllerShown();
          
            
            //Input
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
              
            //Objects 
            if (_leftHand.currentAttachedObject != null)
            {
                dataFrame.ObjectNameLeft = _leftHand.currentAttachedObject.name;
                dataFrame.ObjectAttachedToLeftHand = true;
            }

            if (_rightHand.currentAttachedObject != null)
            {
                dataFrame.ObjectNameRight = _rightHand.currentAttachedObject.name;
                dataFrame.ObjectAttachedToRightHand = true;
            }
            
            //Eyetracking

            VerboseData eyeData;
            if(SRanipal_Eye_v2.GetVerboseData(out eyeData))
            {
                //ValidityCheck;
                dataFrame.combinedGazeValidityBitmask = eyeData.combined.eye_data.eye_data_validata_bit_mask;
                dataFrame.leftGazeValidityBitmask = eyeData.left.eye_data_validata_bit_mask;
                dataFrame.rightGazeValidityBitmask = eyeData.right.eye_data_validata_bit_mask;
                
                //pupil diameter
                dataFrame.pupilDiameterMillimetersLeft = eyeData.left.pupil_diameter_mm;
                dataFrame.pupilDiameterMillimetersRight = eyeData.right.pupil_diameter_mm;
                
                //eye openness
                dataFrame.eyeOpennessLeft = eyeData.left.eye_openness;
                dataFrame.eyeOpennessRight = eyeData.right.eye_openness;
                
                Vector3 coordinateAdaptedGazeDirectionLeft = new Vector3(eyeData.left.gaze_direction_normalized.x * -1,  eyeData.left.gaze_direction_normalized.y, eyeData.left.gaze_direction_normalized.z);
                Vector3 coordinateAdaptedGazeDirectionRight = new Vector3(eyeData.right.gaze_direction_normalized.x * -1,  eyeData.right.gaze_direction_normalized.y, eyeData.right.gaze_direction_normalized.z);
                Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(eyeData.combined.eye_data.gaze_direction_normalized.x * -1,  eyeData.combined.eye_data.gaze_direction_normalized.y, eyeData.combined.eye_data.gaze_direction_normalized.z);
                
                
                //local direction 

                dataFrame.eyeDirectionLeftLocal = coordinateAdaptedGazeDirectionLeft;
                dataFrame.eyeDirectionRightLocal = coordinateAdaptedGazeDirectionRight;
                dataFrame.eyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;
                
                //local position
                
                dataFrame.eyePositionLeftLocal= eyeData.left.gaze_origin_mm;
                dataFrame.eyePositionRightLocal = eyeData.right.gaze_origin_mm;
                dataFrame.eyePositionCombinedLocal = eyeData.combined.eye_data.gaze_origin_mm;
                
                //global direction
                
                dataFrame.eyeDirectionLeftWorld = _hmd.rotation * coordinateAdaptedGazeDirectionLeft;
                dataFrame.eyeDirectionRightWorld = _hmd.rotation * coordinateAdaptedGazeDirectionRight;
                dataFrame.eyeDirectionCombinedWorld = _hmd.rotation * coordinateAdaptedGazeDirectionCombined;
                
                //global position

                dataFrame.eyePositionLeftWorld = eyeData.left.gaze_origin_mm / 1000 + _hmd.position;
                dataFrame.eyePositionRightWorld = eyeData.right.gaze_origin_mm / 1000 + _hmd.position;
                dataFrame.eyePositionCombinedWorld = eyeData.combined.eye_data.gaze_origin_mm / 1000 + _hmd.position;


                RaycastHit[] hits;
                
                hits = Physics.RaycastAll( dataFrame.eyePositionCombinedWorld, dataFrame.eyeDirectionCombinedWorld, 30f);
                if (hits.Length > 0)
                {
                    dataFrame.HitSomething = true;
                    if (hits.Length == 1)
                    {
                        dataFrame.HitObject1 = hits[0].collider.name;
                        dataFrame.HitPosition1 = hits[0].point;
                        dataFrame.HitObjectPosition1 = hits[0].collider.gameObject.transform.position;
                    }

                    if (hits.Length >= 2)
                    {
                        hits.OrderBy(x=>x.distance).ToArray();
                        dataFrame.HitObject1 = hits[0].collider.name;
                        dataFrame.HitPosition1 = hits[0].point;
                        dataFrame.HitObjectPosition1 = hits[0].collider.gameObject.transform.position;
                    
                        dataFrame.HitObject1 = hits[1].collider.name;
                        dataFrame.HitPosition1 = hits[1].point;
                        dataFrame.HitObjectPosition1 = hits[1].collider.gameObject.transform.position;
                    }
                }
                
            }
            else
            {
                RaycastHit[] hits;
            
                hits = Physics.RaycastAll(_hmd.transform.position, _hmd.transform.forward, 30f);
                if (hits.Length > 0)
                {

                    dataFrame.HitSomething = true;
                    if (hits.Length == 1)
                    {
                        dataFrame.HitObject1 = hits[0].collider.name;
                        dataFrame.HitPosition1 = hits[0].point;
                        dataFrame.HitObjectPosition1 = hits[0].collider.gameObject.transform.position;
                    }

                    if (hits.Length >= 2)
                    {
                        hits.OrderBy(x=>x.distance).ToArray();
                        dataFrame.HitObject1 = hits[0].collider.name;
                        dataFrame.HitPosition1 = hits[0].point;
                        dataFrame.HitObjectPosition1 = hits[0].collider.gameObject.transform.position;
                    
                        dataFrame.HitObject1 = hits[1].collider.name;
                        dataFrame.HitPosition1 = hits[1].point;
                        dataFrame.HitObjectPosition1 = hits[1].collider.gameObject.transform.position;
                    }
                }
            }
            
            
            
            _dataFrames.Add(dataFrame);
            if (Vector3.Magnitude(_rotationInput) > 0f)
            {
                _rotationInput=Vector3.zero;
            }
 
            yield return new WaitForSeconds(_frameRate);
        }
    }
}
