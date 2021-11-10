using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEyetrackingDevice
{ 
    void CalibrateDevice();

    void SetSampleRateHertz(float rate);

    bool GetCalibrationStatus();

    void StartRecording();

    void StopRecording();

    bool GetRecordingStatus();

    void SaveRecordedData(string filePath); 
}
