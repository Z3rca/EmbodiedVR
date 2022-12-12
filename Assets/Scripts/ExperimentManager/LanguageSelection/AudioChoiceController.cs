using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChoiceController : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource= GetComponent<AudioSource>();
        
        ExperimentManager.Instance.GetComponent<AudioSelectionManager>().RegisterAudioSource(_audioSource);
    }

    
}
