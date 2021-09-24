using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGatheringRoomManager : MonoBehaviour
{
    private AreaManager _areaManager;
    [SerializeField] private MeasuringFlow _measuringFlow;
    [SerializeField] private RatingSystem _ratingSystem;
    private int motionsicknessChoice;
    
    private void Start()
    {
        
        //events 
            //audio
        _measuringFlow.AudioRecordingStarted+=StartAudioRecording;
        _measuringFlow.AudioRecordingEnded+=EndAudioRecording;
            //motionsickness
        _measuringFlow.MotionsicknessMeasurementStart += SicknessRatingStarted;  
        _ratingSystem.HitEvent += AcceptSicknessRating;
            //postural stabilty
        _measuringFlow.PosturalStabilityTestStarted += StartedPosturalStabilityTest;
        _measuringFlow.PosturalStabitityTestEnded += EndedPosturalStabilityTest;


    }

    public void EnteringDataGatheringRoom()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }

        if (_areaManager != null)
        {
            _areaManager.ReachedDataGatheringRoom();
        }
    }


    private void StartAudioRecording()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        
        if (_areaManager != null)
        {
            _areaManager.StartAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }
    
    private void EndAudioRecording()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        
        if (_areaManager != null)
        {
            _areaManager.EndAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }

    public void ReachedRatingBoard()
    {
        if (_areaManager == null)
        {
            Debug.LogWarning("Area manager was not found, did not correclty collect data");
        }
        if (_areaManager != null)
        {
            _areaManager.ReachedRatingBoard();
        }
    }

    public void SicknessRatingStarted()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        
        if (_areaManager != null)
        {
            _areaManager.startMotionsicknessMeasurementTime= TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }

    public void AcceptSicknessRating(object sender, RatingBoardDataFrame ratingBoardDataFrame)
    {
        motionsicknessChoice= ratingBoardDataFrame.Choice;
        
        if (ExperimentManager.Instance != null)
        {
            _areaManager.choiceValue = motionsicknessChoice;
            _areaManager.choiceTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }

    public void StartedPosturalStabilityTest()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        
        if (_areaManager != null)
        {
            _areaManager.startPosturalStabilityTest = TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }
    
    public void EndedPosturalStabilityTest()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        
        if (_areaManager != null)
        {
            _areaManager.endPosturalStabilityTest = TimeManager.Instance.GetCurrentUnixTimeStamp();
        }
    }

    
    
    
    
}
