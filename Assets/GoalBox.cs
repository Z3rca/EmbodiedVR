using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoalBox : MonoBehaviour
{

    public int counter = 0;
    public int goal = 3;
    public UnityEvent goalReached;
    private bool eventCalled = false;

    public TMP_Text visibleCounter;
    
    public AudioSource audioSource;
    public AudioClip enoughCollected;
    
    // Start is called before the first frame update
    void Start()
    {
        visibleCounter.text = counter + "/" + goal + "spheres collected! \r\n" + (goal-counter) + " are still needed to exit.";
    }

    // Update is called once per frame
    void Update()
    {
        if (!eventCalled & counter>=goal)
        {
            goalReached.Invoke();
            audioSource.clip = enoughCollected;
            audioSource.Play();
            eventCalled = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Sphere")
        {
            counter++;
            visibleCounter.text = counter + " spheres collected! \r\n" + Math.Max((goal-counter), 0) + " still needed to exit.";
            other.gameObject.SetActive(false);
        }
    }
}
