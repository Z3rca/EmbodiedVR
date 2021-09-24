﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public int id;


    public Timer Timer;

    public RatingSystem RatingSystem;
    
    private double _parkourStartTimeStamp; public double parkourStartTimeStamp => _parkourStartTimeStamp;
    
    private double _parkourEndTimeStamp; public double parkourEndTimeStamp => _parkourEndTimeStamp;

    private bool _wasTeleportedToEnd; public bool wasTeleportedToEnd => _wasTeleportedToEnd;
    private double _wasTeleportedTimeStamp; public double wasTeleportedToEndTimeStamp => _wasTeleportedTimeStamp;

    private double _reachedDataGatheringRoomTimeStamp;
    public double reachedDataGatheringRoomTimeStamp => _reachedDataGatheringRoomTimeStamp;

    private double _reachedRatingBoardTimeStamp;
    public double reachedRatingBoardTimeStamp => _reachedRatingBoardTimeStamp;

    private double _startDataGatheringTimeStamp;
    public double startDataGatheringTimeStamp => _startDataGatheringTimeStamp;

    private double _endDataGatheringTimeStamp; 
    public double endDataGatheringTimeStamp => _endDataGatheringTimeStamp;


    [HideInInspector]public  double choiceTimeStamp;    //motionsickness rating end time stamp
    [HideInInspector]public int choiceValue;

    [HideInInspector]public double StartAudioRecordTime;
    [HideInInspector] public double EndAudioRecordTime;
    [HideInInspector]public double  startMotionsicknessMeasurementTime;
    [HideInInspector]public double  startPosturalStabilityTest;
    [HideInInspector]public double  endPosturalStabilityTest;
    public AudioClip participantExpierenceAudioData;
    public string AudioStringName;

    public double PosturalTestStartTime;
    public double PosturalTestEndTime;

    
    

    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.RegisterAreaManager(this);
        }

       // RatingSystem.HitEvent += AcceptRating;
    }
    
    
    public void StartParkour()
    {
        _parkourStartTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        if (Timer != null)
        {
            Timer.StartTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }

        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.PakourBegin();
        }
    }

    public void CompleteParkour()
    {
        if(!_wasTeleportedToEnd)
            _parkourEndTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        
        if (Timer != null)
        {
            Timer.StopTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }
        
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.PakourEnds();
        }
    }

    public void TeleportedToEnd()
    {
        _wasTeleportedTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        _wasTeleportedToEnd = true;
        
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.PakourEnds();
        }
    }
    public void StartDataGathering()
    {
        _startDataGatheringTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.PakourEnds();
        }
    }

    public void EndDataGathering()
    {
        _endDataGatheringTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }
    
    
    
    
    public void ReachedDataGatheringRoom()
    {
        _reachedDataGatheringRoomTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }
    
    public void ReachedRatingBoard()
    {
        _reachedRatingBoardTimeStamp =TimeManager.Instance.GetCurrentUnixTimeStamp();
    }





}
