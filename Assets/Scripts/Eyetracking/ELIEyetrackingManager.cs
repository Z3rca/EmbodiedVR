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

    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    
    public delegate void OnCompletedEyeValidation(bool wasSuccessful);
    public event OnCompletedEyeValidation NotifyEyeValidationCompletnessObservers;
    
    
    public Vector3 CombinedEyeAngleOffset;
    public Vector3 LeftEyeAngleOffset;
    public Vector3 RightEyeAngleOffset;


    private string TestFileName;

    
    private  IEyetrackingDevice device;

    private void Start()
    {
        _eyetrackingValidation = GetComponent<EyetrackingValidation>();
    }

    private void Update()
    {
        
    }
    
    

    public void StartValidation()
    {
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
        SRanipal_Eye_v2.LaunchEyeCalibration();
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
        return device.GetCalibrationStatus();
    }
    
    public Transform GetHmdTransform()
    {
        return _hmdTransform;
    }
    

    public Vector3 GetEyeValidationErrorAngles()
    {
        return _eyeValidationErrorAngles;
    }
    

    private void StoreEyeTrackingData()
    {
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
