using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timerInMinutes;
    public GameObject player;
    public Vector3 exit;
    public AudioSource audioSource;
    public AudioClip firstWarning;
    public AudioClip secondWarning;

    private float timeRemaining;
    private bool timerIsRunning = false;
    private bool firstClipPlayed = false;
    private bool secondClipPlayed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // start timer
        timerIsRunning = true;
        timeRemaining = timerInMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        // if timer is running, reduce time every frame by deltaTime
        if (timerIsRunning)
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
                timerIsRunning = false;
                // teleport player to exit
                player.transform.position = exit;
            }
        }
    }
}
