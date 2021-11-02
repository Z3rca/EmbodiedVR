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
    private float _remainingAudioFileLength;
    [SerializeField] private bool IsDebug;

    [SerializeField] private string Device;

    [SerializeField] private int frequency = 44100;
    
    
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
        
    }

    public float GetRatioOfRemainingFileLenth()
    {
        return _remainingAudioFileLength;
    }

    public void StartRecording(int timeInSeconds, bool loop=false)
    {
        
        _audioSource.Stop();
        _audioSource.clip = Microphone.Start(Device, loop, timeInSeconds, frequency);
        _isRecording=true;
        StartCoroutine(CountRemainingPercentage(timeInSeconds));

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

    private IEnumerator CountRemainingPercentage(int TotaltimeInSeconds)
    {
        float remainingTime = TotaltimeInSeconds;
        while (_isRecording)
        {
            remainingTime -= Time.deltaTime;
            _remainingAudioFileLength = remainingTime/TotaltimeInSeconds; 
            yield return new WaitForEndOfFrame();
        }

        _remainingAudioFileLength = 0f;
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
