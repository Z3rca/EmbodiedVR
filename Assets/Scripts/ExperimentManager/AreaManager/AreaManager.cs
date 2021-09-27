using System;
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

    private bool _wasTeleportedToEnd; 
    public bool wasTeleportedToEnd => _wasTeleportedToEnd;
    private double _wasTeleportedTimeStamp;

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

    private double _startAudioRecordTime;
    private double _endAudioRecordTime;
    private string _audioFileName;
    [HideInInspector]public double  startMotionsicknessMeasurementTime;
    private double  _startPosturalStabilityTest;
    private double  _endPosturalStabilityTest;
    public AudioClip participantExpierenceAudioData;
 

    public double PosturalTestStartTime;
    public double PosturalTestEndTime;

    
    

    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.RegisterAreaManager(this);
        }

        Timer.TimeElasped += TeleportPlayerToExit;
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
        ExperimentManager.Instance.DataGatheringEnds();
    }

    public void BeginAudioRecording()
    {
        _startAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        
        
    }
    
    public void EndAudioRecording()
    {
        _endAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        _audioFileName = ExperimentManager.Instance.GetParticipantID()+ " "+ ExperimentManager.Instance.GetCondition() +" "+ id ;
    }
    public double GetAudioRecordingStart()
    {
        return _startAudioRecordTime;
    }
    public double GetAudioRecordingEnd()
    {
        return _endAudioRecordTime;
    }

    public string GetAudioFileName()
    {
        return _audioFileName;
    }
    public void BeginPosturalStabilityTest()
    {
        PosturalTestStartTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }
    
    public double GetBeginPosturalStabilityTest()
    {
        return PosturalTestStartTime;
    }
    
    public void EndPosturalStabilityTest()
    {
        PosturalTestEndTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }

    public double GetEndPosturalStabilityTest()
    {
        return PosturalTestEndTime;
    }
    
    public void ReachedDataGatheringRoom()
    {
        _reachedDataGatheringRoomTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();

    }
    
    public void ReachedRatingBoard()
    {
        _reachedRatingBoardTimeStamp =TimeManager.Instance.GetCurrentUnixTimeStamp();
    }


    public void TeleportPlayerToExit()
    {
        _wasTeleportedTimeStamp= TimeManager.Instance.GetCurrentUnixTimeStamp();
        _wasTeleportedToEnd = true;
        ExperimentManager.Instance.GetPlayerController().TeleportToPosition(Timer.exit.transform);
    }


    public double GetWasTeleportedTimeStamp()
    {
        return _wasTeleportedTimeStamp;
    }





}
