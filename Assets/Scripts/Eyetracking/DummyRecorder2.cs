using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRecorder2 : MonoBehaviour, IEyetrackingDevice
{
    public void CalibrateDevice()
    {
        Debug.Log("Calibrate 2");
    }

    public bool GetCalibrationStatus()
    {
        throw new System.NotImplementedException();
    }

    public void StartRecording()
    {
        throw new System.NotImplementedException();
    }

    public void StopRecording()
    {
        throw new System.NotImplementedException();
    }

    public bool GetRecordingStatus()
    {
        throw new System.NotImplementedException();
    }

    public void SetSampleRateHertz(float rate)
    {
        throw new System.NotImplementedException();
    }

    public void SaveRecordedData(string filePath)
    {
        throw new System.NotImplementedException();
    }
}
