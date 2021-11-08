using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Screw : MonoBehaviour
{
    public Transform EndPoint;

    public UnityEvent OnScrewLoose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,EndPoint.position)<0.02f)
        {
            OnScrewLoose.Invoke();
        }
    }
}
