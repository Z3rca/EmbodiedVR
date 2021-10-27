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
        ExperimentManager.Instance.OnStationBegin += OnStationBegin;
        ExperimentManager.Instance.OnPakourBegin += OnPakourBegin;
        ExperimentManager.Instance.OnPakourFinished += OnPakourEnds;
        ExperimentManager.Instance.OnDataGatheringCompleted += OnDataGatheringRoomCompleted;
        ExperimentManager.Instance.FinishedExperiment += OnFinishExperiment;
    }

    private void OnFinishExperiment(object sender, ExperimentFinishedArgs e)
    {
        _currentDataFrame.ExperimentFinishTimeStamp = e.ExperimentEndTime;
        
        DataSavingManager.Instance.Save(_currentDataFrame, _currentDataFrame.participantID +" " + _currentDataFrame.Condition +" "+ _currentDataFrame.Order);
    }


    void OnStartedExperiment(object sender, StartExperimentArgs startExperimentArgs)
    {
        _currentDataFrame = new ELIVRDataFrame();
        _currentDataFrame.Condition = ExperimentManager.Instance.GetCondition().ToString();
        _currentDataFrame.applicationStartTimestamp = startExperimentArgs.ApplicationStartTime;
        _currentDataFrame.ExperimentStartTimestamp = startExperimentArgs.ExperimentStartTime;
        _currentDataFrame.participantID = startExperimentArgs.ParticipantID;
        _currentDataFrame.Order = startExperimentArgs.Order;
        _currentDataFrame._stationDataFrames = new List<StationDataFrame>();
        DataSavingManager.Instance.Save(_currentDataFrame, startExperimentArgs.ParticipantID + " - " + _currentDataFrame.ExperimentStartTimestamp);

    }

    void OnStationBegin(object sender, StationBeginArgs stationBeginArgs)
    {
       
        Debug.Assert(_currentDataFrame != null && _currentDataFrame._stationDataFrames != null,
            "Experiment hasnt been started, can't record Station Data");
        
        //Debug.Assert(_currentStationDataFrame ==null, "current station data frame is still intact, cannot create new one yet");

        _currentStationDataFrame = new StationDataFrame();

        _currentStationDataFrame.participantID = stationBeginArgs.participantID;

        _currentStationDataFrame.condition = stationBeginArgs.Condition.ToString();
        _currentStationDataFrame.TeleportationInitalizedTimeStamp = stationBeginArgs.TeleportTimeFromLastStationTimeStamp;
        _currentStationDataFrame.stationIndex = stationBeginArgs.OrderIndex;
        DataSavingManager.Instance.Save(_currentStationDataFrame,"tmp_"+  _currentStationDataFrame.participantID   +"_stat_"+_currentStationDataFrame.condition+"_" + _currentStationDataFrame.stationIndex);
        
    }

    void OnPakourBegin(object sender, PakourBeginArgs pakourBeginArgs)
    {
        if (_currentStationDataFrame == null)
        {
            Debug.LogWarning("Pakour data start data not found, you miss  data");
            _currentStationDataFrame = new StationDataFrame();
        }

        _currentStationDataFrame.PakourStartTimeStamp = pakourBeginArgs.PakourStartTime;


        DataSavingManager.Instance.Save(_currentStationDataFrame,"tmp_"+  _currentStationDataFrame.participantID   +"_stat_"+_currentStationDataFrame.condition+"_" + _currentStationDataFrame.stationIndex);
    }
    
    void OnPakourEnds(object sender, ParkourEndArgs parkourEndArgs)
    {
        if (_currentStationDataFrame == null)
        {
            Debug.LogWarning("Pakour data start data not found, you miss  data");
            _currentStationDataFrame = new StationDataFrame();
        }
        
        _currentStationDataFrame.PakourEndTimeStamp = parkourEndArgs.PakourEndTime;
        _currentStationDataFrame.PakourDuration = _currentStationDataFrame.PakourEndTimeStamp -
                                                  _currentStationDataFrame.PakourStartTimeStamp;

        _currentStationDataFrame.wasTeleportedToEnd = parkourEndArgs.wasTeleportedToEnd;
        _currentStationDataFrame.AbortTeleportStartTimeStamp = parkourEndArgs.wasTeleportedToEndTimeStamp;
        DataSavingManager.Instance.Save(_currentStationDataFrame,"tmp_"+  _currentStationDataFrame.participantID   +"_stat_"+_currentStationDataFrame.condition+"_" + _currentStationDataFrame.stationIndex);
    }

    private void OnDataGatheringRoomCompleted(object sender, DataGatheringEndArgs dataGatheringEndArgs)
    {
        Debug.Log("save data of static data");
        if (_currentStationDataFrame == null)
        {
            Debug.LogWarning("Pakour data start data not found, you miss  data");
            _currentStationDataFrame = new StationDataFrame();
        }
        
        _currentStationDataFrame.RatingBoardReachedTimeStamp = dataGatheringEndArgs.ReachedVotingBoard;
        _currentStationDataFrame.DataGatheringRoomEnteredTimeStamp = dataGatheringEndArgs.EnteredDataGatheringRoom;
        
        
        //Audio releated
        _currentStationDataFrame.NameOfAudioData = dataGatheringEndArgs.NameOfAudioFile;
        _currentStationDataFrame.AudioRecordStarted = dataGatheringEndArgs.StartingAudioRecordTimeStamp;
        _currentStationDataFrame.AudioRecordEnded = dataGatheringEndArgs.EndedAudioRecordingTimeStamp;
        
        //motion sicknesss
        _currentStationDataFrame.MotionsicknessScore = dataGatheringEndArgs.MotionSicknessScore;
        _currentStationDataFrame.MotionsicknessScoreRatingBegin =
            dataGatheringEndArgs.MotionSicknessScoreRatingBeginTimeStamp;
        _currentStationDataFrame.MotionsicknessScoreRatingAcceptedTimeStamp =
            dataGatheringEndArgs.MotionSicknessScoreRatingAcceptedTime;
        //Postural stabiilty test

        _currentStationDataFrame.PosturalStabilityTimeFrameBegin = dataGatheringEndArgs.PostureTestStartTime;
        _currentStationDataFrame.PosturalStabilityTimeFrameEnd = dataGatheringEndArgs.PostureTestEndTime;
        
        
       // DataSavingManager.Instance.Save(_currentStationDataFrame," tmp "+  _currentStationDataFrame.participantID   +" - "  + _currentStationDataFrame.pakourOrder+  " - " + _currentStationDataFrame.stationIndex);
        _currentDataFrame._stationDataFrames.Add(_currentStationDataFrame);
        _currentStationDataFrame = null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
