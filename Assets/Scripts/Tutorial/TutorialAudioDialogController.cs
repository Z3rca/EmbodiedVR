using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioDialogController : MonoBehaviour
{
   public AudioSource audioSource;
   [SerializeField] private List<AudioClip> _audioClips;
   private AudioSelectionManager _audioSelectionManager;
   private bool _playingAudioClip;

   private bool active;
   private List<AudioClip> _currentListedAudioClips;
   private void Start()
   {
      _audioSelectionManager = ExperimentManager.Instance.GetComponent<AudioSelectionManager>();
      _currentListedAudioClips = new List<AudioClip>();
   }

  
   public void ForceStopAllAudio()
   {
      audioSource.Stop();
      _playingAudioClip = false;
   }
   public void SwitchClipsToLanguage(Language language)
   {
      for (int i = 0; i < _audioClips.Count; i++)
      {
         _audioClips[i] = _audioSelectionManager.GetClipInCorrectLanguage(_audioClips[i].name);
      }
   }

   public bool GetActive()
   {
      return active;
   }

   public List<AudioClip> GetAudioClips()
   {
      return _audioClips;
   }

   public void AudioClip1()
   {
      StartAudioClip(_audioClips[0]);
   }

   public void AudioClip2()
   {
      StartAudioClip(_audioClips[1]);
   }
   
   public void AudioClip3()
   {
      StartAudioClip(_audioClips[2]);
   }

   public void AudioClip4()
   {
      StartAudioClip(_audioClips[3]);
   }
   
   public void AudioClip5()
   {
      StartAudioClip(_audioClips[4]);
   }
   
   public void AudioClip6()
   {
      StartAudioClip(_audioClips[5]);
   }
   
   public void AudioClip7()
   {
      StartAudioClip(_audioClips[6]);
   }

   public void AudioClip8()
   {
      StartAudioClip(_audioClips[7]);
   }
   
   public void AudioClip9()
   {
      StartAudioClip(_audioClips[8]);
   }
   
   public void AudioClip10()
   {
      StartAudioClip(_audioClips[9]);
   }
   
   public void AudioClip11()
   {
      StartAudioClip(_audioClips[10]);
   }
   
   public void AudioClip12()
   {
      StartAudioClip(_audioClips[11]);
   }
   
   public void AudioClip13()
   {
      StartAudioClip(_audioClips[12]);
   }
   
   public void FinishedTask()
   {
      throw new NotImplementedException();
   }




   private void StartAudioClip(AudioClip clip, bool skipFormer=false)
   {
      active = true;
      /*Debug.Log("play Clip + " + clip.name);
      if (audioSource.clip == clip)
         return;
      if (skipFormer)
      {
         _playingAudioClip = false;
         ForceStopAllAudio();
      }
      
      
      if (TutorialManager.Instance.GetIsTutorialFinished())
         return;
      
      _currentListedAudioClips.Add(clip);
      StartCoroutine(PlayingAudioClip(clip));*/
      
      
      if(!_currentListedAudioClips.Contains(clip))
         _currentListedAudioClips.Add(clip);
   }


   private void Update()
   {
      
      
      if (active)
      {
         if (!_playingAudioClip)
         {
            if (_currentListedAudioClips.Count > 0)
            {
               StartCoroutine(PlayingAudioListItem(_currentListedAudioClips[0]));
            }
         }
         
         if (_currentListedAudioClips.Count == 0)
         {
            active = false;
         }
      }
   }

   
   
   private IEnumerator PlayingAudioClip(AudioClip clip)
   {
      yield return new WaitUntil(() => !_playingAudioClip);
      _playingAudioClip = true;
      audioSource.clip = clip;
      audioSource.Play();
      while (_playingAudioClip)
      {
         if (!audioSource.isPlaying)
         {
            _playingAudioClip = false;
         }

         yield return new WaitForEndOfFrame();
      }

      audioSource.clip = null;
   }

   private IEnumerator PlayingAudioListItem(AudioClip clip)
   {
      if (!_playingAudioClip)
      {
         _playingAudioClip = true;
         audioSource.clip = clip;
         audioSource.Play();
         while (audioSource.isPlaying)
         {
            yield return new WaitForEndOfFrame();
         }
      
         _currentListedAudioClips.RemoveAt(0);
         _playingAudioClip = false;
      }
   }



   public bool GetPlayingAudioStatus()
   {
      return _playingAudioClip;
   }


  
}
