﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    
    
    public GameObject Player;

    public StationSpawner ActiveStation;
    private List<StationSpawner> RemainingstationsStationSpawners =new List<StationSpawner>();

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
//        Player = FindObjectOfType<HybridControl>().gameObject;
    }

    public void TakeParticipantToNextStation()
    {
        RemainingstationsStationSpawners.Remove(ActiveStation);
        
        if (StationIndex > StationOrder.Count)
        {
            StationIndex++;
        }

        ActiveStation= RemainingstationsStationSpawners[StationOrder[StationIndex]];


        Player.GetComponentInChildren<PhysicalMovement>().TeleportToPosition(ActiveStation.transform.position);
        
    }


    public void RegisterSpawnerToList(StationSpawner spawner)
    {
        RemainingstationsStationSpawners.Add(spawner);
    }
}
