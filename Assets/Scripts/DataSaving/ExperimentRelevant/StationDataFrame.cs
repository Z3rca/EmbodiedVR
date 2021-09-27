using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class StationDataFrame
{
    //experiment Relevant
    public string stationID;

    public string participantID;

    public string condition;

    public string pakourOrder;

    public int stationIndex;
    //General Time relevant Data
    public double TeleportationInitalizedTimeStamp; //beeing in the start room
    public double PakourStartTimeStamp;
    public double PakourEndTimeStamp;
    public double PakourDuration;
    
    //Teleportation was initialized?
    public double AbortTeleportStartTimeStamp;  //if the pakour wasn't finished, at which time was the teleport initialized
    public bool wasTeleportedToEnd;
    
    
    //Datagathering room
    public double DataGatheringRoomEnteredTimeStamp;
    public double RatingBoardReachedTimeStamp;
    
    //Audio Data Related
    public double AudioRecordStarted;
    public double AudioRecordEnded;
    public string NameOfAudioData;
    //Motionsickness Reladed
    public double MotionsicknessScoreRatingBegin;
    public int MotionsicknessScore;
    public double MotionsicknessScoreRatingAcceptedTimeStamp;
    //Postural Stability Related
    public double PosturalStabilityTimeFrameBegin;
    public double PosturalStabilityTimeFrameEnd;


   
    
  
}
