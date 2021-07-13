using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MovingPlatform>()!=null)
          //rigidbody.velocity = Vector3.zero;
          other.gameObject.transform.Translate(0,0,0);
    }
}
