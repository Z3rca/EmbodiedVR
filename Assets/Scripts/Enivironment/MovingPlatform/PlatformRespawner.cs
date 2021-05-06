using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRespawner : MonoBehaviour
{
    public GameObject SpawnPosition;
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<MovingPlatform>()!=null)
            other.gameObject.transform.position = SpawnPosition.transform.position;
    }
}
