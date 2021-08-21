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

    public double StartRatingTime;
    public double EndRatingTime;

    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.RegisterAreaManager(this);
        }
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

    public void StartRating()
    {
        StartRatingTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }

    public void EndRating()
    {
        EndRatingTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
    }





}
