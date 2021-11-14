using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyetrackingValidation : MonoBehaviour
{
    public static EyetrackingValidation Instance { get; private set; }
    
    #region Fields

    [Space] [Header("Eye-tracker validation field")]
    private GameObject _camera;
    [SerializeField] private GameObject fixationPoint;
    [SerializeField] private GameObject InstructionText;
    private float TimeShowingInstructionText = 2f;
    [SerializeField] private List<Vector3> keyPositions;

    private bool _isValidationRunning;
    private bool _isErrorCheckRunning;

    private int _calibrationFreq;
    
    private Coroutine _runValidationCo;
    private Coroutine _runErrorCheckCo;
    private Transform _hmdTransform;
    private List<EyeValidationData> _eyeValidationDataFrames;
    private EyeValidationData _eyeValidationData;
    private float ErrorThreshold = 1.5f;

    public EventHandler<EyeValidationArgs> OnValidationCompleted;

    #endregion

    #region Private methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
//        fixationPoint.SetActive(false);
        _eyeValidationDataFrames = new List<EyeValidationData>();
    }

    private void StoreValidationError()
    {
        
    }

    public void SetHMDTransform(Camera camera)
    {
        _hmdTransform = camera.transform;
    }
    
    private Vector3 GetValidationError()
    {
        return _eyeValidationData.EyeValidationError;
    }

    public void StartValidateEyetracker()
    {
        _runValidationCo = StartCoroutine(ValidateEyeTracker());
        
    }

    public void StopValidation()
    {
        StopCoroutine(_runValidationCo);
        fixationPoint.SetActive(false);
    }

    private IEnumerator ValidateEyeTracker(float delay=2)
    {
        if (_isValidationRunning) yield break;
        _isValidationRunning = true;
        

        fixationPoint.transform.parent = _hmdTransform.gameObject.transform;
        InstructionText.SetActive(false);
        //_hmdTransform = EyetrackingManager.Instance.GetHmdTransform();

        fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,30);

        fixationPoint.transform.LookAt(_hmdTransform);


        fixationPoint.GetComponent<MeshRenderer>().enabled = false;
        InstructionText.SetActive(true);
        yield return new WaitForSeconds(TimeShowingInstructionText);
        InstructionText.SetActive(false);
        fixationPoint.GetComponent<MeshRenderer>().enabled = true;
        
        
        //Instruction texts
        
        yield return new WaitForSeconds(.15f);
        
        fixationPoint.SetActive(true);

        yield return new WaitForSeconds(delay);
        
        var anglesX = new List<float>();
        var anglesY = new List<float>();
        var anglesZ = new List<float>();
        
        for (var i = 1; i < keyPositions.Count; i++)
        {
            var startTime = Time.time;
            float timeDiff = 0;

            while (timeDiff < 1f)
            {
                fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * Vector3.Lerp(keyPositions[i-1], keyPositions[i], timeDiff / 1f);   
                fixationPoint.transform.LookAt(_hmdTransform);
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }
            
            // _validationPointIdx = i;
            startTime = Time.time;
            timeDiff = 0;
            
            while (timeDiff < 2f)
            {
                fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * keyPositions[i] ;
                fixationPoint.transform.LookAt(_hmdTransform);
                EyeValidationData validationData = GetEyeValidationData();
                
                if (validationData != null)
                {
                    anglesX.Add(validationData.CombinedEyeAngleOffset.x);
                    anglesY.Add(validationData.CombinedEyeAngleOffset.y);
                    anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                    
                    validationData.EyeValidationError.x = CalculateValidationError(anglesX);
                    validationData.EyeValidationError.y = CalculateValidationError(anglesY);
                    validationData.EyeValidationError.z = CalculateValidationError(anglesZ);

                    _eyeValidationData = validationData;
                }
                
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }
        }

        fixationPoint.transform.position = Vector3.zero;

        _isValidationRunning = false;
        
        fixationPoint.transform.parent = gameObject.transform;

        Debug.Log( "Get validation error" + GetValidationError() + " + " + _eyeValidationData.EyeValidationError);
        
        _eyeValidationDataFrames.Add(_eyeValidationData);
       // SaveValidationFile();


        fixationPoint.SetActive(false);
        bool successful;
        // give feedback whether the error was too large or not
        if (CalculateValidationError(anglesX) > ErrorThreshold || 
            CalculateValidationError(anglesY) > ErrorThreshold ||
          //  CalculateValidationError(anglesZ) > ErrorThreshold ||
            _eyeValidationData.EyeValidationError == Vector3.zero)
        {
            Debug.Log("calibration failed" + _eyeValidationData.EyeValidationError);
            successful = false;
        }
        else
        {
            successful = true;
            Debug.Log("calibration sucessful"+ _eyeValidationData.EyeValidationError);
           
        }

        EyeValidationArgs arguments = new EyeValidationArgs();

        arguments.eyeValidationData = _eyeValidationData;
        arguments.errorAngles = _eyeValidationData.EyeValidationError;
        arguments.leftEyeErrorAngles = _eyeValidationData.LeftEyeAngleOffset;
        arguments.rightEyeErrorAngles = _eyeValidationData.RightEyeAngleOffset;
        
        arguments.eyeValidationSuccessful = successful;


        OnValidationCompleted.Invoke(this,arguments);
    }
    
    private IEnumerator CheckErrorEyeTracker(float delay=5)
    {
        if (_isErrorCheckRunning) yield break;
        _isErrorCheckRunning = true;
        
        fixationPoint.transform.parent = _camera.gameObject.transform;

      // _hmdTransform = ExperimentManager.Instance.GetHmdTransform();

        fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,45);

        fixationPoint.transform.LookAt(_hmdTransform);

        
        
        yield return new WaitForSeconds(.15f);

        fixationPoint.SetActive(true);

        yield return new WaitForSeconds(delay);
        
        var anglesX = new List<float>();
        var anglesY = new List<float>();
        var anglesZ = new List<float>();
        
        EyeValidationData validationData = GetEyeValidationData();
            
        if (validationData != null)
        {
            anglesX.Add(validationData.CombinedEyeAngleOffset.x);
            anglesY.Add(validationData.CombinedEyeAngleOffset.y);
            anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                    
            validationData.EyeValidationError.x = CalculateValidationError(anglesX);
            validationData.EyeValidationError.y = CalculateValidationError(anglesY);
            validationData.EyeValidationError.z = CalculateValidationError(anglesZ);

            _eyeValidationData = validationData;
        }

        fixationPoint.transform.position = Vector3.zero;

        _isErrorCheckRunning = false;
        
        fixationPoint.transform.parent = gameObject.transform;

        Debug.Log( "Get validation error" + GetValidationError() + " + " + _eyeValidationData.EyeValidationError);
        
        _eyeValidationDataFrames.Add(_eyeValidationData);


        fixationPoint.SetActive(false);
        
        // give feedback whether the error was too large or not
        if (CalculateValidationError(anglesX) > ErrorThreshold || 
            CalculateValidationError(anglesY) > ErrorThreshold ||
            CalculateValidationError(anglesZ) > ErrorThreshold ||
            _eyeValidationData.EyeValidationError == Vector3.zero)
        {
            //ExperimentManager.Instance.SetValidationSuccessStatus(false);
           
        }
        else
        {
           // ExperimentManager.Instance.SetValidationSuccessStatus(true);
            
        }
    }
    
    private EyeValidationData GetEyeValidationData()
    {
        
        EyeValidationData eyeValidationData = new EyeValidationData();
        
        
        Ray ray;
        
        eyeValidationData.UnixTimestamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        eyeValidationData.IsErrorCheck = _isErrorCheckRunning;
        
        eyeValidationData.CalibrationFreq = _calibrationFreq;
        

        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;
            
            eyeValidationData.LeftEyeAngleOffset = angles;
        }
        

   
        
        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;

            eyeValidationData.RightEyeAngleOffset = angles;
        }

        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;

            eyeValidationData.CombinedEyeAngleOffset = angles;
        }
        

//        Debug.Log("validate..." + eyeValidationData.LeftEyeAngleOffset + eyeValidationData.CombinedEyeAngleOffset +eyeValidationData.RightEyeAngleOffset);
        return eyeValidationData;
    }
    
    private float CalculateValidationError(List<float> angles)
    {
        return angles.Select(f => f > 180 ? Mathf.Abs(f - 360) : Mathf.Abs(f)).Sum() / angles.Count;
    }
    
    #endregion

    #region Public methods

    public void SetExperimentStatus(bool status)
    {
     //   _isExperiment = status;
    }

    public void ValidateEyeTracking()
    {
        if(!_isValidationRunning) _runValidationCo = StartCoroutine(ValidateEyeTracker());
    }
    
    public void CheckErrorEyeTracking()
    {
        if(!_isErrorCheckRunning) _runErrorCheckCo = StartCoroutine(CheckErrorEyeTracker());
    }


    #endregion
    
    
   
}

public class EyeValidationArgs : EventArgs
{
    public bool eyeValidationSuccessful;
    public Vector3 errorAngles;
    public Vector3 leftEyeErrorAngles;
    public Vector3 rightEyeErrorAngles;
    public EyeValidationData eyeValidationData;
}
