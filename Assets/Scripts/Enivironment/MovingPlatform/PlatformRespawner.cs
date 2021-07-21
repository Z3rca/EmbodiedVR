using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRespawner : MonoBehaviour
{
    public GameObject SpawnPosition;

    public bool DeleteOnExit;
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MovingPlatform>() != null)
        {
            if (DeleteOnExit)
            {
                other.gameObject.SetActive(false);
            }
            else
            {
                other.gameObject.transform.position = SpawnPosition.transform.position;
            }
        }
            
        
    }
}
