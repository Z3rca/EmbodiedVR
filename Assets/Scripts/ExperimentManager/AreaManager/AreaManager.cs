using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public int id;


    public Timer Timer;

    public RatingSystem RatingSystem;
    
    public double ParkourStartTime;
    public double ParkourEndTime;
    public bool wasTeleportedToEnd;

    public double ReachedDataGatheringRoomTime;
    public double ReachedRatingBoardTime;
    public double StartDataGatheringTime;
    public double EndDataGatheringTime;

    
    public double choiceTimeStamp;
    public int choiceValue;

    public double StartAudioRecordTime;
    public double EndAudioRecordTime;
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

        RatingSystem.HitEvent += AcceptRating;
    }



    private void AcceptRating(object sender, RatingBoardDataFrame ratingBoardDataFrame)
    {
        choiceValue = ratingBoardDataFrame.Choice;
        choiceTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }
    
    public void StartParkour()
    {
        ParkourStartTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        if (Timer != null)
        {
            Timer.StartTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }
    }

    public void CompleteParkour()
    {
        ParkourEndTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        if (Timer != null)
        {
            Timer.StopTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }
    }

    
    public void StartDataGathering()
    {
        StartDataGatheringTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }

    public void EndDataGathering()
    {
        EndDataGatheringTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }

    public void StartAudioRecording()
    {
        StartAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }

    public void EndAudioRecording()
    {
        EndAudioRecordTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        AudioStringName = participantExpierenceAudioData.name;
    }
    
    public void ReachedDataGatheringRoom()
    {
        ReachedDataGatheringRoomTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }
    
    public void ReachedRatingBoard()
    {
        ReachedRatingBoardTime =TimeManager.Instance.GetCurrentUnixTimeStamp();
    }





}
