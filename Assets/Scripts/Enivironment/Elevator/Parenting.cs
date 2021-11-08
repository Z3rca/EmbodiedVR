using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenting : MonoBehaviour
{
    private Transform player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            player = other.GetComponent<HybridCharacterController>().GetGeneralControl().transform;
            player.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            player.parent = null;
        }
    }
}
