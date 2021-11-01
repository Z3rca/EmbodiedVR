using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImprovedTimer : MonoBehaviour
{
    public float timerInMinutes;

    public AudioSource firstWarning19;
    public AudioSource secondWarning20;
    private float timeRemaining;
    private bool _timerIsRunning = false;
    private bool firstClipPlayed = false;
    private bool secondClipPlayed = false;
    
    public UnityEvent whenTimerIsUp;
    
    // Start is called before the first frame update
    void Start()
    {
        // start timer
        timeRemaining = timerInMinutes * 60;
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
            else if (timeRemaining <= 15 & timeRemaining > firstWarning19.clip.length)
            {
                // starting countdown
                if (firstClipPlayed == false)
                {
                    firstWarning19.Play();
                    firstClipPlayed = true;
                }

                timeRemaining -= Time.deltaTime;
            }
            else if (timeRemaining <= secondWarning20.clip.length & timeRemaining > 0)
            {
                // warning before teleport
                if (secondClipPlayed == false)
                {
                    secondWarning20.Play();
                    secondClipPlayed = true;
                }

                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                _timerIsRunning = false;
                // invoke unity event
                whenTimerIsUp.Invoke();
            }
        }
    }
}