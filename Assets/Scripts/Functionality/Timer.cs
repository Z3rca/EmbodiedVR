using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timerInMinutes;
    public GameObject player;
    public Vector3 exit;
    
    private float timeRemaining;
    private bool timerIsRunning = false;
    
    
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
            else if (timeRemaining <= 15 & timeRemaining > 0)
            {
                // starting countdown
                Debug.Log("Time remaining" + timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                // teleport player to exit
                player.transform.position = exit;
            }
        }
    }
}
