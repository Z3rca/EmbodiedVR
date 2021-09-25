using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timerInMinutes;
    public GameObject exit;
    public AudioSource audioSource;
    public AudioClip firstWarning;
    public AudioClip secondWarning;

    private float timeRemaining;
    private bool _timerIsRunning = false;
    private bool firstClipPlayed = false;
    private bool secondClipPlayed = false;
    
    public event Action TimeElasped;
    
    // Start is called before the first frame update
    void Start()
    {
        ExperimentManager.Instance.startedExperiment += OnExperimentStarted;
    }


    void OnExperimentStarted(object sender, EventArgs eventArgs)
    {
        timeRemaining = timerInMinutes * 60;
        // audioSource = FindObjectOfType<AudioSource>();

    }


    public void StartTimer()
    {
        _timerIsRunning = true;
    }

    public void StopTimer()
    {
        _timerIsRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if timer is running, reduce time every frame by deltaTime
        if (_timerIsRunning) 
        {
            if (timeRemaining > 15)
            {
                timeRemaining -= Time.deltaTime;
            }
            else if (timeRemaining <= 15 & timeRemaining > secondWarning.length)
            {
                // starting countdown
                if (firstClipPlayed == false)
                {
                    audioSource.clip = firstWarning;
                    audioSource.Play();
                    firstClipPlayed = true;
                }
                timeRemaining -= Time.deltaTime;
            }
            else if (timeRemaining <= secondWarning.length & timeRemaining > 0)
            {
                // warning before teleport
                if (secondClipPlayed == false)
                {
                    audioSource.clip = secondWarning;
                    audioSource.Play();
                    secondClipPlayed = true;
                }
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                _timerIsRunning = false;
                // teleport player to exit
                TimeElasped.Invoke();
            }
        }
    }
}
