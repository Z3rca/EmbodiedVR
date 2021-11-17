using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;

/*The Eyetracking Manager is the the callable instance for the Experiment Manager, which keeps information which of all compartments,
 concerning Eyetracking Devices, potential validation and so on. Ultimately, others should not be connected to those, except the Saving System */


public class ELIEyetrackingManager : MonoBehaviour
{
    public static ELIEyetrackingManager Instance { get; private set; }

    public EyetrackingValidation _eyetrackingValidation;
    public int SamplingRate = 90;
    private Transform _hmdTransform;
    private bool _eyeValidationSucessful;
    private bool _calibrationSuccess;


    private bool _validationCompleted;

    private bool _isCalibrated;

    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    private Vector3 _leftEyeValidationErrorAngles;
    private Vector3 _rightEyeValidationErrorAngles;

    private EyeValidationData _validationData;
    
    public delegate void OnCompletedEyeValidation(bool wasSuccessful);
    public event OnCompletedEyeValidation NotifyEyeValidationCompletnessObservers;
    
    
    public Vector3 CombinedEyeAngleOffset;
    public Vector3 LeftEyeAngleOffset;
    public Vector3 RightEyeAngleOffset;


    private string TestFileName;

    
    private  IEyetrackingDevice device;
    
    

    private void Update()
    {
        
    }


    private void Start()
    {
        _eyetrackingValidation.OnValidationCompleted += OnValidationCompleted;
    }


    public void AbortValidation()
    {
        //_eyetrackingValidation.AbortValidation();
        //NotifyEyeValidationCompletnessObservers?.Invoke(false);
    }

    public void StartValidation(float delay)
    {
       // _eyetrackingValidation.StartValidation(delay);        // Validation shouldn't be done here, potential new class
    }
    
    
    public void StartCalibration()
    {
        _isCalibrated = SRanipal_Eye_v2.LaunchEyeCalibration();
    }

    public void StartValidation()
    {
        _eyetrackingValidation.SetHMDTransform(ExperimentManager.Instance.GetActiveCamera());
        _eyetrackingValidation.StartValidateEyetracker();
    }

    public void StartRecording()
    {
        Debug.Log("<color=green>Recording eye-tracking Data!</color>");
        device.StartRecording();
    }

    public void SaveEyetrackingData(string fileName="Test")
    {
        device.SaveRecordedData(Path.Combine(DataSavingManager.Instance.GetSavePath(), fileName + ".json"));
    }

    
    
    
    public void StopRecording()
    {
        Debug.Log("<color=red>Stopping recording eye-tracking Data!</color>");
        device.StopRecording();
    }

    public bool EyetrackerIsCalibrated()
    {
        return _isCalibrated;
    }
    
    public Transform GetHmdTransform()
    {
        return _hmdTransform;
    }
    

    public Vector3 GetCombinedEyeValidationErrorAngles()
    {
        return _eyeValidationErrorAngles;
    }

    public Vector3 GetLeftEyeValidationErrorAngles()
    {
        return _leftEyeValidationErrorAngles;
    }
    
    public Vector3 GetRightEyeValidationErrorAngles()
    {
        return _rightEyeValidationErrorAngles;
    }

    
    
    private void StoreEyeTrackingData()
    {
    }

    private void OnValidationCompleted(object sender, EyeValidationArgs eyeValidationArgs)
    {
        _eyeValidationErrorAngles = eyeValidationArgs.errorAngles;
        _leftEyeValidationErrorAngles = eyeValidationArgs.leftEyeErrorAngles;
        _rightEyeValidationErrorAngles = eyeValidationArgs.rightEyeErrorAngles;
        _eyeValidationSucessful = eyeValidationArgs.eyeValidationSuccessful;
        _validationData = eyeValidationArgs.eyeValidationData;
        _validationCompleted = true;
        
    }

    public bool GetValidationCompletedSatus()
    {
        return _validationCompleted;
    }
    
    private void SetEyeValidationStatus(bool eyeValidationWasSucessfull, Vector3 errorAngles)
    {
        Debug.Log("eyeValidation Status was called in EyeTrackingManager with " + eyeValidationWasSucessfull);
        _eyeValidationSucessful = eyeValidationWasSucessfull;
        
        if (!eyeValidationWasSucessfull)
        {
            _eyeValidationErrorAngles = errorAngles;
            NotifyEyeValidationCompletnessObservers?.Invoke(false);
            
        }
        else
        {
            _eyeValidationErrorAngles = errorAngles;
            NotifyEyeValidationCompletnessObservers?.Invoke(true);
        }
    }

    public bool GetEyeValidationStatus()
    {
        return _eyeValidationSucessful;
    }
    
    public double getCurrentTimestamp()
    {
        //This should not be done here, we take a TimeManager for this.
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
}
