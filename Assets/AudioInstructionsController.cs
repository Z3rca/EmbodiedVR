using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstructionsController : MonoBehaviour
{

    public AudioSource audioSource;

    private bool playedAudio = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAudio()
    {
        if (!playedAudio)
        {
            audioSource.Play();
            playedAudio = true;
        }
    }
}
