using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RatingSystem : MonoBehaviour
{
   private int _value; //choice
   private double currentTimeStamp;
   private List<int> _choiceValues;
   private List<double> _timeStamps;

   private bool activeRatingProcess;
   [SerializeField] private string basic;
   [SerializeField] private GameObject PreSelection;
   [SerializeField] private GameObject PostSelection;
   [SerializeField] private List<GameObject> RatingButtons;
   [SerializeField] private GameObject AcceptButton;
   public EventHandler<RatingBoardDataFrame> HitEvent;

   private bool readyToAccept;


   public UnityEvent whenRatingFinished;

   private void Start()
   {
      _choiceValues = new List<int>();
      _timeStamps = new List<double>();

      foreach (var button in RatingButtons)
      {
         button.GetComponent<ButtonMaterialModifier>().LockMaterial();
      }
   }

   public void SetActiveRatingProcess(bool state)
   {
      InitialText();
      activeRatingProcess = state;
      LockAcceptButton();
   }
   public void SetValue(int val)
   {
      if(!activeRatingProcess)
         return;
            
      _value = val;
      ChangedText();
      ResetColors();
      readyToAccept = true;
      SetButtonColored(RatingButtons[val]);
      UnlockAcceptButton();
      
   }

   private void InitialText()
   {
      PreSelection.SetActive(true);
      PostSelection.SetActive(false);
   }
   private void ChangedText()
   {
      PreSelection.SetActive(false);
      PostSelection.SetActive(true);
   }
   private void SetText(int val)
   {
      //textField.text = val;
   }

   public void Restart()
   {
      LockAcceptButton();
      ResetColors();
      _value = 0;
      SetText(_value);
   }

   
   private void UnlockAcceptButton()
   {
      AcceptButton.GetComponent<ButtonMaterialModifier>().ChangeMaterial();
   }

   private void LockAcceptButton()
   {
      AcceptButton.GetComponent<ButtonMaterialModifier>().LockMaterial();
   }
   private void ResetColors()
   {
      foreach (var button in RatingButtons)
      {
         button.GetComponent<ButtonMaterialModifier>().RestoreMaterial();
      }
   }

   private void SetButtonColored(GameObject button)
   {
      button.GetComponent<ButtonMaterialModifier>().ChangeMaterial();
   }

   public void AcceptRating()
   {
      if (readyToAccept)
      {
         RatingBoardDataFrame frame = new RatingBoardDataFrame()
         {
            Approved = true,
            Ratings = _choiceValues,
            RatingTimeStamps = _timeStamps,
            ChoiceTimeStamp = currentTimeStamp,
            Choice = _value
         };
      
         HitEvent?.Invoke(this,frame);
         readyToAccept = false;
         LockAcceptButton();
         Restart();

         whenRatingFinished.Invoke();
      }
   }

   public void AbortRating()
   {
      RatingBoardDataFrame frame = new RatingBoardDataFrame()
      {
         Approved = false,
         Ratings = _choiceValues,
         RatingTimeStamps = _timeStamps,
         ChoiceTimeStamp = 0.0f,
         Choice = 0
      };

      HitEvent?.Invoke(this,frame);
   }
   
   
   public double GetCurrentUnixTimeStamp()
   {
      DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
      return (DateTime.UtcNow - epochStart).TotalSeconds;
   }

   public void Deactivate()
   {
      // todo deactivate the system
      throw new NotImplementedException();
   }

   public void Activate()
   {
      // todo activate the system
      throw new NotImplementedException();
   }
}
