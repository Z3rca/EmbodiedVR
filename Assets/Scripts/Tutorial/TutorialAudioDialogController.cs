using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioDialogController : MonoBehaviour
{
   public AudioClip Familarization;
   public AudioClip SwitchViewButton;
   public AudioClip Movement;
   public AudioClip Interaction;
   public AudioClip PickUp;
   public AudioClip ThrowBallInBoxInstruction;
   public AudioClip ThrowBallInBox;
   public AudioClip ExitTutorial;
   public AudioSource audioSource;

   private bool _playingAudioClip;
   public void Start()
   {
      
   }


   public void FamilarizationAudioClip()
   {
      StartAudioClip(Familarization);
   }

   public void SwitchViewButtonAudioClip()
   {
      StartAudioClip(SwitchViewButton);
   }
   
   public void MovementAudioClip()
   {
      StartAudioClip(Movement);
   }

   public void InteractionAudioClip()
   {
      StartAudioClip(Interaction);
   }
   
   public void PickBallAudioClip()
   {
      StartAudioClip(PickUp);
   }
   
   public void ThrowBallInBoxInstructionAudioClip()
   {
      StartAudioClip(ThrowBallInBoxInstruction);
   }
   
   public void ThrowBallInBoxAudioClip()
   {
      StartAudioClip(ThrowBallInBox);
   }

   public void ExitTutorialAudioClip()
   {
      StartAudioClip(ExitTutorial);
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
