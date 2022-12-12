using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area2Manager : AreaManager
{
    
    public ImprovedTimer ImprovedTimer;
    

    private void Start()
    {
        if (ExperimentManager.Instance != null)
        {
            ExperimentManager.Instance.RegisterAreaManager(this);
        }
    }


    public void StartArea()
    {
        if (ImprovedTimer != null)
        {
            ImprovedTimer.StartTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }
    }

    public void StopArea()
    {
        if (ImprovedTimer != null)
        {
            ImprovedTimer.StopTimer();
        }
        else
        {
            Debug.Log("Timer is not assigned");
        }
    }
}
