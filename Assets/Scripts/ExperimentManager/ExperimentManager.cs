using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;

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
    private Dictionary<AreaManager, int> AreaManagers = new Dictionary<AreaManager, int>();

    public List<int> StationOrder;
    private int StationIndex;

    public TutorialManager tutorialManager;


    public event EventHandler<StartExperimentArgs> startedExperiment;
    public event EventHandler<ExperimentFinishedArgs> FinishedExperiment;

    public event EventHandler<ParkourBeginArgs> OnPakourBegin; 
    public event EventHandler<ParkourEndArgs> OnPakourFinished; 
    public event EventHandler<DataGatheringBeginArgs> stationCompleted;
    
    
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

        PakourBegins();
        
        _playerController.Fading(0.5f,0.5f,0.5f);
        
    }

   
    public Condition GetCondition()
    {
        return _condition;
    }
    
    public void TakeParticipantToNextStation()
    {
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
                _ActiveStation = stationSpawner;
                
            }
        }

        if (_playerController == null)
        {
            _playerController = SelectedAvatar.GetComponent<HybridController>();

        }

        _playerController.TeleportToPosition(_ActiveStation.gameObject.transform);
    }


    public void RegisterSpawnerToList(StationSpawner spawner)
    {
        AvaibleStationSpawners.Add(spawner);
    }

    public void RegisterAreaManager(AreaManager manager)
    {
        AreaManagers.Add(manager, manager.id);
    }

   
    
    private void StartedExperiment()
    {
        StartExperimentArgs startExperimentArgs = new StartExperimentArgs();
        startExperimentArgs.CharacterController = _playerCharacterController;
        startExperimentArgs.Order = order;
        startExperimentArgs.ApplicationStartTime = TimeManager.Instance.GetApplicationStartTime();
        startExperimentArgs.ExperimentStartTime = TimeManager.Instance.GetCurrentUnixTimeStamp();

        if (startedExperiment != null)
        {
            startedExperiment.Invoke(this,startExperimentArgs);
        }
        else
        {
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
        }
    }

    private void PakourBegins()
    {
        ParkourBeginArgs parkourBeginArgs = new ParkourBeginArgs();
        parkourBeginArgs.participantID = _participantId;
        parkourBeginArgs.Condition = _condition;
        parkourBeginArgs.stationID = _ActiveStation.ID;
        parkourBeginArgs.TeleportTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        OnPakourBegin.Invoke(this,parkourBeginArgs);
    }

    
    
    public void PakourEnds()
    {
        ParkourEndArgs parkourEndArgs = new ParkourEndArgs();
        parkourEndArgs.StationStartTime = _activeAreaManager.ParkourStartTime;
        parkourEndArgs.StationEndTime = _activeAreaManager.ParkourEndTime;
        parkourEndArgs.StationID = _activeAreaManager.id;
        parkourEndArgs.Condition = GetCondition();

        if (OnPakourFinished != null)
            OnPakourFinished.Invoke(this, parkourEndArgs);
        else
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
    }


    public void DataGatheringBegins()
    {
        
        DataGatheringBeginArgs dataGatheringBeginArgs = new DataGatheringBeginArgs();
        dataGatheringBeginArgs.ParticipantID = _participantId;
        dataGatheringBeginArgs.StationID = _ActiveStation.ID;
        dataGatheringBeginArgs.Condition = _condition;
        dataGatheringBeginArgs.EnteredDataGatheringRoomStart = _activeAreaManager.ReachedDataGatheringRoomTime;
        dataGatheringBeginArgs.ReachedVotingBoard = _activeAreaManager.ReachedRatingBoardTime;
    }


    public void DataGatheringEnds()
    {
        DataGatheringEndArgs dataGatheringEndArgs = new DataGatheringEndArgs();
        dataGatheringEndArgs.ParticipantID = _participantId;
        dataGatheringEndArgs.StationID = _ActiveStation.ID;
        dataGatheringEndArgs.Condition = _condition;
        dataGatheringEndArgs.DataGatheringStarted = _activeAreaManager.StartDataGatheringTime;
        dataGatheringEndArgs.DataGatheringEnded = _activeAreaManager.EndDataGatheringTime;
        
        dataGatheringEndArgs.MotionSicknessScore = _activeAreaManager.choiceValue;
        dataGatheringEndArgs.MotionSicknessScoreRatingAcceptedTime = _activeAreaManager.choiceTimeStamp;
        
        dataGatheringEndArgs.StartingAudioRecordTimeStamp = _activeAreaManager.StartAudioRecordTime;
        dataGatheringEndArgs.EndedAudioRecordingTimeStamp = _activeAreaManager.EndAudioRecordTime;
        dataGatheringEndArgs.NameOfAudioFile = _activeAreaManager.AudioStringName;

        dataGatheringEndArgs.PostureTestStartTime = _activeAreaManager.PosturalTestStartTime;
        dataGatheringEndArgs.PostureTestEndTime = _activeAreaManager.PosturalTestEndTime;
    }

    public void FinishExperiment()
    {
        ExperimentFinishedArgs experimentFinishedArgs = new ExperimentFinishedArgs();
        experimentFinishedArgs.ParticipantID = _participantId;
        experimentFinishedArgs.Condition = _condition;
        experimentFinishedArgs.ExperimentEndTime = TimeManager.Instance.GetCurrentUnixTimeStamp();
        experimentFinishedArgs.Condition = GetCondition();
        
        
        if (FinishedExperiment != null)
            FinishedExperiment.Invoke(this, experimentFinishedArgs);
        else
            Debug.LogWarning("WARNING DATA EVENT HAS NO LISTENER");
        // TODO finish experiment Logic
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

public class ParkourBeginArgs : EventArgs
{
    public int stationID;
    public string participantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    public double TeleportTime;
}
public class ParkourEndArgs : EventArgs
{
    public int StationID;
    public string ParticipantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    public double StationStartTime;
    public double StationEndTime;
    public bool wasTeleportedToEnd;
}

public class DataGatheringBeginArgs : EventArgs
{
    public int StationID;
    public string ParticipantID; 
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    
    public double EnteredDataGatheringRoomStart;
    public double ReachedVotingBoard;
}

public class DataGatheringEndArgs : EventArgs
{
    public int StationID;
    public string ParticipantID;
    public ExperimentManager.Condition Condition;
    public int OrderIndex;
    
    
    
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




