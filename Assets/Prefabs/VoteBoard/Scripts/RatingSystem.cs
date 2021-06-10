﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingSystem : MonoBehaviour
{
   private int _value; //choice
   private double currentTimeStamp;
   private List<int> _choiceValues;
   private List<double> _timeStamps;
   
   
   [SerializeField] private string basic;
   [SerializeField] private Text textField;
   [SerializeField] private List<GameObject> RatingButtons;
   [SerializeField] private GameObject AcceptButton;
   public EventHandler<RatingBoardDataFrame> HitEvent;

   private bool readyToAccept;



   private void Start()
   {
      _choiceValues = new List<int>();
      _timeStamps = new List<double>();
   }
   
   public void SetValue(int val)
   {
      _value = val;
      SetText(_value);
      ResetColors();
      readyToAccept = true;
      SetButtonColored(RatingButtons[val-1]);
      UnlockAcceptButton();
      
   }
   
   private void SetText(int val)
   {
      textField.text= basic + ": "+ val;
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
      AcceptButton.GetComponent<ButtonMaterialModifier>().RestoreMaterial();
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
            Choice = 0
         };
      
         HitEvent?.Invoke(this,frame);
         readyToAccept = false;
         LockAcceptButton();
         Restart();
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
