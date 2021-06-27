using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSpawner : MonoBehaviour
{
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        ExperimentManager.Instance.RegisterSpawnerToList(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
}
