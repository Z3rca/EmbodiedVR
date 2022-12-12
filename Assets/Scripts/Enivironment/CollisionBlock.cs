using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlock : MonoBehaviour
{
    public GameObject Cube;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Cube)
        {
            this.gameObject.SetActive(false);
        }
    }
}
