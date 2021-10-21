﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;
using Valve.VR;

public class ExperimentManager : MonoBehaviour
{

    public Camera mainMenuCamera;
    public static ExperimentManager Instance { get; private set; }

    public List<GameObject> Avatars;
    public GameObject SelectedAvatar;
    private HybridController _playerController;
    public HybridCharacterController _playerCharacterController;

    private StationSpawner _ActiveStation;
    private AreaManager _activeAreaManager;
    private List<StationSpawner> AvaibleStationSpawners = new List<StationSpawner>();
    private List<StationSpawner> RemainingstationsStationSpawners = new List<StationSpawner>();
    private Dictionary<int, AreaManager> AreaManagers = new Dictionary<int, AreaManager>();

    private bool _gettingToNewStation;
    public List<int> StationOrder;
    public int StationIndex;

    public TutorialManager tutorialManager;

    [SerializeField] private int _defaultRecordingTime=300;

    [SerializeField] private MicrophoneManager _microphoneManager;

    [SerializeField] private LiveDataRecorder liveDataRecorder;
    

    public event EventHandler<StartExperimentArgs> startedExperiment;
    public event EventHandler<ExperimentFinishedArgs> FinishedExperiment;

    public event EventHandler<StationBeginArgs> OnStationBegin;
    public event EventHandler<PakourBeginArgs> OnPakourBegin;
    public event EventHandler<ParkourEndArgs> OnPakourFinished;

    public event EventHandler<DataGatheringEndArgs> OnDataGatheringCompleted;
    
     
    private event Action OnDataSavingCompleted; 
    
    private double ExperimentStartTime;



    private Condition _condition;
    private MenuState _menuState;
    private string _participantId;
    private string order;
    private string condition;

    private float totalTime;
    private bool runningExperiment;

    
    private enum MenuState
    {
        IDAndOrderSelection,
        Condition,
        MainMenu,
        SafetyBeforeStart,
        Running
    }


    public enum Condition
    {
        Hybrid,
        FirstPerson,
        Blob,
        Bodiless
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        if(runningExperiment)
            totalTime += Time.deltaTime;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        
    }

    private void Start()
    {
        OnDataSavingCompleted += TakeParticipantToNextStation;
        _microphoneManager = GetComponentInChildren<MicrophoneManager>();
    }
    

    private void StartExperiment()
    {
        runningExperiment = true;

        foreach (var stationSpawner in AvaibleStationSpawners)
        {
            foreach (var id in StationOrder)
            {
                if (stationSpawner.ID == id)
                {
                    RemainingstationsStationSpawners.Add(stationSpawner);
                }
            }
            
        }

        foreach (var station in RemainingstationsStationSpawners)
        {
            if (station.ID == StationOrder[0])
            {
                _ActiveStation = station;
                _activeAreaManager = AreaManagers[station.ID];
                Debug.Log(_activeAreaManager);
            }
        }

        mainMenuCamera.gameObject.SetActive(false);

        
        InstantiatePlayerOnStation();
        
    }

    private void InstantiatePlayerOnStation()
    {
        StartCoroutine(PlayerInstantiation());
    }

    private IEnumerator PlayerInstantiation()
    {
        SelectedAvatar.gameObject.SetActive(true);
        
        yield return new WaitUntil(() => SelectedAvatar.GetComponent<HybridController>() != null);

        Debug.Log("finished establishing character");
        _playerController = SelectedAvatar.GetComponent<HybridController>();
        _playerCharacterController = _playerController.GetHybridChracterController();

        
        
        _playerController.ShowControllers(false);
        
        if (_ActiveStation.ID == 0)
        {
            tutorialManager.StartTutorial();
        }
        else
        {
            _playerController.AllowViewSwitch(true);
            _playerController.AllowInput(true);
        }
        
        
        StartedExperiment();


        yield return new WaitForFixedUpdate();
        
        _playerController.TeleportToPosition(_ActiveStation.gameObject.transform);
        
        liveDataRecorder.Initialize();

        StationBegin();
        
        _playerController.Fading(0.5f,0.5f,0.5f);
        
        
        
    }

   
    public Condition GetCondition()
    {
        return _condition;
    }
    
    public void TakeParticipantToNextStation()
    {
        if (_gettingToNewStation) return;
        _gettingToNewStation = true;
        Debug.Log("Take to the next chapter my friends");
            
        RemainingstationsStationSpawners.Remove(_ActiveStation);
        if (!RemainingstationsStationSpawners.Any())
        {
            FinishExperiment();
            Debug.Log("Finished condition");
        }
        
        
        
        StationIndex++;

        if (StationIndex > StationOrder.Count)
        {
            Debug.Log("no teleport, finished");
        }

        foreach (var stationSpawner in RemainingstationsStationSpawners)
        {
            if (stationSpawner.ID == StationOrder[StationIndex])
            {
                Debug.Log("we found the according new area manager ");
                _ActiveStation = stationSpawner;
                _activeAreaManager = null;
                _activeAreaManager = AreaManagers[StationOrder[StationIndex]];
            }
        }

        if (_playerController == null)
        {
            _playerController = SelectedAvatar.GetComponent<HybridController>();

        }
        
        
        StationBegin();
        _playerController.TeleportToPosition(_ActiveStation.gameObject.transform);
        
        
    }

    public HybridController GetPlayerController()
    {
        return _playerController;
    }

    public void TeleportComplete()
    {
        _gettingToNewStation = false;
    }
    public void RegisterSpawnerToList(StationSpawner spawner)
    {
        AvaibleStationSpawners.Add(spawner);
    }

    public void RegisterAreaManager(AreaManager manager)
    {
        AreaManagers.Add(manager.id,manager );
        
    }

    
    
    private void StartedExperiment()
    {
        Debug.Log(AreaManagers.Count+  " managers");
        StartExperimentArgs startExperimentArgs = new StartExperimentArgs();
        startExperimentArgs.CharacterController = _playerCharacterController;
        startExperimentArgs.Order = order;
        startExperimentArgs.ApplicationStartTime = TimeManager.Instance.GetApplicationStartTime();
        startExperimentArgs.ExperimentStartTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        startExperimentArgs.ParticipantID = _participantId;

        if (startedExperiment != null)
        {
            startedExperiment.Invoke(this,startExperimentArgs);
        }
        else
        {
           
        }
    }

    private void StationBegin()
    {
        liveDataRecorder.StartRecording();
        
        StationBeginArgs stationBeginArgs = new StationBeginArgs();
        stationBeginArgs.participantID = _participantId;
        stationBeginArgs.Condition = _condition;
        stationBeginArgs.stationID = _ActiveStation.ID;
        stationBeginArgs.Order = order;
        stationBeginArgs.OrderIndex = StationIndex;
        
        stationBeginArgs.TeleportTimeFromLastStationTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        if (OnStationBegin != null)
        {
            OnStationBegin.Invoke(this, stationBeginArgs);
        }
        else
        {
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
        }
    }

    public void PakourBegin()
    {
        PakourBeginArgs pakourBeginArgs = new PakourBeginArgs();
        pakourBeginArgs.Condition = _condition;
        pakourBeginArgs.ParticipantID = _participantId;
        pakourBeginArgs.OrderIndex = StationIndex;
        pakourBeginArgs.PakourStartTime = _activeAreaManager.parkourStartTimeStamp;
        OnPakourBegin.Invoke(this,pakourBeginArgs);
    }

    
    
    public void PakourEnds()
    {
        ParkourEndArgs parkourEndArgs = new ParkourEndArgs();
        parkourEndArgs.wasTeleportedToEnd = _activeAreaManager.wasTeleportedToEnd;
        parkourEndArgs.wasTeleportedToEndTimeStamp = _activeAreaManager.GetWasTeleportedTimeStamp();
        parkourEndArgs.PakourEndTime = _activeAreaManager.parkourEndTimeStamp;
        parkourEndArgs.StationID = _activeAreaManager.id;
        parkourEndArgs.Condition = GetCondition();

        if (OnPakourFinished != null)
            OnPakourFinished.Invoke(this, parkourEndArgs);
        else
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
    }

    public void StartMicrophoneRecording(int time)
    {
        _microphoneManager.StartRecording(time);
    }

    public void DataGatheringEnds()
    {
        DataGatheringEndArgs dataGatheringEndArgs = new DataGatheringEndArgs();
        dataGatheringEndArgs.ParticipantID = _participantId;
        dataGatheringEndArgs.StationID = _ActiveStation.ID;
        dataGatheringEndArgs.Condition = _condition;
        dataGatheringEndArgs.OrderIndex = StationIndex;

        dataGatheringEndArgs.EnteredDataGatheringRoom = _activeAreaManager.reachedDataGatheringRoomTimeStamp;
        dataGatheringEndArgs.ReachedVotingBoard = _activeAreaManager.reachedRatingBoardTimeStamp;

        dataGatheringEndArgs.DataGatheringStarted = _activeAreaManager.startDataGatheringTimeStamp;
        dataGatheringEndArgs.DataGatheringEnded = _activeAreaManager.endDataGatheringTimeStamp;
        
        dataGatheringEndArgs.MotionSicknessScore = _activeAreaManager.choiceValue;
        dataGatheringEndArgs.MotionSicknessScoreRatingBeginTimeStamp= _activeAreaManager.startMotionsicknessMeasurementTime;
        dataGatheringEndArgs.MotionSicknessScoreRatingAcceptedTime = _activeAreaManager.choiceTimeStamp;
        
        dataGatheringEndArgs.StartingAudioRecordTimeStamp = _activeAreaManager.GetAudioRecordingStart();
        dataGatheringEndArgs.EndedAudioRecordingTimeStamp = _activeAreaManager.GetAudioRecordingEnd();
        dataGatheringEndArgs.NameOfAudioFile = _activeAreaManager.GetAudioFileName();
        _microphoneManager.SetAudioFileName(_activeAreaManager.GetAudioFileName());

        dataGatheringEndArgs.PostureTestStartTime = _activeAreaManager.GetBeginPosturalStabilityTest();
        dataGatheringEndArgs.PostureTestEndTime = _activeAreaManager.GetEndPosturalStabilityTest();

        if (OnDataGatheringCompleted != null)
        {
            OnDataGatheringCompleted.Invoke(this,dataGatheringEndArgs);
        }
        else
        {
            Debug.LogWarning("DATA WASNT SAVED");
        }

        StartCoroutine(SaveDataCoroutine(1f));
    }

    public IEnumerator SaveDataCoroutine(float FadeOutDuration)
    {
        SelectedAvatar.GetComponent<HybridController>().FadeOut(FadeOutDuration/2);
        yield return new WaitForSeconds(FadeOutDuration);
        liveDataRecorder.StopRecording();
        
        liveDataRecorder.SaveData();
        _microphoneManager.SaveAudioClip();
        
        
        yield return new WaitForSeconds(FadeOutDuration / 2);
        OnDataSavingCompleted.Invoke();
        liveDataRecorder.ClearData();
        _microphoneManager.ClearData();
        SelectedAvatar.GetComponent<HybridController>().FadeIn(0.5f);
    }

    public void FinishExperiment()
    {
        ExperimentFinishedArgs experimentFinishedArgs = new ExperimentFinishedArgs();
        experimentFinishedArgs.ParticipantID = _participantId;
        experimentFinishedArgs.Condition = _condition;
        experimentFinishedArgs.ExperimentEndTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        experimentFinishedArgs.Condition = GetCondition();
        
        SelectedAvatar.GetComponent<HybridController>().FadeOut(2f);
        if (FinishedExperiment != null)
            FinishedExperiment.Invoke(this, experimentFinishedArgs);
        else
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
        
        
        // TODO finish experiment Logic
    }


    public string GetParticipantID()
    {
        return _participantId;
    }

    private void ReadOutPakourOrder(string Order)
    {
        var charArray = Order.ToCharArray();
        order = "";
        for (int i = 0; i < charArray.Length; i++)
        {
            int j = Convert.ToInt32(charArray[i]) - 48;         //ASCII to int
            
            Debug.Log(j);

            if (j < 0 && j <= 4)
            {
                order = "ERROR";
                StationOrder.Clear();
                return;
            }
           
            StationOrder.Add(j);
            order+=(j);
            


        }
        
        
    }

    public bool MicrophoneIsRecording()
    {
        return _microphoneManager.isRecording();
    }

    public void StopRecordingMicrophone()
    {
        _microphoneManager.StopRecording();
    }
    
    public int GetDefaultAudioRecordingTime()
    {
        return _defaultRecordingTime;
    }
    
    public AreaManager GetCurrentAreaManager()
    {
        return _activeAreaManager;
    }
    
    private void OnGUI()
    {
        var x = 100;
        var y = 100;
        var w = 200;
        var h = 50;

        GUI.backgroundColor = Color.cyan;
        GUI.skin.textField.fontSize = 30;

        var boxStyle = new GUIStyle(GUI.skin.box)
        {
            
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        var buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 28,
            alignment = TextAnchor.MiddleCenter
        };

        var valX = x;
        var valY = y;
        switch (_menuState)
        {
            case MenuState.IDAndOrderSelection:
                GUI.Box(new Rect(750, 490, 200, 40), "Participant ID", boxStyle);
                _participantId = GUI.TextField(new Rect(750, 530, 200, 40), _participantId);

                GUI.Box(new Rect(970, 490, 200, 40), "Order", boxStyle);
                order = GUI.TextField(new Rect(970, 530, 200, 40), order);

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    //Save Order temporary
                    //Save Participant ID temporary

                    ReadOutPakourOrder(order);
                    
                    
                    
                    _menuState = MenuState.Condition;

                }

                break;
            case MenuState.Condition:

                valX = x;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ _participantId), boxStyle);

                valX += w+ 2;
                GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Order: "+ order), boxStyle);

                GUI.backgroundColor = Color.red;

                valX += w + 2;

                if (GUI.Button(new Rect(valX, 100, w, 80), "Change", buttonStyle))
                {
                    _menuState = MenuState.IDAndOrderSelection;
                }
                
                GUI.backgroundColor = Color.cyan;


                if (GUI.Button(new Rect(x, Screen.height/2, w, 80), "Hybrid", buttonStyle))
                {
                    condition = "Hybrid";
                    _condition = Condition.Hybrid;
                    SelectedAvatar = Avatars[0];
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*3.5f, Screen.height/2, w, 80), "Hybrid(Blob)", buttonStyle))
                {
                    SelectedAvatar = Avatars[2];
                    condition = "Hybrid(Blob)";
                    _condition = Condition.Blob;
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*6, Screen.height/2, w, 80), "First-person", buttonStyle))
                {
                    SelectedAvatar = Avatars[1];
                    condition = "First-person";
                    _condition = Condition.FirstPerson;
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*8.5f, Screen.height/2, w*1.3f, 80), "Bodiless", buttonStyle))
                {
                    SelectedAvatar = Avatars[3];
                    condition = "First-Person(Blob)";
                    _condition = Condition.Bodiless;
                    _menuState = MenuState.MainMenu;
                }
                
                
                break;
            case MenuState.MainMenu:
                valX = x;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ _participantId), boxStyle);

                valX += w+ 2;
                GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Order: "+ order), boxStyle);

                GUI.backgroundColor = Color.red;

                valX += w + 2;

                if (GUI.Button(new Rect(valX, 100, w, 80), "Change", buttonStyle))
                {
                    _menuState = MenuState.IDAndOrderSelection;
                }
                
                valX += w;
                
                

                valX += w+2;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent(condition), boxStyle);
                
                valX += w+2;
                
                if (GUI.Button(new Rect(valX, 100, w, 80), "Change", buttonStyle))
                {
                    _menuState = MenuState.Condition;
                }
                
                
                GUI.backgroundColor = Color.cyan;
                valX = x;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Calibration", buttonStyle))
                {
                   //launch Calibration and Validation of the Eyetracker
                }
                
                valX += w + 2;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Validation", buttonStyle))
                {
                    //singe validation
                }
                
                valX += w + 2 ;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w*1.25f, 80), "Start Experiment", buttonStyle))
                {
                    _menuState = MenuState.SafetyBeforeStart;
                }
                break;
            
                case MenuState.SafetyBeforeStart:
                    
                    valX = x;
                    GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ _participantId), boxStyle);

                    valX += w+ 2;
                    GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Order: "+ order), boxStyle);

                    GUI.backgroundColor = Color.red;

                    valX += w + 2;

                    if (GUI.Button(new Rect(valX, 100, w, 80), "Change", buttonStyle))
                    {
                        _menuState = MenuState.IDAndOrderSelection;
                    }
                
                    valX += w;
                
                

                    valX += w+2;
                    GUI.Box(new Rect(valX, 100, w, 80), new GUIContent(condition), boxStyle);
                
                    valX += w+2;
                
                    if (GUI.Button(new Rect(valX, 100, w, 80), "Change", buttonStyle))
                    {
                        _menuState = MenuState.Condition;
                    }
                    
                    
                    valX = x;
                    
                GUI.Box(new Rect(valX, Screen.height/2-y, w*1.5f, 80), new GUIContent("Are you Sure?"), boxStyle);
                    
                    GUI.backgroundColor = Color.green;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Yes", buttonStyle))
                {
                    _menuState = MenuState.Running;
                    StartExperiment();
                }
                valX += w;
                    GUI.backgroundColor = Color.red;
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "No", buttonStyle))
                {
                    _menuState = MenuState.MainMenu;
                }

                break;
                    
                case MenuState.Running:
                    
                valX = x;
               
                int buttonwidth =(int) (w * 2);
                GUI.Box(new Rect(valX, valY, buttonwidth, 80), new GUIContent("ID: "+ _participantId), boxStyle);

                valX += buttonwidth+ 2;
                GUI.Box(new Rect(valX , valY, buttonwidth, 80), new GUIContent("Order: "+ order), boxStyle);
                
                valX += buttonwidth +2;
                GUI.Box(new Rect(valX , valY, buttonwidth, 80), new GUIContent("Station: "+ _ActiveStation.ID), boxStyle);

                valX = x;
                valY = y + 100;
                
                valX += buttonwidth+ 2;
                GUI.Box(new Rect(valX , valY, buttonwidth, 80), new GUIContent("Time Station: "+ _ActiveStation.ID), boxStyle);

                valX += buttonwidth+ 2;
                TimeSpan time = TimeSpan.FromSeconds(totalTime);
                GUI.Box(new Rect(valX , valY, buttonwidth, 80), new GUIContent("Time Total "+ time.ToString("mm':'ss")), boxStyle);

                break;
                
        }
    }
}



public class StartExperimentArgs : EventArgs
{
    public HybridCharacterController CharacterController;
    public ExperimentManager.Condition Condition;
    public double ApplicationStartTime;
    public double ExperimentStartTime;
    public string ParticipantID;
    public string Order;
}

public class ExperimentFinishedArgs : EventArgs
{
    public string ParticipantID;
    public HybridCharacterController CharacterController;
    public ExperimentManager.Condition Condition;
    public double ExperimentEndTime;
}

public class StationBeginArgs : EventArgs
{
    public int stationID;
    public string participantID;
    public ExperimentManager.Condition Condition;
    public string Order;
    public int OrderIndex;
    public double TeleportTimeFromLastStationTimeStamp;
}

public class PakourBeginArgs : EventArgs
{
    public int StationID;
    public string ParticipantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    public double PakourStartTime;
}
public class ParkourEndArgs : EventArgs
{
    public int StationID;
    public string ParticipantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    public double PakourEndTime;
    public bool wasTeleportedToEnd;
    public double wasTeleportedToEndTimeStamp;
}

public class DataGatheringEndArgs : EventArgs
{
    public int StationID;
    public string ParticipantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    
    public double EnteredDataGatheringRoom;
    public double ReachedVotingBoard;
    
    public double StartingAudioRecordTimeStamp;
    public string NameOfAudioFile;
    public double EndedAudioRecordingTimeStamp;
    
    public double MotionSicknessScoreRatingBeginTimeStamp;
    public int MotionSicknessScore;
    public double MotionSicknessScoreRatingAcceptedTime;
    
    
    public double PostureTestStartTime;
    public double PostureTestEndTime;
    
    public double DataGatheringStarted;
    public double DataGatheringEnded;
}




