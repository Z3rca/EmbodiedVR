using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }


    public GameObject Player;
    private PhysicalMovement _playerMovement;

    public StationSpawner ActiveStation;
    private List<StationSpawner> RemainingstationsStationSpawners = new List<StationSpawner>();
    private List<AreaManager> AreaManagers = new List<AreaManager>();

    public List<int> StationOrder;
    private int StationIndex;


    private enum MenuState
    {
        IDAndOrderSelection,
        Condition,
        MainMenu,
        SafetyBeforeStart,
        Running
    }

    private MenuState _menuState;
    private string participantId;
    private string order;
    private string condition;

    // Update is called once per frame
    void Update()
    {
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
        foreach (var stationSpawner in RemainingstationsStationSpawners)
        {
            if (stationSpawner.ID == StationOrder[0])
            {
                ActiveStation = stationSpawner;
            }
        }

        if (_playerMovement == null)
        {
            _playerMovement = Player.GetComponentInChildren<PhysicalMovement>();
        }

        _playerMovement.TeleportToPosition(ActiveStation.gameObject.transform.position);
    }

    public void TakeParticipantToNextStation()
    {
        RemainingstationsStationSpawners.Remove(ActiveStation);
        if (!RemainingstationsStationSpawners.Any())
        {
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
                ActiveStation = stationSpawner;
            }
        }

        if (_playerMovement == null)
        {
            _playerMovement = Player.GetComponentInChildren<PhysicalMovement>();
        }

        _playerMovement.TeleportToPosition(ActiveStation.gameObject.transform.position);
    }


    public void RegisterSpawnerToList(StationSpawner spawner)
    {
        RemainingstationsStationSpawners.Add(spawner);
    }

    public void RegisterAreaManager(AreaManager manager)
    {
        AreaManagers.Add(manager);
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
        switch (_menuState)
        {
            case MenuState.IDAndOrderSelection:
                GUI.Box(new Rect(750, 490, 200, 40), "Participant ID", boxStyle);
                participantId = GUI.TextField(new Rect(750, 530, 200, 40), participantId);

                GUI.Box(new Rect(970, 490, 200, 40), "Order", boxStyle);
                order = GUI.TextField(new Rect(970, 530, 200, 40), order);

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    //Save Order temporary
                    //Save Participant ID temporary


                    _menuState = MenuState.Condition;

                }

                break;
            case MenuState.Condition:

                valX = x;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ participantId), boxStyle);

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
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*3.5f, Screen.height/2, w, 80), "Hybrid(Blob)", buttonStyle))
                {
                    condition = "Hybrid(Blob)";
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*6, Screen.height/2, w, 80), "First-person", buttonStyle))
                {
                    condition = "First-person";
                    _menuState = MenuState.MainMenu;
                }
                if (GUI.Button(new Rect(x*8.5f, Screen.height/2, w*1.3f, 80), "First-person(Blob)", buttonStyle))
                {
                    condition = "First-Person(Blob)";
                    _menuState = MenuState.MainMenu;
                }
                
                
                break;
            case MenuState.MainMenu:
                valX = x;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ participantId), boxStyle);

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
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Calibration", buttonStyle))
                {
                   //launch Calibration and Validation of the Eyetracker
                }
                
                valX += w;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Calibration", buttonStyle))
                {
                    //singe validation
                }
                
                valX += w;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Start Experiment", buttonStyle))
                {
                    _menuState = MenuState.SafetyBeforeStart;
                }
                break;
            
                case MenuState.SafetyBeforeStart:
                    
                GUI.Box(new Rect(valX, Screen.height/2+y, w, 80), new GUIContent("Sure?"+ participantId), boxStyle);
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "Yes", buttonStyle))
                {
                    _menuState = MenuState.Running;
                    StartExperiment();
                }
                valX += w;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w, 80), "No", buttonStyle))
                {
                    _menuState = MenuState.MainMenu;
                }

                break;
                    
                case MenuState.Running:
                    
                valX = x;
                GUI.Box(new Rect(valX, 100, w, 80), new GUIContent("ID: "+ participantId), boxStyle);

                valX += w+ 2;
                GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Order: "+ order), boxStyle);
                
                valX += w+ 2;
                GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Station: "+ ActiveStation.ID), boxStyle);
                
                valX += w+ 2;
                GUI.Box(new Rect(valX , 100, w, 80), new GUIContent("Station: "+ ActiveStation.ID), boxStyle);
                
                
                

                    






                    break;
            case MenuState.Running:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}