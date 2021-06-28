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
    private List<StationSpawner> RemainingstationsStationSpawners =new List<StationSpawner>();
    private List<AreaManager> AreaManagers = new List<AreaManager>();

    public  List<int> StationOrder;
    private int StationIndex;


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
}
