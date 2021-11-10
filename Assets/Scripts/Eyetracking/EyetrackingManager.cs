using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/*The Eyetracking Manager is the the callable instance for the Experiment Manager, which keeps information which of all compartments,
 concerning Eyetracking Devices, potential validation and so on. Ultimately, others should not be connected to those, except the Saving System */


public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }

    public int SamplingRate = 90;
    private Transform _hmdTransform;
    private bool _eyeValidationSucessful;
    private bool _calibrationSuccess;

    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    
    public delegate void OnCompletedEyeValidation(bool wasSuccessful);
    public event OnCompletedEyeValidation NotifyEyeValidationCompletnessObservers;


    private string TestFileName;

    
    private  IEyetrackingDevice device;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
        
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Eyetracking Manager should be persistent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        //  I do not like this: we still needs tags to find that out.
    }

    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _hmdTransform = Camera.main.transform;
        //Debug.Log("hello new World");
    }

    // Start is called before the first frame update
    void Start()
    {
        device = GetComponent<IEyetrackingDevice>();        // this might be done a little bit more elegant, still no need to change this now. 
                                                            //TODO check for single instance for such device.
        device.SetSampleRateHertz(SamplingRate);            // update sampling rate 
                                                            
        TestFileName = "Test";
    }


    private void Update()
    {
        
    }
    
    

    public void StartValidation()
    {
     //   Debug.Log("validating...");
     //   _eyetrackingValidation.StartValidation(eyeValidationDelay);
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
       device.CalibrateDevice();
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
