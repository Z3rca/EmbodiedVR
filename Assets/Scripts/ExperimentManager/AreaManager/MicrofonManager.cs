using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrofonManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private string DeviceName;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartRecording()
    {
        _audioSource.clip = Microphone.Start(DeviceName, false, 120, 44100);
    }

    public void StopRecording()
    {
         Microphone.End(DeviceName);
    }
}
