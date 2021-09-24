using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationDataFrame
{
    //experiment Relevant
    public string stationID;

    public string participantID;

    public string condition;

    public string pakourOrder;

    public int stationIndex;
    //General Time relevant Data
    public double TeleportStartTimeStamp;
    public double PakourStartTimeStamp;
    public double PakourEndTimeStamp;
    public double PakourDuration;
    
    
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


    //Teleportation was initialized?

    public bool wasTeleportedToEnd;
    public double TeleportationInitalized;
    
  
}
