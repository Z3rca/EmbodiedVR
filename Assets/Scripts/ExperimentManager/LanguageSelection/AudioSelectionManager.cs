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
    private List<TextSelectionController> _textSelectionControllers;
    
    private Language _setLanguage;

    private TutorialAudioDialogController _audioDialogController;


    private void Awake()
    {
        _audioSources = new List<AudioSource>();
        _textSelectionControllers = new List<TextSelectionController>();
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
        

        _audioDialogController = ExperimentManager.Instance.tutorialManager.GetComponent<TutorialAudioDialogController>();

    }


    public void RegisterAudioSource(AudioSource audioSource)
    {
        if (_audioSources != null)
        {
            _audioSources.Add(audioSource);
        }

    }
    
    public void RegisterTextController(TextSelectionController controller)
    {
        _textSelectionControllers.Add(controller);
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

                foreach (var controller in _textSelectionControllers)
                {
                    controller.SetGermanText();
                }

                _audioDialogController.SwitchClipsToLanguage(Language.German);
                
                break;
            case Language.English:
                foreach (var audioSource in _audioSources)
                {
                    OverrideAudioClipFromDictionary(_englishAudioDictionary, audioSource, language);
                }
                
                foreach (var controller in _textSelectionControllers)
                {
                    controller.SetEnglishText();
                }
                
                _audioDialogController.SwitchClipsToLanguage(Language.English);
                break;
        }
        
        
    }


    public AudioClip GetClipInCorrectLanguage(string audioClipName)
    {
        Language language;
        int index = 0;
        index = audioClipName.IndexOf("_de", StringComparison.Ordinal);
        language = Language.German;
      
        if (index == -1)
        {
            index = audioClipName.IndexOf("_en", StringComparison.Ordinal);
            language = Language.English;
        }
        

        if (_setLanguage == Language.English)
        {
            string name = audioClipName.Substring(0, index);
            return _englishAudioDictionary[name];
        }

        if (_setLanguage == Language.German)
        {
            string name = audioClipName.Substring(0, index);
            return _germanAudioDictionary[name];
        }

        return null;

    }

    

    // Update is called once per frame
    void Update()
    {
       
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
