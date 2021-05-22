using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioDialogController : MonoBehaviour
{
   public AudioClip Familarization;
   public AudioSource audioSource;

   private bool _playingAudioClip;
   public void Start()
   {
      
   }


   public void FamilarizationAudioClip()
   {
      StartAudioClip(Familarization);
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
