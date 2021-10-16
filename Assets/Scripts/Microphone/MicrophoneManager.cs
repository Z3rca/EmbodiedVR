using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private List<AudioClip> Audioclips;
    private bool _isRecording;
    private string _audioFileName;
    [SerializeField] private bool IsDebug;

    [SerializeField] private string Device;

    [SerializeField] private int frequency = 44100;

    [SerializeField] private float[] _data;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (string.IsNullOrEmpty(Device))
        {
            foreach (var name in Microphone.devices)
            {
                Debug.Log(name);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartRecording(10);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StopRecording();
        }
    }


    public void StartRecording(int timeInSeconds, bool loop=false)
    {
        
        _audioSource.Stop();
        _audioSource.clip = Microphone.Start(Device, loop, timeInSeconds, frequency);

        if (Microphone.IsRecording(Device))
        {
            //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
            while (!(Microphone.GetPosition(Device) > 0))
            {
            } // Wait until the recording has started. 
            
            Debug.Log(Device+ " records...");
            
        }
        else
        {
            Debug.LogWarning(Device + " is not working");
        }



        StartCoroutine(StoreDataAfterRecording(timeInSeconds));
    }

    private IEnumerator StoreDataAfterRecording(int timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        StopRecording();

        if (IsDebug)
        {
            _audioSource.Play();
        }
        
    }

    public bool isRecording()
    {
        return _isRecording;
    }

    public void StopRecording()
    {
         Microphone.End(Device);
         _isRecording = false;
         // _audioSource.clip.Create()
    }

    public void SetAudioFileName(string name)
    {
        _audioFileName = name;
    }

    public void SaveAudioClip()
    {
        if (_isRecording)
        {
            StopRecording();
        }
        _audioSource.clip.GetData(_data, 0);
        DataSavingManager.Instance.SaveToWav(_audioSource.clip,_audioFileName);
    }

    public void ClearData()
    {
        if (_isRecording)
        {
            StopRecording();
        }

        _audioSource.clip = null;
    }
}
