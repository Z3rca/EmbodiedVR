using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance { get; private set; }
    
    
    public GameObject Player;

    public GameObject ActiveStation;
    public List<StationSpawner> RemainingstationsStationSpawners;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    public void TakeParticipantToNextStation()
    {
        Player.GetComponent<PhysicalMovement>().TeleportToPosition(Remainingstations[Remainingstations.Count].transform.position);
    }


    public void RegisterSpawnerToList(StationSpawner spawner)
    {
        RemainingstationsStationSpawners.Add(spawner);
    }
}
