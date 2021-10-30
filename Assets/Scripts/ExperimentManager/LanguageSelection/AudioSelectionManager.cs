using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class AudioSelectionManager : MonoBehaviour
{
    [SerializeField] private int requiredClips=  24;
    [SerializeField] private List<AudioClip> germanClips;
    [SerializeField] private List<AudioClip> englishClips;


    private Dictionary<string, AudioClip> _germanAudioDictionary;
    private Dictionary<string, AudioClip> _chosenAudioDictionarty;
    private Dictionary<string, AudioClip> _englishAudioDictionary; 
    private List<AudioSource> _audioSources;
    private Language _setLanguage;


    private void Awake()
    {
        _audioSources = new List<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _germanAudioDictionary = new Dictionary<string, AudioClip>();
        _englishAudioDictionary = new Dictionary<string, AudioClip>();

        if (germanClips.Count != requiredClips || englishClips.Count != requiredClips)
        {
            Debug.LogError("The amount of clips is not sufficient, please check for Mistakes");
        }

        foreach (var audioClip in germanClips)
        {
            string name;
            var indexOf = audioClip.name.IndexOf("_de", StringComparison.Ordinal);
            name = audioClip.name.Substring(0, indexOf);
         
            _germanAudioDictionary.Add(name,audioClip);
          
        }
        
        foreach (var audioClip in englishClips)
        {
            string name;
            var indexOf = audioClip.name.IndexOf("_en", StringComparison.Ordinal);
            name = audioClip.name.Substring(0, indexOf);
            _englishAudioDictionary.Add(name,audioClip);
        }

        
        foreach (var pair in _germanAudioDictionary)
        {
            Debug.Log(pair.Key+ " "+  pair.Value.name);
        }
        
        foreach (var pair in _englishAudioDictionary)
        {
            Debug.Log(pair.Key+ " "+  pair.Value.name);
        }

    }


    public void RegisterAudioSource(AudioSource audioSource)
    {
        Debug.Log("hey there Im audio"
        + " clip " + audioSource.clip.name);
        if (_audioSources != null)
        {
            _audioSources.Add(audioSource);
        }

    }

    public void SetLanguage(Language language)
    {
        _setLanguage = language;

        switch (language)
        {
            case Language.German:
                foreach (var audioSource in _audioSources)
                {
                    OverrideAudioClipFromDictionary(_germanAudioDictionary, audioSource, language);
                }

                break;
            case Language.English:
                foreach (var audioSource in _audioSources)
                {
                    OverrideAudioClipFromDictionary(_englishAudioDictionary, audioSource, language);
                }
                break;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("assign language");
            SetLanguage(Language.German);
        }
    }

    private void OverrideAudioClipFromDictionary(Dictionary<string, AudioClip> dictionary, AudioSource AssignedSource, Language language)
    {
       
        int index = 0;
        
        index = AssignedSource.clip.name.IndexOf("_de", StringComparison.Ordinal);

        if (index == -1)
        {
            index = AssignedSource.clip.name.IndexOf("_en", StringComparison.Ordinal);
        }
        
        string name = AssignedSource.clip.name.Substring(0, index);

        if (dictionary.ContainsKey(name))
            AssignedSource.clip = dictionary[name];
        else
        {
            Debug.LogError("AUDIO FILE NOT FOUND " + AssignedSource.gameObject.name);
        }
    }

    
}

public enum Language
{
    German,
    English
}
