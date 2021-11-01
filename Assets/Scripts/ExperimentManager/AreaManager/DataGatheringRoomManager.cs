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
        _measuringFlow.DataGatheringEnded += EndDataGathering;


    }

    public void EnteringDataGatheringRoom()
    {
        if (ExperimentManager.Instance != null)
        {
            _areaManager = ExperimentManager.Instance.GetCurrentAreaManager();
        }
        else
        {
            Debug.Log("EXperimentManager not found");
        }

        if (_areaManager != null)
        {
            _areaManager.ReachedDataGatheringRoom();
        }
        else
        {
            Debug.LogWarning("Area manager was not found");
        }
        
        _measuringFlow.StartDataGathering();
    }


    private void StartAudioRecording()
    {
        if (_areaManager != null)
        {
            Debug.Log("saving Audio begin");
            _areaManager.BeginAudioRecording();
        }
        else
        {
            Debug.LogWarning("Area manager was not found, did not correclty collect data");
        }
    }
    
    private void EndAudioRecording()
    {
        
        if (_areaManager != null)
        {
            Debug.Log("saving Audio end");
            _areaManager.EndAudioRecording();
        }
        else
        {
            Debug.LogWarning("Area manager was not found, did not correclty collect data");
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
            _areaManager.BeginPosturalStabilityTest();
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
            _areaManager.EndPosturalStabilityTest();
        }
    }

    public void EndDataGathering()
    {
        _areaManager.EndDataGathering();
    }

    
    
    
    
}
