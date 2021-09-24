﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataRecorder : MonoBehaviour
{
    private ELIVRDataFrame _currentDataFrame;
    private StationDataFrame _currentStationDataFrame;
    
    void Start()
    {
        ExperimentManager.Instance.startedExperiment += OnStartedExperiment;
        ExperimentManager.Instance.OnPakourBegin += OnPakourBegin;
    }


    void OnStartedExperiment(object sender, StartExperimentArgs startExperimentArgs)
    {
        _currentDataFrame = new ELIVRDataFrame();

        _currentDataFrame.applicationStartTimestamp = startExperimentArgs.ApplicationStartTime;
        _currentDataFrame.ExperimentStartTimestamp = startExperimentArgs.ExperimentStartTime;
        _currentDataFrame.participantID = startExperimentArgs.ParticipantID;
        _currentDataFrame.Order = startExperimentArgs.Order;
        _currentDataFrame._stationDataFrames = new List<StationDataFrame>();
        DataSavingManager.Instance.Save(_currentDataFrame, startExperimentArgs.ParticipantID + " - " + _currentDataFrame.ExperimentStartTimestamp);

    }

    void OnPakourBegin(object sender, StationBeginArgs stationBeginArgs)
    {
       
        Debug.Assert(_currentDataFrame != null && _currentDataFrame._stationDataFrames != null,
            "Experiment hasnt been started, can't record Station Data");
        
        Debug.Assert(_currentStationDataFrame ==null, "current station data frame is still intact, cannot create new one yet");

        _currentStationDataFrame = new StationDataFrame();

        _currentStationDataFrame.participantID = stationBeginArgs.participantID;

        _currentStationDataFrame.condition = stationBeginArgs.Condition.ToString();
        _currentStationDataFrame.TeleportStartTimeStamp = stationBeginArgs.TeleportTimeFromLastStationTimeStamp;
        _currentStationDataFrame.stationIndex = stationBeginArgs.OrderIndex;
        DataSavingManager.Instance.Save(_currentStationDataFrame," tmp "+  _currentStationDataFrame.participantID   +" - "  + stationBeginArgs.Order+  " - " + stationBeginArgs.OrderIndex);
        
    }

    void OnPakourEnds(object sender, ParkourEndArgs parkourEndArgs)
    {
        if (_currentStationDataFrame == null)
        {
            Debug.LogWarning("The Pakour start  have been corrupted, abort");
        }

        _currentStationDataFrame.PakourEndTimeStamp = parkourEndArgs.StationEndTime;
        _currentStationDataFrame.PakourDuration = -_currentStationDataFrame.PakourEndTimeStamp -
                                                  _currentStationDataFrame.PakourStartTimeStamp;

        _currentStationDataFrame.wasTeleportedToEnd = parkourEndArgs.wasTeleportedToEnd;
        _currentStationDataFrame.TeleportStartTimeStamp = parkourEndArgs.wasTeleportedToEndTimeStamp;

    }

    private void OnDataGatheringRoomCompleted(object sender, DataGatheringEndArgs dataGatheringEndArgs)
    {
        if (_currentStationDataFrame == null)
        {
            Debug.LogWarning("Pakour data not found, aborted");
        }
        
        _currentStationDataFrame.RatingBoardReachedTimeStamp = dataGatheringEndArgs.ReachedVotingBoard;
        _currentStationDataFrame.DataGatheringRoomEnteredTimeStamp = dataGatheringEndArgs.EnteredDataGatheringRoom;
        _currentStationDataFrame.NameOfAudioData = dataGatheringEndArgs.NameOfAudioFile;
        _currentStationDataFrame.PosturalStabilityTimeFrameBegin = dataGatheringEndArgs.PostureTestStartTime;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
