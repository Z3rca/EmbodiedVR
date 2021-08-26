using System.Collections;
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

    void OnPakourBegin(object sender, ParkourBeginArgs parkourBeginArgs)
    {
       
        Debug.Assert(_currentDataFrame != null && _currentDataFrame._stationDataFrames != null,
            "Experiment hasnt been started, can't record Station Data");
        
        Debug.Assert(_currentStationDataFrame ==null, "current station data frame is still intact, cannot create new one yet");

        _currentStationDataFrame = new StationDataFrame();

        _currentStationDataFrame.participantID = parkourBeginArgs.participantID;

        _currentStationDataFrame.condition = parkourBeginArgs.Condition.ToString();
        _currentStationDataFrame.TeleportStartTimeStamp = parkourBeginArgs.TeleportTime;
        _currentStationDataFrame.stationIndex = parkourBeginArgs.OrderIndex;
        
        DataSavingManager.Instance.Save(_currentStationDataFrame," tmp "+  parkourBeginArgs.participantID  +" - "  + parkourBeginArgs.Order+  " - " + parkourBeginArgs.OrderIndex);
        
    }
    
    void OnPakourFinished(object sender, ParkourEndArgs parkourEndArgs)
    {
        _currentStationDataFrame.PakourEndTimeStamp = parkourEndArgs.StationEndTime;
        _currentStationDataFrame.PakourDuration = _currentStationDataFrame.PakourStartTimeStamp -
                                                  _currentStationDataFrame.PakourEndTimeStamp;
        
    }
    
    void OnVotingBoardReached(object sender, ParkourEndArgs parkourEndArgs)
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
