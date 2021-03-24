using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteVR : MonoBehaviour
{
    public GameObject LocalHMD;
    public GameObject LocalLeft;
    public GameObject LocalRight;
    
    public GameObject RemoteHMD;

    public GameObject RemoteLeft;

    public GameObject RemoteRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void FixedUpdate()
    {
        RemoteHMD.transform.localPosition = LocalHMD.transform.localPosition;
        RemoteHMD.transform.localRotation = LocalHMD.transform.localRotation;

        RemoteLeft.transform.localPosition = LocalLeft.transform.localPosition;
        RemoteLeft.transform.localRotation = LocalLeft.transform.localRotation;

        RemoteRight.transform.localPosition = LocalRight.transform.localPosition;
        RemoteRight.transform.localRotation = LocalRight.transform.localRotation;
    }
}
