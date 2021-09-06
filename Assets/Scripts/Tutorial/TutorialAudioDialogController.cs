using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioDialogController : MonoBehaviour
{
   public AudioClip clip_1;
   public AudioClip clip_2;
   public AudioClip clip_3;
   public AudioClip clip_4;
   public AudioClip clip_5;
   public AudioClip clip_6;
   public AudioClip clip_7;
   public AudioClip clip_8;
   public AudioClip clip_9;
   public AudioClip clip_10;
   public AudioClip clip_11;
   public AudioClip clip_12;
   public AudioClip clip_13;
   public AudioSource audioSource;

   private bool _playingAudioClip;
   public void Start()
   {
      
   }


   public void AudioClip1()
   {
      StartAudioClip(clip_1);
   }

   public void AudioClip2()
   {
      StartAudioClip(clip_2);
   }
   
   public void AudioClip3()
   {
      StartAudioClip(clip_3);
   }

   public void AudioClip4()
   {
      StartAudioClip(clip_4);
   }
   
   public void AudioClip5()
   {
      StartAudioClip(clip_5);
   }
   
   public void AudioClip6()
   {
      StartAudioClip(clip_6);
   }
   
   public void AudioClip7()
   {
      StartAudioClip(clip_7);
   }

   public void AudioClip8()
   {
      StartAudioClip(clip_8);
   }
   
   public void AudioClip9()
   {
      StartAudioClip(clip_9);
   }
   
   public void AudioClip10()
   {
      StartAudioClip(clip_10);
   }
   
   public void AudioClip11()
   {
      StartAudioClip(clip_11);
   }
   
   public void AudioClip12()
   {
      StartAudioClip(clip_12);
   }
   
   public void AudioClip13()
   {
      StartAudioClip(clip_13);
   }
   
   public void FinishedTask()
   {
      throw new NotImplementedException();
   }




   private void StartAudioClip(AudioClip clip)
   {
      _playingAudioClip = true;
      audioSource.clip = clip;
      StartCoroutine(PlayingAudioClip());
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
   }



   public bool GetPlayingAudioStatus()
   {
      return _playingAudioClip;
   }


  
}
