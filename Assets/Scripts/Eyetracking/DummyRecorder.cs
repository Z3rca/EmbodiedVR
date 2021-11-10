using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRecorder : MonoBehaviour, IEyetrackingDevice
{
    private List<DummyEyetrackingDataFrame> frames;

    private void Start()
    {
        frames = new List<DummyEyetrackingDataFrame>();
        DummyEyetrackingDataFrame frame = new DummyEyetrackingDataFrame();

        frame.test = true;
        
        frames.Add(frame);
    }

    public void CalibrateDevice()
    {
        Debug.Log("Calibration");
    }

    public bool GetCalibrationStatus()
    {
        Debug.Log("I am Calibrated");
        return true;
    }

    public void StartRecording()
    {
        Debug.Log("Start Recording");
    }

    public void StopRecording()
    {
        Debug.Log("Calibration");
    }

    public bool GetRecordingStatus()
    {
        throw new System.NotImplementedException();
    }

    public void SetSampleRateHertz(float rate)
    {
        throw new NotImplementedException();
    }


    public void SaveRecordedData(string filePath)
    {
        //TODO Data Saving manager should use standard path if not given otherwise, head do not make sense right now.
        DataSavingManager.Instance.SaveList(frames,name);
    }
    
}
