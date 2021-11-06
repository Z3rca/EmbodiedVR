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


   private void Start()
   {
      _audioSelectionManager = ExperimentManager.Instance.GetComponent<AudioSelectionManager>();

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
      if (audioSource.clip == clip)
         return;
      if (!skipFormer)
      {
         StartCoroutine(WaitForClipPlayed());
      }
      else
      {
         
      }

      if (TutorialManager.Instance.GetIsTutorialFinished())
         return;
      
      _playingAudioClip = true;
      audioSource.clip = clip;
      StartCoroutine(PlayingAudioClip());
   }


   private IEnumerator WaitForClipPlayed()
   {
      if (_playingAudioClip)
      {
         yield return new WaitUntil(() => !_playingAudioClip);
      }
      
   }

   private IEnumerator PlayingAudioClip()
   {
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



   public bool GetPlayingAudioStatus()
   {
      return _playingAudioClip;
   }


  
}
